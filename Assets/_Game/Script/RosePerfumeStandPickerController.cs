using System.Collections;
using _Game.Script.Character;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Düzeltilecek
/// </summary>
public class RosePerfumeStandPickerController : MonoBehaviour, IPickerController
{
    public ItemList itemList;
    private IItemPlaceController _standPlaceController;
    private SlotController _slotController;
    private IItemController _itemController;
    private StackData _stackData;
    [Tag] public string playerTag;

    private bool _isStayPlayer;


    public void Start()
    {
        _standPlaceController = GetComponent<IItemPlaceController>();
        _slotController = GetComponentInParent<SlotController>();
        _stackData = _slotController.slot.stackData;
        _itemController = GetComponent<IItemController>();
        // _itemController.Init(_stackData);
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
            var itemController = other.GetComponent<IItemController>();
            StopCoroutine(GetItem(itemController));
            _isStayPlayer = false;
        }
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var playerItemController = (PlayerItemController) itemController;
        var standStackData = _slotController.slot.stackData;
        yield return new WaitForSeconds(_slotController.slot.firstTriggerCooldown);

        while (_isStayPlayer)
        {
            if (standStackData.IsAvailable())
            {
                yield return new WaitForSeconds(_slotController.slot.triggerCooldown);
                if (playerItemController.stackData.ProductTypes.Count <= 0) break;
                var (productType, item, isItemFinish) = playerItemController.GetValue(_slotController.slot.itemType);
                if (!isItemFinish) continue;

                var gridSlot = _standPlaceController.GetPosition();
                if (gridSlot == null) continue;

                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                item.transform.parent = gridSlot.transform;
                _itemController.SetValue(productType);
                item.moveDurationValue = 1f;
                item.Play(Vector3.zero);
            }
            else
            {
                var playerController = playerItemController.GetComponent<PlayerController>();
                if (!playerController.playerSettings.isBot)
                    yield break;
                if (playerItemController.stackData.ProductTypes.Count <= 0) break;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}