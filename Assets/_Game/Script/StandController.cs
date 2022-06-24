using System;
using System.Collections.Generic;
using System.Collections;
using _Game.Script.Character;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class StandController : MonoBehaviour
{
    public StackData stackData;
    [Tag] public string playerTag;
    private SlotController _slotController;
    private StandStackController _standStackController;
    private bool isStayInPlayer;

    public ItemType itemType;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        _standStackController = GetComponent<StandStackController>();
        stackData = _slotController.slot.stackData;
        _standStackController.Init(_slotController);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isStayInPlayer = true;
            var player = other.GetComponent<PlayerPickerController>();
            StartCoroutine(GetValuePlayer(player));
            // var playerItems = player.GetItems(itemType, _standStackController.AvailableSlotCount());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isStayInPlayer = false;
            var player = other.GetComponent<PlayerPickerController>();
            StopCoroutine(GetValuePlayer(player));
        }
    }

    private IEnumerator GetValuePlayer(PlayerPickerController playerPicker)
    {
        while (isStayInPlayer)
        {
            yield return new WaitForSeconds(playerPicker.playerStackData.ProductionRate);
            // playerPicker.GetItems()
            if (!_slotController.slot.stackData.CheckMaxCount())
            {
                yield break;
            }
            else
            {
                var gridSlot = playerPicker.GetItem(itemType);
                if (gridSlot != null)
                {
                    gridSlot.isFull = false;
                    var isCheck = _standStackController.SetValue(gridSlot.slotInObject);
                    gridSlot.slotInObject = null;
                }else
                    yield break;
            }
        }
    }
}