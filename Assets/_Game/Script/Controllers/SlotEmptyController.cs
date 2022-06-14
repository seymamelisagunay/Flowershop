using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SlotEmptyController : MonoBehaviour
{
    private SlotController _slotController;
    private bool isInsidePlayer;
    [ReadOnly]
    public SlotEmptyData emptyData;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        emptyData = _slotController.slot.emptyData;
        SlotOpenEffect();
        _slotController.slotHud.Open("empty");
        var remaing = _slotController.slot.emptyData.Price - _slotController.slot.emptyData.CurrenctPrice;
        _slotController.slotHud.SetPriceText(remaing.ToString());
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
            isInsidePlayer = true;
            StartCoroutine(StayInPlayer());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsidePlayer = false;
            StopCoroutine(StayInPlayer());
        }
    }

    private IEnumerator StayInPlayer()
    {
        yield return new WaitForSeconds(_slotController.slot.firstTriggerCooldown);

        while (isInsidePlayer)
        {
            var result = UserManager.Instance.DecreasingMoney(1);
            if (!result)
            {
                isInsidePlayer = false;
                yield return null;
            }
            else
            {
                emptyData.CurrenctPrice++;
                // _slotHud.emptyPrice.SetText(moneyCounter.ToString());
                if (emptyData.CurrenctPrice == emptyData.Price)
                {
                    isInsidePlayer = false;
                    emptyData.IsOpen = true;
                    SlotManager.instance.NextSlot();
                    SlotClose();
                    _slotController.OpenSlot();
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void SlotClose()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
    }

}
