using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Character;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Slot İçerisinde çalışacak ve 
/// </summary>
public class MoneyController : MonoBehaviour, ISlotController
{
    [Tag] public string playerTag;
    private GridSlotController moneyGrid;
    private bool isInPlayer;
    [HideInInspector] public PlayerController playerController;
    private SlotController _slotController;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        moneyGrid = GetComponent<GridSlotController>();
        for (int i = 0; i < 10; i++)
        {
            moneyGrid.CreateObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        isInPlayer = true;
        playerController = other.GetComponent<PlayerController>();
        StartCoroutine(GetMoney());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        isInPlayer = false;
        StopCoroutine(GetMoney());
    }

    private IEnumerator GetMoney()
    {
        while (isInPlayer)
        {
            yield return new WaitForSeconds(0.05f);
            var gridSlot = moneyGrid.GetSlotObject();
            if (gridSlot == null)
            {
                isInPlayer = false;
                _slotController.slot.emptyData.IsOpen = true;
                SlotManager.instance.NextSlot();
                _slotController.OpenSlot();
                SlotClose();
                yield return null;
            }
            else
            {
                UserManager.Instance.IncrementMoney(10);
                gridSlot.isFull = false;
                var item = gridSlot.slotInObject.GetComponent<Item>();
                item.Play(playerController.transform, true);
            }
        }
    }


    public void SlotClose()
    {
        transform.GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
    }
}