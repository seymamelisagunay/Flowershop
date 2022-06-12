using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlotEmpty : MonoBehaviour
{
    private Slot _slot;
    private SlotHud _slotHud;
    private bool isInsidePlayer;
    public void Init(Slot slot, SlotHud slotHud)
    {
        _slot = slot;
        _slotHud = slotHud;
        SlotOpenEffect();

        _slotHud.emptyPrice.SetText(slot.emptyData.currenctPrice.ToString());
        _slot.emptyData.OnChangeVariable.AddListener((data) =>
        {
            _slotHud.emptyPrice.SetText(data.currenctPrice.ToString());
        });
    }

    private void SlotOpenEffect(){
        var parent = transform.parent;
        parent.localScale = Vector3.zero;
        parent.DOScale(Vector3.one,0.5f).SetEase(Ease.InOutBounce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_slot.emptyData.isOpen)
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

    IEnumerator StayInPlayer()
    {
        yield return new WaitForSeconds(_slot.firstTriggerTime);

        while (isInsidePlayer)
        {
            var result = UserManager.Instance.DecreasingMoney(1);
            if (!result)
            {
                isInsidePlayer = false;
                yield return null;
            }
            _slot.emptyData.currenctPrice--;
            // _slotHud.emptyPrice.SetText(moneyCounter.ToString());
            if (_slot.emptyData.currenctPrice == 0)
            {
                isInsidePlayer = false;
                _slot.emptyData.isOpen = true;
                // Burada Slot Düzeltilcek
                Debug.Log("Test ! slot Aktif edilcek ");
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

}
