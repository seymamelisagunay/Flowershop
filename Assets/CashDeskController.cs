using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class CashDeskController : MonoBehaviour
{
    [Tag] public string playerTag;
    public StackData stackData;
    private SlotController _slotController;
    private CashTradeController _cashTradeController;
    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        stackData = _slotController.slot.stackData;
        // Burada Client 
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
