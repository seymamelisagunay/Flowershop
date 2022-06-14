using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slot Manager Amac� slotlar�n hangi s�ra ile a��laca��n� 
/// kontrol etti�imiz yer oluyor
/// </summary>
public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;
    public IntVariable currentOrderCount;
    public List<SlotState> slotStates = new List<SlotState>();

    private void Awake()
    {
        instance = this;
        currentOrderCount.Value = PlayerPrefs.GetInt("orderCount", 1);
        currentOrderCount.OnChangeVariable.AddListener(SaveOrderCount);
        currentOrderCount.OnChangeVariable.AddListener(SlotOpen);
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




