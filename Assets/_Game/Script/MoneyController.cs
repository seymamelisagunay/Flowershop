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
    private GridSlotController _money;
    private bool isInPlayer;
    [HideInInspector] public PlayerController playerController;
    private SlotController _slotController;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        _money = GetComponent<GridSlotController>();
        _money.ReSize();
        for (int i = 0; i < 10; i++)
        {
            _money.CreateObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        var clonePlayer = other.GetComponent<PlayerController>();
        if (clonePlayer.playerSettings.isBot) return;
        playerController = clonePlayer;
        isInPlayer = true;
        StartCoroutine(GetMoney());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        var clonePlayer = other.GetComponent<PlayerController>();
        if (clonePlayer.playerSettings.isBot) return;
        isInPlayer = false;
        StopCoroutine(GetMoney());
    }

    private IEnumerator GetMoney()
    {
        yield return new WaitForSeconds(playerController.playerSettings.firstTriggerCooldown);
        while (isInPlayer)
        {
            yield return new WaitForSeconds(playerController.playerSettings.pickingSpeed);
            var gridSlot = _money.GetSlotObject();
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
                var item = gridSlot.slotInObject;
                item.transform.parent = playerController.transform;
                item.Play(Vector3.zero, true);
                SoundManager.instance.Play("money_transfer");
            }
        }
    }


    public void SlotClose()
    {
        transform.GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
    }
}