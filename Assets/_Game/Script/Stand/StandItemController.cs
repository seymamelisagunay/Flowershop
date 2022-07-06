using System;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class StandItemController : MonoBehaviour, IItemController
{
    public StackData stackData;
    [Tag] public string playerTag;
    private IItemPlaceController _standPlaceController;
    public ItemType itemType;

    public void Init(StackData stackData)
    {
        this.stackData = stackData;
        _standPlaceController = GetComponent<IItemPlaceController>();
        _standPlaceController.ReSize();
        LoadData();
    }

    private void LoadData()
    {
        foreach (var itemType in stackData.ProductTypes)
        {
            _standPlaceController.CreateObject();
        }
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public (ItemType, Item, bool) GetValue()
    {
        return GetValue(itemType);
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        if (stackData.ProductTypes.Count <= 0) return (ItemType.none, null, false);
        var gridSlot = _standPlaceController.GetSlotObject();
        gridSlot.isFull = false;
        var resultData = gridSlot.slotInObject;
        gridSlot.slotInObject = null;
        stackData.RemoveProduct(0);
        return (resultData.itemType, resultData, true);
    }

    public void SetValue(ItemType itemType)
    {
        stackData.AddProduct(itemType);
    }
}