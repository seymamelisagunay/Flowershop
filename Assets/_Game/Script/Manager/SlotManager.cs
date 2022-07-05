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
        currentOrderCount.Value++;
    }

    public void SlotOpen()
    {
        var nextSlot = slotStates.FindAll(x => x.orderCount == currentOrderCount.Value);
        if (nextSlot.Count > 0)
            nextSlot.ForEach(x =>
            {
                if (!x.slotController.slot.emptyData.IsOpen)
                {
                    x.slotController.OpenEmpty();
                }
            });
    }

    public SlotController GetActiveSlot(ItemType itemType)
    {
        var result = slots.Find(x => x.slot.emptyData.IsOpen && x.slot.itemType == itemType);
        return result;
    }

    public SlotController GetSlotController(SlotType slotType, ItemType itemType)
    {
        var result = slots.Find(x => x.slot.slotType == slotType && x
            .slot.itemType == itemType);
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
    public int orderCount;
    public SlotController slotController;
}