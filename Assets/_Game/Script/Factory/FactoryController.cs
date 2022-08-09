using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    public IPickerController pickerController;
    public IItemController itemController;
    [HideInInspector] public SlotController slotController;
    public List<GridSlot> botSlot = new List<GridSlot>();

    public Machine machine;
    public float machineStrength;
    public int machineVibration;

    public float itemCreateDuration;


    public void Init(SlotController slotController)
    {
        itemController = GetComponentInChildren<IItemController>();
        pickerController = GetComponentInChildren<IPickerController>();
        this.slotController = slotController;
        var factoryPicker = (FactoryPickerController) pickerController;
        factoryPicker.Init(this.slotController);
        if (this.slotController.slot.stackData.ProductTypes.Count <= 0)
        {
            this.slotController.slot.stackData.ProductTypes = new List<ItemType>()
                {ItemType.Water, ItemType.Water, ItemType.Water};
        }

        StartCoroutine(ItemCreator(factoryPicker));
    }

    public IEnumerator ItemCreator(FactoryPickerController factoryPicker)
    {
        var picker = factoryPicker;
        var factoryItemController = (FactoryItemController) itemController;
        while (true)
        {
            yield return new WaitUntil(() => picker.pickerData.ProductTypes.Count > 0);
            yield return new WaitUntil(() => slotController.slot.stackData.IsAvailable());
            yield return new WaitUntil(() => factoryItemController.itemData.IsAvailable());
            machine?.Play(itemCreateDuration, machineStrength, machineVibration);
            yield return new WaitForSeconds(itemCreateDuration);
            // Item Üretilecek
            // Sıralama Söyle 
            // İlk Pickerdan itemi al Daha Sonra 
            // itemi yok et ve yeni bir item üret 
            var item = picker.gridSlotController.GetSlotObject();
            if (item != null)
            {
                item.isFull = false;
                var itemObject = item.slotInObject;
                Destroy(itemObject.gameObject);
                //TODO Makinaya gitme effecti burada yapıla bilir 
            }

            factoryItemController.gridSlotController.CreateObject();

            itemController.SetValue(itemController.GetItemType());
            picker.pickerData.RemoveProduct(0);
        }
    }

    public GridSlot GetCustomerSlot()
    {
        var resultObject = botSlot.RandomSelectObject();
        return resultObject;
    }
}