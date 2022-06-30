using System;
using System.Collections;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using UnityEngine;

public class CustomerPickerController : MonoBehaviour, IPickerController
{
    [Tag] public string standTag;
    private bool _isStayStand;
    public PlayerSettings customerSettings;
    [ReadOnly] public StackData customerTradeData;
    public StackData shoppingData;
    private GridSlotController _gridSlotController;
    private CustomerItemController _customerItemController;

    public void Init(StackData customerTradeData, StackData shoppingData, PlayerSettings customerSettings)
    {
        this.customerSettings = customerSettings;
        this.customerTradeData = customerTradeData;
        _customerItemController = GetComponent<CustomerItemController>();
        this.shoppingData = shoppingData;
        _gridSlotController = GetComponentInChildren<GridSlotController>();
        _gridSlotController.ReSize();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(standTag)) return;
        var itemController = other.GetComponent<IItemController>();
        StartCoroutine(GetItem(itemController));
        _isStayStand = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(standTag)) return;
        var itemController = other.GetComponent<IItemController>();
        StopCoroutine(GetItem(itemController));
        _isStayStand = false;
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var slotItemController = itemController;
        var pickerItemList = shoppingData.ProductTypes.FindAll(x => x == slotItemController.GetItemType());
        yield return new WaitForSeconds(customerSettings.firstTriggerCooldown);
        while (_isStayStand)
        {
            for (int i = 0; i < pickerItemList.Count; i++)
            {
                yield return new WaitForSeconds(customerSettings.pickingSpeed);
                //Toplama yapılacak 
                var (productType, item, isItemFinish) = slotItemController.GetValue();
                if (!isItemFinish) continue;

                _customerItemController.SetValue(productType);
                var gridSlot = _gridSlotController.GetPosition();
                item.transform.parent = gridSlot.transform;
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                item.Play(Vector3.zero);
            }
        }
    }
}