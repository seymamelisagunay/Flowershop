using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class PlayerItemController : MonoBehaviour, IItemController
{
    [HideInInspector]
    public PlayerSettings playerSettings;
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
            var item = stackData.ProductTypes.Find(x => x == itemType);
            if (item == ItemType.none)
                return (ItemType.Rose, null, false);
            var gridSlot = _gridSlotController.GetSlotObject(itemType);
            Item resultData = null;
            if (gridSlot != null)
            {
                gridSlot.isFull = false;
                resultData = gridSlot.slotInObject;
                gridSlot.slotInObject = null;
            }
            stackData.RemoveProduct(itemType);
            if (!playerSettings.isBot)
            {
                SoundManager.instance.Play("place_item");
                MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            }

            return (item, resultData, true);
        }

        return (ItemType.Rose, null, false);
    }

    public void SetValue(ItemType itemType)
    {
        if (stackData.CheckMaxCount())
            stackData.AddProduct(itemType);
    }
}