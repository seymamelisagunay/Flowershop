using System;
using System.Collections.Generic;
using System.Collections;
using _Game.Script.Character;
using NaughtyAttributes;
using UnityEngine;
using MoreMountains.NiceVibrations;

/// <summary>
/// Bunun İçersinde Müşteriler Listelenecek ve bu Müşterilerden para alınacacak
/// 
/// </summary>
public class CashTradeController : MonoBehaviour
{
    public string playerPrefsKey = "CashTradeDesk";
    [Tag] public string playerTag;
    public float customerTradeTime;
    [ReadOnly] public List<CustomerController> customerQueue = new List<CustomerController>();
    public List<TradeWaitingPoint> customerQueueTargetPoints = new List<TradeWaitingPoint>();
    [HideInInspector] public GridSlotController gridSlotController;
    public bool isInPlayer;
    public int currentCurrency;
    private bool isContinueTrade;
    public bool isCashier;

    // Para kazanma
    public IntVariable tradeMoneyCount;
    private Coroutine resize;

    private void Start()
    {
        gridSlotController = GetComponentInChildren<GridSlotController>();
        gridSlotController.ReSize();
        tradeMoneyCount.Value = PlayerPrefs.GetInt(playerPrefsKey, 0);
        tradeMoneyCount.OnChangeVariable.AddListener(ChangeMoneyValue);
        var customerManager = FindObjectOfType<CustomerManager>();
        customerManager.cashTradeController = this;
        LoadMoney();
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadMoney()
    {
        var loadMoneyCount = tradeMoneyCount.Value / 10;
        for (int i = 0; i < loadMoneyCount; i++)
        {
            CreateMoney();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateMoney()
    {
        var grid = gridSlotController.GetPosition();
        if (grid == null) return;
        var clone = Instantiate(gridSlotController.sampleObject, gridSlotController.parent);
        clone.transform.localPosition = grid.slotPosition;
        grid.isFull = true;
        grid.slotInObject = clone;
        clone.gameObject.SetActive(true);
        clone.PlayScaleEffect(0.5f);
    }

    /// <summary>
    /// Clientlar buraya kendilerini sıraya sokmak için istekte bulunacaklar
    /// Burada Sıraya girmek isteyen clienta Girmesi gerekn sıra yeri verilecek.
    /// </summary>
    public void SetCustomerQueue(CustomerController customerController)
    {
        foreach (var point in customerQueueTargetPoints)
        {
            if (point.isFull) continue;
            StartCoroutine(customerController.SetTradePoint(point));
            break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void ChangeMoneyValue()
    {
        PlayerPrefs.SetInt(playerPrefsKey, tradeMoneyCount.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (isCashier) return;
        if (other.CompareTag("Cashier"))
        {
            isCashier = true;
            isInPlayer = true;
            StartCoroutine(StartCustomerSell(0));
        }

        if (!other.CompareTag(playerTag)) return;
        var player = other.GetComponent<PlayerController>();
        if (player.playerSettings.isBot) return;
        if (customerQueue.Count > 0)
        {
            player?.hudDotIdle?.gameObject.SetActive(true);
            player?.hudDotIdle?.Play();
            StartCoroutine(PlayThreeDotEffect(player));
        }

        isInPlayer = true;
        StartCoroutine(StartCustomerSell(player.playerSettings.firstTriggerCooldown));
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCashier) return;
        if (!other.CompareTag(playerTag)) return;
        var player = other.GetComponent<PlayerController>();
        if (player.playerSettings.isBot) return;
        isInPlayer = false;
        isContinueTrade = false;
        player?.hudDotIdle?.Stop();
        player?.hudDotIdle?.gameObject.SetActive(false);
    }

    private IEnumerator PlayThreeDotEffect(PlayerController player)
    {
        while (isInPlayer)
        {
            yield return new WaitUntil(() => customerQueue.Count <= 0);
            player?.hudDotIdle?.Stop();
            player?.hudDotIdle?.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstTriggerDuration"></param>
    /// <returns></returns>
    private IEnumerator StartCustomerSell(float firstTriggerDuration)
    {
        while (isInPlayer)
        {
            yield return new WaitForSeconds(0.01f);
            if (!isInPlayer) continue;
            if (customerQueue.Count <= 0) continue;
            yield return NextCustomerSell();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerator NextCustomerSell()
    {
        yield return ReSize();
        var currentClient = customerQueue[0];
        if (currentClient == null) yield break;
        //0. client hazır olduğunda işleme başlanılacak 
        yield return new WaitUntil(() => currentClient.isCashDeskReady);
        currentCurrency = currentClient.MoneyCalculator();
        yield return currentClient.SellingProducts();
        CurrentClientMoneyCalculate();
        SoundManager.instance.Play("payment");
        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
        customerQueue.Remove(currentClient);
        yield return ReSize(); // Burda Bekleyen Müşteriler Tekrar Yerleştirilmeli 
    }

    /// <summary>
    /// 
    /// </summary>
    private void CurrentClientMoneyCalculate()
    {
        var moneyObjectCount = currentCurrency / 10;
        tradeMoneyCount.Value += currentCurrency;
        for (int i = 0; i < moneyObjectCount; i++)
        {
            CreateMoney();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator ReSize()
    {
        customerQueueTargetPoints.ForEach((point) => { point.isFull = false; });
        for (var i = 0; i < customerQueue.Count; i++)
        {
            yield return customerQueue[i]?.SetTradePoint(customerQueueTargetPoints[i]);
        }
    }

    [Button]
    public void TestNextClient()
    {
        isInPlayer = true;
        NextCustomerSell();
    }

    [Button]
    public void TestNextClientExit()
    {
        isInPlayer = false;
    }
}