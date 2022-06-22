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
    public List<ClientController> clientQueue = new List<ClientController>();
    public List<TradeWaitingPoint> clientQueueTargetPoints = new List<TradeWaitingPoint>();
    [HideInInspector]
    public GridSlotController gridSlotController;
    /// <summary>
    /// 
    /// </summary>
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
    public void SetClientQueue(ClientController clientController)
    {
        foreach (var point in clientQueueTargetPoints)
        {
            if (point.isFull) continue;
            clientQueue.Add(clientController);
            clientController.SetTradePoint(point);
            break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void NextClientSell()
    {
        if (clientQueue.Count <= 0) return;
        if (!isInPlayer) return;

        var currentClient = clientQueue[0];
        moneyCount.Value += currentClient.SellingProducts(NextClientCallback);
    }

    private void NextClientCallback()
    {
        clientQueue.RemoveAt(0);
        // Burda Bekleyen Müşteriler Tekrar Yerleştirilmeli 
        ReSize();
    }

    private void ReSize()
    {
        clientQueueTargetPoints.ForEach((point) => { point.isFull = false; });
        for (var i = 0; i < clientQueue.Count; i++)
        {
            clientQueue[i].SetTradePoint(clientQueueTargetPoints[i]);
        }

        NextClientSell();
    }
    public void MoneyEffect()
    {
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
            NextClientSell();
        }
    }

    [Button]
    public void TestNextClient()
    {
        isInPlayer = true;
        NextClientSell();
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