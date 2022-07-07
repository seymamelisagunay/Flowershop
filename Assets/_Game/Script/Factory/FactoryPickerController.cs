using System.Collections;
using _Game.Script.Controllers;
using _Game.Script.Manager;
using NaughtyAttributes;
using UnityEngine;

public class FactoryPickerController : MonoBehaviour, IPickerController
{
    [Tag] public string playerTag;

    private bool _isStayPlayer;
    public GridSlotController gridSlotController;
    public StackData pickerData;
    private SlotController _slotController;
    public ItemType pickerItemType;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;
    }

    private void Start()
    {
        gridSlotController.ReSize();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isStayPlayer = true;
        var playerItemController = other.GetComponent<IItemController>();
        StartCoroutine(GetItem(playerItemController));
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isStayPlayer = false;
        var playerItemController = other.GetComponent<IItemController>();
        StopCoroutine(GetItem(playerItemController));
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var playerItemController = itemController;
        yield return new WaitForSeconds(_slotController.slot.firstTriggerCooldown);

        while (_isStayPlayer)
        {
            if (pickerData.CheckMaxCount())
            {
                yield return new WaitForSeconds(_slotController.slot.triggerCooldown);
                var (productType, item, isItemFinish) = playerItemController.GetValue(pickerItemType);
                if (!isItemFinish) continue;
                var gridSlot = gridSlotController.GetPosition();
                if (item == null)
                {
                    var prefab = GameManager.instance.itemList.GetItemPrefab(productType);
                    item = Instantiate(prefab, gridSlot.transform);
                }
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                item.transform.parent = gridSlot.transform;
                pickerData.ProductTypes.Add(productType);
                item.Play(Vector3.zero);
            }
            else
                yield break;
        }
    }
}