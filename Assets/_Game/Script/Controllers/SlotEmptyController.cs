using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlotEmptyController : MonoBehaviour
{
    private Slot _slot;
    private SlotHud _slotHud;
    private bool isInsidePlayer;
    
    public void Init(Slot slot, SlotHud slotHud)
    {
        _slot = slot;
        _slotHud = slotHud;
        SlotOpenEffect();
        slotHud.Open("empty");
        slotHud.SetPriceText(_slot.emptyData.CurrenctPrice.ToString());
        _slot.emptyData.OnChangeVariable.AddListener((data) =>
        {
            slotHud.SetPriceText(data.ToString());
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
        if (other.CompareTag("Player") && !_slot.emptyData.IsOpen)
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
        yield return new WaitForSeconds(_slot.firstTriggerCooldown);

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
                _slot.emptyData.CurrenctPrice--;
                // _slotHud.emptyPrice.SetText(moneyCounter.ToString());
                if (_slot.emptyData.CurrenctPrice == 0)
                {
                    isInsidePlayer = false;
                    _slot.emptyData.IsOpen = true;
                    // Burada Slot Düzeltilcek
                    Debug.Log("Test ! slot Aktif edilcek ");
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

}
