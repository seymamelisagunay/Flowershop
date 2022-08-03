using System;
using System.Collections;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using NaughtyAttributes;
using UnityEngine;

public class CustomerPickerController : MonoBehaviour, IPickerController
{
    [Tag] public string standTag;
    public PlayerSettings customerSettings;
    [ReadOnly] public StackData customerTradeData;
    public StackData shoppingData;
    private GridSlotController _gridSlotController;
    private CustomerItemController _customerItemController;
    [HideInInspector] public UIEmojiController emojiController;

    public void Init(StackData customerTradeData, StackData shoppingData, PlayerSettings customerSettings)
    {
        this.customerSettings = customerSettings;
        this.customerTradeData = customerTradeData;
        _customerItemController = GetComponent<CustomerItemController>();
        this.shoppingData = shoppingData;
        _gridSlotController = GetComponentInChildren<GridSlotController>();
        _gridSlotController.ReSize();
        emojiController = GetComponentInChildren<UIEmojiController>();
        EmojiInit();
    }

    private void EmojiInit()
    {
        var firstItemList = shoppingData.ProductTypes.FindAll(x => x == shoppingData.ProductTypes[0]);
        var itemListText = "0/" + firstItemList.Count;
        emojiController.SetItemText(itemListText);
        var itemSprite = GameManager.instance.itemIcons.Find(x => x.id == (int) shoppingData.ProductTypes[0]);
        emojiController.SetItemIcon(itemSprite.Value);
    }

    private void SetItemUI(int maxItemCount, int itemCount, ItemType itemType)
    {
        var itemListText = itemCount + "/" + maxItemCount;
        emojiController.SetItemText(itemListText);
        var itemSprite = GameManager.instance.itemIcons.Find(x => x.id == (int) itemType);
        emojiController.SetItemIcon(itemSprite.Value);
    }


    public void OnTriggerEnter(Collider other)
    {
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        // Buraya kuracağız Item UI sistemi
        var slotItemController = itemController;
        var itemType = slotItemController.GetItemType();
        var pickerItemList = shoppingData.ProductTypes.FindAll(x => x == itemType);
        var pickerItemCount = 0;
        SetItemUI(pickerItemList.Count,pickerItemCount ,itemType);
        yield return new WaitForSeconds(customerSettings.firstTriggerCooldown);
        while (pickerItemCount <= pickerItemList.Count - 1)
        {
            for (int i = 0; i < pickerItemList.Count; i++)
            {
                var isItemFinish = false;
                var productType = ItemType.none;
                Item item = null;
                while (!isItemFinish)
                {
                    yield return new WaitForSeconds(customerSettings.pickingSpeed);
                    (productType, item, isItemFinish) = slotItemController.GetValue();
                }
                //Toplama yapılacak 
                _customerItemController.SetValue(productType);
                var gridSlot = _gridSlotController.GetPosition();
                item.transform.parent = gridSlot.transform;
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                pickerItemCount++;
                item.Play(Vector3.zero);
                SetItemUI(pickerItemList.Count,pickerItemCount ,itemType);
            }
        }
    }
}