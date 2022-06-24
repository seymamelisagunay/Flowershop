using _Game.Script.Controllers;
using UnityEngine;

public class StandStackController : MonoBehaviour, IStackController
{
    public ItemList itemList;
    private StandPlaceController _standPlaceController;
    private SlotController _slotController;
    private StackData _stackData;

    public void Init(SlotController slotController)
    {
        _standPlaceController = GetComponent<StandPlaceController>();
        _slotController = slotController;
        _stackData = _slotController.slot.stackData;
        _standPlaceController.Init(itemList, _stackData);
    }
    public (ItemType, Item, bool) GetValue()
    {
        //Remove Edilmeli
        // _stackData.RemoveProduct(ItemType.Rose);
        throw new System.NotImplementedException();
    }
    public void SetValue(ItemType itemType)
    {
    }
    /// <summary>
    /// Verilen Item Slotumuzda uygun yer var ise alınacak ve true döndürülecek 
    /// </summary>
    /// <param name="item"></param>
    public bool SetValue(Item item)
    {
        var gridSlot = _standPlaceController.GetPosition();
        if (gridSlot != null)
        {
            item.transform.parent = gridSlot.transform;
            item.Play(gridSlot.transform).AddOnComplete(() =>
            {
                Debug.Log("Şekil Değiştireceğiz");
            });
            gridSlot.isFull = true;
            gridSlot.slotInObject = item;
            _stackData.AddProduct(item.itemType);
            return true;
        }
        return false;
    }
}