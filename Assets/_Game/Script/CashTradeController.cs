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
    [ReadOnly] public List<CustomerController> customerQueue = new List<CustomerController>();
    public List<TradeWaitingPoint> customerQueueTargetPoints = new List<TradeWaitingPoint>();
    [HideInInspector] public GridSlotController gridSlotController;

    public bool isInPlayer;

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

    private void LoadMoney()
    {
        var loadMoneyCount = tradeMoneyCount.Value / 10;
        for (int i = 0; i < loadMoneyCount; i++)
        {
            gridSlotController.CreateObject();
        }
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
        tradeMoneyCount.Value += currentClient.SellingProducts(NextClientCallback);
    }

    private void NextClientCallback()
    {
        customerQueue.RemoveAt(0);
        // Burda Bekleyen Müşteriler Tekrar Yerleştirilmeli 
        ReSize();
    }

    private void ReSize()
    {
        customerQueueTargetPoints.ForEach((point) => { point.isFull = false; });
        for (var i = 0; i < customerQueue.Count; i++)
            customerQueue[i].SetTradePoint(customerQueueTargetPoints[i]);
        NextCustomerSell();
    }

    private void ChangeMoneyValue()
    {
        PlayerPrefs.SetInt(playerPrefsKey, tradeMoneyCount.Value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        var player = other.GetComponent<PlayerController>();
        isInPlayer = true;
        StartCoroutine(StartCustomerSell(player.playerSettings.firstTriggerCooldown));
    }

    private IEnumerator StartCustomerSell(float firstTriggerDuration)
    {
        yield return new WaitForSeconds(firstTriggerDuration);
        if (isInPlayer)
        {
            NextCustomerSell();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInPlayer = false;
        }
    }
}