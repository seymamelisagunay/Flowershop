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
        if (customerQueue.Count <= 0) return;
        if (!isInPlayer) return;
        var currentClient = customerQueue[0];
        currentCurrency = currentClient.SellingProducts(NextClientCallback);
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
    }

    /// <summary>
    /// 
    /// </summary>
    private void ReSize()
    {
        customerQueueTargetPoints.ForEach((point) => { point.isFull = false; });
        for (var i = 0; i < customerQueue.Count; i++)
            customerQueue[i].SetTradePoint(customerQueueTargetPoints[i]);
        NextCustomerSell();
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
        player?.hudDotIdle.gameObject.SetActive(true);
        player?.hudDotIdle.Play();
        isInPlayer = true;
        StartCoroutine(StartCustomerSell(player.playerSettings.firstTriggerCooldown));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstTriggerDuration"></param>
    /// <returns></returns>
    private IEnumerator StartCustomerSell(float firstTriggerDuration)
    {
        // while (isInPlayer)
        // {
        yield return new WaitForSeconds(firstTriggerDuration);
        if (isInPlayer)
        {
            NextCustomerSell();
        }
        // }
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
        if (other.CompareTag(playerTag))
        {
            isInPlayer = false;
            var player = other.GetComponent<PlayerController>();
            player?.hudDotIdle.Stop();
            player?.hudDotIdle.gameObject.SetActive(false);
        }
    }
}