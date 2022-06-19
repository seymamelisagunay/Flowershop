using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public StackData clientTradeData;
    public ClientStackController stackController;
    public StackData shoppingCard;
    [ReadOnly]
    public TradeWaitingPoint waitingPoint;

    private ClientManager _clientManager;
    // Toplanacak olan data Yapısı kurulacak 

    public void Init(ClientManager clientManager,int maxTradeCount,StackData shoppingCard)
    {
        _clientManager = clientManager;
        this.shoppingCard = shoppingCard;
        clientTradeData.MaxProductCount = maxTradeCount;
        stackController = GetComponent<ClientStackController>();
    }

    public void SetTradePoint(TradeWaitingPoint point)
    {
        transform.position = point.transform.position;
        waitingPoint = point;
    }

    public void SellingProducts(Action callback)
    {
        waitingPoint = null;
        // var priceCount =
    }

    private IEnumerator SellEffect(Action callback)
    {
        yield return new WaitForSeconds(0.1f);
        callback.Invoke();
    }
    [Button]
    public void SetCheck()
    {
        _clientManager.cashTradeController.SetClientQueue(this);
    }
}
/// <summary>
/// Müşterinin topladığı ürünleri yönettiği yer burası oluyor
/// 
/// </summary>
public class ClientStackController : MonoBehaviour, IStackController
{
    public (ProductType, GameObject, bool) GetValue()
    {
        throw new System.NotImplementedException();
    }

    public void SetValue(ProductType productType)
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// Müşterinin toplayacağı ürünler burada barınacak 
/// </summary>
public class ClientHarvestPicker : MonoBehaviour
{


    public void Init(StackData shoppingCard)
    {
        
    }
}
