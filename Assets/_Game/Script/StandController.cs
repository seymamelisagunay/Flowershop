using System;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class StandController : MonoBehaviour, IItemController
{
    public StackData stackData;
    [Tag] public string playerTag;
    private StandPlaceController _standPlaceController;
    private StandPickerController _standPickerController;
    private bool isStayInPlayer;
    public ItemType itemType;

    public void Init(StackData stackData)
    {
        _standPickerController = GetComponent<StandPickerController>();
        this.stackData = stackData;
        var _slotController = transform.GetComponentInParent<SlotController>();
        _standPickerController.Init(_slotController);
    }

    public (ItemType, Item, bool) GetValue()
    {
        return GetValue(itemType);
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        if (stackData.ProductTypes.Count > 0)
        {
            var gridSlot = _standPlaceController.GetSlotObject();
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
        stackData.AddProduct(itemType);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag(playerTag))
        // {
        //     isStayInPlayer = true;
        //     var player = other.GetComponent<PlayerPickerController>();
        //     StartCoroutine(GetValuePlayer(player));
        //     // var playerItems = player.GetItems(itemType, _standStackController.AvailableSlotCount());
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag(playerTag))
        // {
        //     isStayInPlayer = false;
        //     var player = other.GetComponent<PlayerPickerController>();
        //     StopCoroutine(GetValuePlayer(player));
        // }
    }

    // private IEnumerator GetValuePlayer(PlayerPickerController playerPicker)
    // {
    //     while (isStayInPlayer)
    //     {
    //         yield return new WaitForSeconds(playerPicker.playerStackData.ProductionRate);
    //         // playerPicker.GetItems()
    //         if (!_slotController.slot.stackData.CheckMaxCount())
    //         {
    //             yield break;
    //         }
    //         else
    //         {
    //             var gridSlot = playerPicker.GetItem(itemType);
    //             if (gridSlot != null)
    //             {
    //                 gridSlot.isFull = false;
    //                 var isCheck = _standStackController.SetValue(gridSlot.slotInObject);
    //                 gridSlot.slotInObject = null;
    //             }
    //             else
    //                 yield break;
    //         }
    //     }
    // }
}