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
using Random = UnityEngine.Random;

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
    public CustomerHUD customerHUD;
    public bool isCashDeskReady;


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
    public IEnumerator SetTradePoint(TradeWaitingPoint point)
    {
        customerHUD.uiEmojiController.ShowCashDeskIcon();
        point.isFull = true;
        _path = new NavMeshPath();
        _pathIndex = 1;
        _activeGrid.isFull = false;
        GameManager.instance.NavMesh.CalculatePath(transform.position, point.transform.position, _path);
        yield return MoveToPoint(_path);
        var customer = _customerManager.cashTradeController.customerQueue.Find(x => x == this);
        if (customer == null)
            _customerManager.cashTradeController.customerQueue.Add(this);

        if (point.isFirst)
        {
            isCashDeskReady = true;
        }

        waitingPoint = point;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator SellingProducts()
    {
        if (waitingPoint != null)
        {
            waitingPoint.isReady = false;
            waitingPoint.isFull = false;
        }

        waitingPoint = null;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(SellEffect());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator SellEffect()
    {
        Debug.Log("Alışveriş Arabasını yok et !");
        shoppingCar.SetActive(false);
        shoppingBox.gameObject.SetActive(true);
        shoppingBox.PlayScaleEffect(0.5f);
        customerHUD.uiEmojiController.ShowSmile();
        // Client Çıktığı noktaya doğru gidiyor 
        _path = new NavMeshPath();
        _pathIndex = 1;
        GameManager.instance.NavMesh.CalculatePath(transform.position, _firstPosition, _path);
        _customerManager.RemoveCustomer(this);
        yield return MoveToPoint(_path);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Debug.Log("Customer Puf !");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int MoneyCalculator()
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
            var randomWaitingTime = Random.Range(0.35f, 0.55f);
            yield return new WaitForSeconds(randomWaitingTime);
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
            yield return new WaitForSeconds(0.5f);
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