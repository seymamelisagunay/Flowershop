using System;
using System.Collections;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using UnityEngine;

public class CustomerPickerController : MonoBehaviour, IPickerController
{
    [Tag] public string standTag;
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
    }

    public void OnTriggerExit(Collider other)
    {

    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var slotItemController = itemController;
        var pickerItemList = shoppingData.ProductTypes.FindAll(x => x == slotItemController.GetItemType());
        var pickerItemCount = 0;
        yield return new WaitForSeconds(customerSettings.firstTriggerCooldown);
        while (pickerItemCount <= pickerItemList.Count - 1)
        {
            for (int i = 0; i < pickerItemList.Count; i++)
            {
                var isItemFinish = false;
                var productType = ItemType.none;
                var item = new Item();

                while (!isItemFinish)
                {
                    yield return new WaitForSeconds(customerSettings.pickingSpeed);
                    (productType, item, isItemFinish) = slotItemController.GetValue();
                }
                //Toplama yapılacak 
                Debug.Log("Ver item Ver ");
                _customerItemController.SetValue(productType);
                var gridSlot = _gridSlotController.GetPosition();
                item.transform.parent = gridSlot.transform;
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                pickerItemCount++;
                item.Play(Vector3.zero);
            }
        }
    }
}