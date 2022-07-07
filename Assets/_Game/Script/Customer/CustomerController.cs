using System;
using System.Collections;
using _Game.Script.Bot;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using DG.Tweening;
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
    private StandController _activeStandController;
    private GridSlot _activeGrid;
    public Item shoppingBox;
    public GameObject shoppingCar;
    private Vector3 _firstPosition;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerManager"></param>
    /// <param name="maxTradeCount"></param>
    /// <param name="shoppingCard"></param>
    public void Init(CustomerManager customerManager, int maxTradeCount, StackData shoppingData, Vector3 firstPosition)
    {
        _firstPosition = firstPosition;
        transform.position = _firstPosition;
        _customerManager = customerManager;
        this.shoppingData = shoppingData;
        customerTradeData.MaxItemCount = maxTradeCount;
        _customerPickerController = GetComponent<CustomerPickerController>();
        _customerItemController = GetComponent<CustomerItemController>();
        _customerPickerController.Init(customerTradeData, this.shoppingData, customerSettings);
        _customerItemController.Init(customerTradeData);
        _customerItemController.shoppingData = this.shoppingData;
        _input = GetComponent<IInput>();
        _input.StartListen();
        StartCoroutine(CustomerShoppingProgress());
    }

    /// <summary>
    /// Burada Bütün satın almalarımız bitti ve bekliyoruz oluyor 
    /// </summary>
    /// <param name="point"></param>
    public void SetTradePoint(TradeWaitingPoint point)
    {
        point.isFull = true;
        _path = new NavMeshPath();
        _pathIndex = 1;
        _activeGrid.isFull = false;
        GameManager.instance.NavMesh.CalculatePath(transform.position, point.transform.position, _path);
        StartCoroutine(MoveToPoint(_path));
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
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Alışveriş Arabasını yok et !");
        shoppingCar.SetActive(false);
        shoppingBox.gameObject.SetActive(true);
        shoppingBox.PlayScaleEffect(0.5f);
        // Client Çıktığı noktaya doğru gidiyor 
        _path = new NavMeshPath();
        _pathIndex = 1;
        GameManager.instance.NavMesh.CalculatePath(transform.position, _firstPosition, _path);
        callback.Invoke();
        yield return MoveToPoint(_path);
        _customerManager.RemoveCustomer(this);
        yield return new WaitForSeconds(0.5f);
        _customerManager.firstCustomer++;
        _customerManager.SaveFirstCustomerCount();
        Destroy(gameObject);
        Debug.Log("Customer Puf !");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int MoneyCalculator()
    {
        var money = 0;
        foreach (var productType in customerTradeData.ProductTypes)
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

    private IEnumerator CustomerShoppingProgress()
    {
        while (shoppingData.ProductTypes.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            // Gidilecek olan stand Bulunacak Sıradaki Ürüne
            if (shoppingData.ProductTypes.Count <= 0) continue;
            var queuenItemType = shoppingData.ProductTypes[0];
            //Sırada Gidilecek olan Standı bulmak var
            var activeSlot = SlotManager.instance.GetActiveStand(queuenItemType);
            if (activeSlot.slot.slotType != SlotType.Stand) continue;
            _activeStandController = activeSlot.GetComponentInChildren<StandController>();
            _activeGrid = _activeStandController.GetCustomerSlot();
            _activeGrid.isFull = true;
            _path = new NavMeshPath();
            _pathIndex = 1;
            GameManager.instance.NavMesh.CalculatePath(transform.position, _activeGrid.transform.position, _path);
            yield return MoveToPoint(_path); //Toplama yerine vardık 
            transform.DOLookAt(_activeStandController.transform.position, 0.5f);
            yield return new WaitForSeconds(1);
            yield return PickItem();
        }

        // TODO KAsada gidecek yer bulunmayınca Patlıyor bakılacak
        // Burada Döngü bitiyor ve next step olan Kasaya gitmeye başlıyoruz
        _customerManager.cashTradeController.SetCustomerQueue(this);
    }

    private IEnumerator PickItem()
    {
        var standItemController = _activeStandController.GetComponent<IItemController>();
        yield return _customerPickerController.GetItem(standItemController);
    }

    private IEnumerator MoveToPoint(NavMeshPath path)
    {
        while (path.corners.Length != _pathIndex)
        {
            yield return new WaitForSeconds(0.1f);
            if (_pathIndex >= path.corners.Length) continue;

            if (path.corners.Length > 0)
            {
                var direction = path.corners[_pathIndex] - transform.position;
                _input.SetDirection(direction.normalized);
                if (direction.magnitude < 0.5f)
                    _pathIndex++;
            }
        }

        _input.ClearDirection();
    }
}