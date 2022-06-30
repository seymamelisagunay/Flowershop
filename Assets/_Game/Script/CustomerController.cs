using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Bot;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public ItemList itemList;
    /// <summary>
    /// 
    /// </summary>
    public StackData customerTradeData;
    /// <summary>
    /// Satın Alınacaklar
    /// </summary>
    public StackData shoppingData;
    [ReadOnly] public TradeWaitingPoint waitingPoint;
    private CustomerManager _customerManager;
    private IInput _input;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerManager"></param>
    /// <param name="maxTradeCount"></param>
    /// <param name="shoppingCard"></param>
    public void Init(CustomerManager customerManager, int maxTradeCount, StackData shoppingData)
    {
        _customerManager = customerManager;
        this.shoppingData = shoppingData;
        customerTradeData.MaxItemCount = maxTradeCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    public void SetTradePoint(TradeWaitingPoint point)
    {
        point.isFull = true;
        var direction = transform.position - point.transform.position;
        _input.SetDirection(direction.normalized);
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
        foreach (var productType in shoppingData.ProductTypes)
        {
            var type = itemList.GetItemPrefab(productType);
            money += type.price;
        }
        return money;
    }

    [Button]
    public void SetCheck()
    {
        _customerManager.cashTradeController.SetCustomerQueue(this);
    }
}