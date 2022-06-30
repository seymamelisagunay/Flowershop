using System.Collections.Generic;
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
    public List<CustomerController> customerQueue = new List<CustomerController>();
    public List<TradeWaitingPoint> customerQueueTargetPoints = new List<TradeWaitingPoint>();
    [HideInInspector]
    public GridSlotController gridSlotController;
    public bool isInPlayer;
    // Para kazanma
    public IntVariable moneyCount;
    private void Start()
    {
        gridSlotController = GetComponentInChildren<GridSlotController>();
        gridSlotController.ReSize();
        moneyCount.Value = PlayerPrefs.GetInt(playerPrefsKey, 0);
        moneyCount.OnChangeVariable.AddListener(ChangeMoneyValue);
    }

    private void LoadMoney()
    {
        for (int i = 0; i < moneyCount.Value; i++)
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
        moneyCount.Value += currentClient.SellingProducts(NextClientCallback);
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
        {
            customerQueue[i].SetTradePoint(customerQueueTargetPoints[i]);
        }
        NextCustomerSell();
    }
    private void ChangeMoneyValue()
    {
        PlayerPrefs.SetInt(playerPrefsKey, moneyCount.Value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInPlayer = true;
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