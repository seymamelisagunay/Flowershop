using _Game.Script.Controllers;
using UnityEngine;

public class FactoryItemController : MonoBehaviour, IItemController
{
    public StackData itemData;
    public ItemType itemType;

    public GridSlotController gridSlotController;

    public void Init(StackData stackData)
    {
        gridSlotController.ReSize();
        itemData = stackData;
        Load();
    }

    private void Load()
    {
        foreach (var itemType in itemData.ProductTypes)
        {
            gridSlotController.CreateObject();
        }
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public (ItemType, Item, bool) GetValue()
    {
        return GetValue(itemType);
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        if (itemData.ProductTypes.Count <= 0) return (ItemType.none, null, false);

        var grid = gridSlotController.GetSlotObject();
        grid.isFull = false;
        var resultData = grid.slotInObject;
        grid.slotInObject = null;
        itemData.RemoveProduct(0);
        return (resultData.itemType, resultData, true);
    }

    public void SetValue(ItemType itemType)
    {
        itemData.AddProduct(itemType);
    }
}