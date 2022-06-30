using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Bot;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

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
    public PlayerSettings customerSettings;
    private CustomerPickerController _customerPickerController;
    private CustomerItemController _customerItemController;
    private NavMeshPath _path;
    private int _pathIndex;



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
        _customerPickerController = GetComponent<CustomerPickerController>();
        _customerItemController = GetComponent<CustomerItemController>();
        _customerPickerController.Init(customerTradeData, this.shoppingData, customerSettings);
        _input = GetComponent<IInput>();
        _input.StartListen();
        StartCoroutine(CustomerStateProgress());
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

    private IEnumerator CustomerStateProgress()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            // Gidilecek olan stand Bulunacak Sıradaki Ürüne
            if (shoppingData.ProductTypes.Count <= 0) continue;
            var queuenItemType = shoppingData.ProductTypes[0];
            //Sırada Gidilecek olan Standı bulmajk var
            var activeSlot = SlotManager.instance.GetActiveStand(queuenItemType);
            if (activeSlot.slot.slotType != SlotType.Stand) continue;
            var standController = activeSlot.GetComponentInChildren<StandController>();
            var grid = standController.GetCustomerSlot();
            grid.isFull = true;
            _path = new NavMeshPath();
            _pathIndex = 1;
            GameManager.instance.navMesh.CalculatePath(transform.position, grid.transform.position, _path);
            yield return MoveToPoint(_path);
            //Toplama Yapılıyor onu çekliyoruz
            //yield return new WaitUntil
            //yield break;
        }

    }

    private IEnumerator MoveToPoint(NavMeshPath path)
    {
        Debug.Log("path : "+ path.corners.Length);
        while (_path.corners.Length != _pathIndex)
        {
            yield return new WaitForSeconds(0.1f);
            if (_path.corners.Length == 0) break;
            Debug.Log("Gidiyoruz Evet !");
            var direction = _path.corners[_pathIndex] - transform.position;
            _input.SetDirection(direction.normalized);
            if (direction.magnitude < 0.5f)
            {
                _pathIndex++;
                Debug.Log("next Point !!!");
            }
        }
        Debug.Log("Vardık be Sonunda !");
        _input.ClearDirection();
    }
}