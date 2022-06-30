using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class CashDeskController : MonoBehaviour, IItemController
{
    [Tag] public string playerTag;
    public StackData stackData;
    private SlotController _slotController;
    private CashTradeController _cashTradeController;

    public void Init(StackData stackData)
    {
        this.stackData = stackData;
        // Burada Client 
    }

    public (ItemType, Item, bool) GetValue()
    {
        throw new System.NotImplementedException();
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        throw new System.NotImplementedException();
    }

    public void SetValue(ItemType itemType)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Player trigger içinde ise işlem yapılır
        if (other.CompareTag(playerTag))
        {
        }
    }
}