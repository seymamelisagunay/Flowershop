using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Slot Manager Amacı slotların hangi sıra ile açılacağını 
/// kontrol ettiğimiz yer oluyor
/// </summary>
public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;
    public IntVariable currentOrderCount;
    public List<SlotState> slotStates = new List<SlotState>();
    public BoolVariable isClientCreate;
    [HideInInspector] public List<SlotController> slots = new List<SlotController>();

    private void Awake()
    {
        instance = this;
        currentOrderCount.Value = PlayerPrefs.GetInt("orderCount", 1);
        currentOrderCount.OnChangeVariable.AddListener(SaveOrderCount);
        currentOrderCount.OnChangeVariable.AddListener(SlotOpen);
        slots = Transform.FindObjectsOfType<SlotController>().ToList();
    }

    public void NextSlot()
    {
        var slots = slotStates.FindAll(x => x.orderCount == currentOrderCount.Value);
        foreach (var state in slots)
        {
            if (state.slotController.slot.emptyData.IsOpen && state.isNewSlotOpen)
            {
                currentOrderCount.Value++;
                return;
            }
        }
    }

    public void SlotOpen()
    {
        var nextSlot = slotStates.FindAll(x => x.orderCount == currentOrderCount.Value);
        if (nextSlot.Count > 0)
            nextSlot.ForEach(x =>
            {
                x.slotController.CustomerLimitIncreaseValue = x.customerLimitIncreaseValue;
                
                if (!x.slotController.slot.emptyData.IsOpen)
                {
                    x.slotController.OpenEmpty();
                }
            });
    }

    public List<SlotController> GetActiveFarmAndFactory()
    {
        var result = slots.FindAll(x => (x.slot.slotType == SlotType.Farm || x.slot.slotType == SlotType.Factory) &&
                                        x.slot.emptyData.IsOpen);
        return result;
    }
    
    public SlotController GetActiveStand(ItemType itemType)
    {
        var result = slots.Find(x => x.slot.emptyData.IsOpen && x.slot.itemType == itemType&&x
            .slot.slotType == SlotType.Stand);
        return result;
    }

    public List<SlotController> GetSlotController(SlotType slotType)
    {
        var result = slots.FindAll(x => x.slot.slotType == slotType && x.slot.emptyData.IsOpen);
        return result;
    }

    private void SaveOrderCount()
    {
        PlayerPrefs.SetInt("orderCount", currentOrderCount.Value);
    }
}

[Serializable]
public class SlotState
{
    public int customerLimitIncreaseValue;
    
    public bool isNewSlotOpen;
    public int orderCount;
    public SlotController slotController;
}