using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CustomerItemController : MonoBehaviour, IItemController
{
    [ReadOnly] public StackData tradeData;
    public StackData shoppingData;
    private GridSlotController gridSlotController;

    public void Init(StackData stackData)
    {
        tradeData = stackData;
        gridSlotController = GetComponentInChildren<GridSlotController>();
    }

    public ItemType GetItemType()
    {
        return ItemType.none;
    }

    public (ItemType, Item, bool) GetValue()
    {
        throw new System.NotImplementedException();
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        throw new System.NotImplementedException();
    }

    public void SetValue(ItemType itemType)
    {
        tradeData.ProductTypes.Add(itemType);
        shoppingData.ProductTypes.Remove(itemType);
    }
}