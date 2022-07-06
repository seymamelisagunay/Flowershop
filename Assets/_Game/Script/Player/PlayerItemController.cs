using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using UnityEngine;

public class PlayerItemController : MonoBehaviour, IItemController
{
    public StackData stackData;
    private GridSlotController _gridSlotController;


    public void Init(StackData stackData)
    {
        this.stackData = stackData;
        _gridSlotController = GetComponentInChildren<GridSlotController>();
    }

    public ItemType GetItemType()
    {
        return ItemType.none;
    }

    public (ItemType, Item, bool) GetValue()
    {
        if (stackData.ProductTypes.Count > 0)
        {
            return GetValue(stackData.ProductTypes[0]);
        }

        return (ItemType.Rose, null, false);
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        if (stackData.ProductTypes.Count > 0)
        {
            if (itemType != stackData.ProductTypes[0])
            {
                return (ItemType.Rose, null, false);
            }

            var gridSlot = _gridSlotController.GetSlotObject(stackData.ProductTypes[0]);
            gridSlot.isFull = false;
            var resultData = gridSlot.slotInObject;
            gridSlot.slotInObject = null;
            stackData.RemoveProduct(0);
            return (resultData.itemType, resultData, true);
        }

        return (ItemType.Rose, null, false);
    }

    public void SetValue(ItemType itemType)
    {
        if (stackData.CheckMaxCount())
            stackData.AddProduct(itemType);
    }
}