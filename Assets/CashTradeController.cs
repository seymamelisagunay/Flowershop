using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Bunun İçersinde Müşteriler Listelenecek ve bu Müşterilerden para alınacacak
/// 
/// </summary>
public class CashTradeController : MonoBehaviour
{
    [Tag] public string playerTag;
    public List<ClientController> clientQueue = new List<ClientController>();
    public List<TradeWaitingPoint> clientQueueTargetPoints = new List<TradeWaitingPoint>();

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

    public void NextClientSell()
    {
        if (clientQueue.Count <= 0) return;

        var currentClient = clientQueue[0];
        currentClient.SellingProducts(NextClientCallback);
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
        for (int i = 0; i < clientQueue.Count; i++)
        {
            clientQueue[i].SetTradePoint(clientQueueTargetPoints[i]);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            NextClientSell();
        }
    }
}