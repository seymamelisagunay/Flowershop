using System.Collections;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Düzeltilecek
/// </summary>
public class StandPickerController : MonoBehaviour, IPickerController
{
    public ItemList itemList;
    private StandPlaceController _standPlaceController;
    private SlotController _slotController;
    private IItemController _itemController;
    private StackData _stackData;
    [Tag] public string playerTag;

    private bool _isStayPlayer;

    public void Init(SlotController slotController)
    {
        _standPlaceController = GetComponent<StandPlaceController>();
        _slotController = slotController;
        _stackData = _slotController.slot.stackData;
        _standPlaceController.Init(itemList, _stackData);
        _itemController = GetComponent<IItemController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log(other.name);
            _isStayPlayer = true;
            var itemController = other.GetComponent<IItemController>();
            StartCoroutine(GetItem(itemController));
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isStayPlayer = false;
            var itemController = other.GetComponent<IItemController>();
            StopCoroutine(GetItem(itemController));
        }
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var playerItemController = itemController;
        var standStackData = _slotController.slot.stackData;
        yield return new WaitForSeconds(_slotController.slot.firstTriggerCooldown);

        while (_isStayPlayer)
        {
            if (standStackData.CheckMaxCount())
            {
                yield return new WaitForSeconds(_slotController.slot.triggerCooldown);
                var (productType, item, isItemFinish) = playerItemController.GetValue(_slotController.slot.itemType);
                if (!isItemFinish) continue;

                var gridSlot = _standPlaceController.GetPosition();
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                item.transform.parent = gridSlot.transform;
                _itemController.SetValue(productType);
                item.Play(Vector3.zero);
            }
            else
                yield break;
        }
    }
}