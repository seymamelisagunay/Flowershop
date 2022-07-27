using System;
using System.Collections.Generic;
using System.Collections;
using _Game.Script.Character;
using NaughtyAttributes;
using UnityEngine;

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

    // Para kazanma
    public IntVariable tradeMoneyCount;

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

    private void Update()
    {
        if (!isInPlayer || !isContinueTrade) return;
        if (customerQueue.Count <= 0) return;
        isContinueTrade = false;
        NextCustomerSell();
       
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
            customerQueue.Add(customerController);
            customerController.SetTradePoint(point);
            break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void NextCustomerSell()
    {
        var currentClient = customerQueue[0];
        currentCurrency = currentClient.MoneyCalculator();
        StartCoroutine(currentClient.SellingProducts(NextClientCallback));
    }

    /// <summary>
    /// 
    /// </summary>
    private void NextClientCallback()
    {
        if (customerQueue.Count > 0)
            customerQueue.RemoveAt(0);
        var moneyObjectCount = currentCurrency / 10;
        tradeMoneyCount.Value += currentCurrency;
        for (int i = 0; i < moneyObjectCount; i++)
        {
            CreateMoney();
        }

        // Burda Bekleyen Müşteriler Tekrar Yerleştirilmeli 
        ReSize();
        if (customerQueue.Count > 0)
            NextCustomerSell();
      
    }

    /// <summary>
    /// 
    /// </summary>
    private void ReSize()
    {
        customerQueueTargetPoints.ForEach((point) => { point.isFull = false; });
        for (var i = 0; i < customerQueue.Count; i++)
            customerQueue[i].SetTradePoint(customerQueueTargetPoints[i]);
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
        if (!other.CompareTag(playerTag)) return;
        var player = other.GetComponent<PlayerController>();
        isInPlayer = true;
        if (customerQueue.Count > 0)
        {
            player?.hudDotIdle?.gameObject.SetActive(true);
            player?.hudDotIdle?.Play();
            StartCoroutine(PlayThreeDotEffect(player));
        }
        else
            isContinueTrade = true;

        StartCoroutine(StartCustomerSell(player.playerSettings.firstTriggerCooldown));
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
        yield return new WaitForSeconds(firstTriggerDuration);

        if (customerQueue.Count <= 0) yield break;
        if (!isInPlayer)
        {
            isContinueTrade = true;
            yield break;
        }

        NextCustomerSell();
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

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        
        isInPlayer = false;
        isContinueTrade = false;
        var player = other.GetComponent<PlayerController>();
        player?.hudDotIdle?.Stop();
        player?.hudDotIdle?.gameObject.SetActive(false);
    }
}