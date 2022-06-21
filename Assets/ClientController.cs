using System;
using System.Collections;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public ItemList itemList;
    /// <summary>
    /// 
    /// </summary>
    public StackData clientTradeData;
    /// <summary>
    /// Satın Alınacaklar
    /// </summary>
    public StackData shoppingCard;
    [ReadOnly] public TradeWaitingPoint waitingPoint;
    private ClientManager _clientManager;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientManager"></param>
    /// <param name="maxTradeCount"></param>
    /// <param name="shoppingCard"></param>
    public void Init(ClientManager clientManager, int maxTradeCount, StackData shoppingCard)
    {
        _clientManager = clientManager;
        this.shoppingCard = shoppingCard;
        clientTradeData.MaxProductCount = maxTradeCount;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    public void SetTradePoint(TradeWaitingPoint point)
    {
        point.isFull = true;
        transform.position = point.transform.position;
        waitingPoint = point;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public int SellingProducts(Action callback)
    {
        waitingPoint = null;
        var priceCount = MoneyCalculator();
        StartCoroutine(SellEffect(callback));
        return priceCount;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator SellEffect(Action callback)
    {
        yield return new WaitForSeconds(3f);
        callback.Invoke();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int MoneyCalculator()
    {
        var money = 0;
        foreach (var productType in shoppingCard.ProductTypes)
        {
            var type = itemList.GetStackObject(productType);
            money += type.price;
        }
        return money;
    }
    [Button]
    public void SetCheck()
    {
        _clientManager.cashTradeController.SetClientQueue(this);
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