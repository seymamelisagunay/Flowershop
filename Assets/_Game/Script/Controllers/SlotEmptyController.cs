using System.Collections;
using System.Collections.Generic;
using _Game.Script.Character;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SlotEmptyController : MonoBehaviour, ISlotController
{
    private SlotController _slotController;
    private bool _isInsidePlayer;
    [ReadOnly] public SlotEmptyData emptyData;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        emptyData = _slotController.slot.emptyData;
        SlotOpenEffect();
        _slotController.slotHud.Open("empty");
        var remaining = _slotController.slot.emptyData.Price - _slotController.slot.emptyData.CurrenctPrice;
        _slotController.slotHud.SetPriceText(remaining.ToString());
        _slotController.slot.emptyData.OnChangeVariable.AddListener((data) =>
        {
            var remaingCount = data.Price - data.CurrenctPrice;
            _slotController.slotHud.SetPriceText(remaingCount.ToString());
        });
    }

    private void SlotOpenEffect()
    {
        var parent = transform.parent;
        parent.localScale = Vector3.zero;
        parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_slotController.slot.emptyData.IsOpen)
        {
            _isInsidePlayer = true;
            var player = other.GetComponent<PlayerController>();
            StartCoroutine(StayInPlayer(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInsidePlayer = false;
            var player = other.GetComponent<PlayerController>();
            StopCoroutine(StayInPlayer(player));
        }
    }

    private IEnumerator StayInPlayer(PlayerController player)
    {
        yield return new WaitUntil(() => !player.characterController.inMotion && _isInsidePlayer);
        Debug.Log("Test Geçtik !");
        yield return new WaitForSeconds(_slotController.slot.firstTriggerCooldown);
        while (_isInsidePlayer)
        {
            if (emptyData.CurrenctPrice < 40)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (emptyData.CurrenctPrice >= 40 & emptyData.CurrenctPrice < 100)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else if (emptyData.CurrenctPrice >= 100 & emptyData.CurrenctPrice < 300)
            {
                yield return new WaitForSeconds(0.002f);
            }
            else
            {
                yield return new WaitForSeconds(0.0001f);
            }

            var result = UserManager.Instance.DecreasingMoney(1);
            if (!result)
            {
                _isInsidePlayer = false;
                yield return null;
            }
            else
            {
                emptyData.CurrenctPrice++;
                // _slotHud.emptyPrice.SetText(moneyCounter.ToString());
                if (emptyData.CurrenctPrice == emptyData.Price)
                {
                    _isInsidePlayer = false;
                    emptyData.IsOpen = true;
                    SlotManager.instance.NextSlot();
                    SlotClose();
                    _slotController.OpenSlot();
                    _slotController.slotHud.active.Close();
                }
            }
        }
    }

    public void SlotClose()
    {
        transform.GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
    }
}

public interface ISlotController
{
    public void Init(SlotController slotController);
}