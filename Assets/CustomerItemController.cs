using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CustomerItemController : MonoBehaviour,IItemController
{
    [ReadOnly] public StackData tradeData;
    private GridSlotController gridSlotController;
    public void Init(StackData stackData)
    {
        tradeData = stackData;
        gridSlotController = GetComponentInChildren<GridSlotController>();
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
        throw new System.NotImplementedException();
    }
}
