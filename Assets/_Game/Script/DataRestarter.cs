using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Data Restarter", menuName = "Gnarly Team/Restarter", order = 0)]
public class DataRestarter : ScriptableObject
{
    [BoxGroup("Mini Settings")] public List<Slot> Slots;


    [Space(5)] [BoxGroup("Mini Settings")] public int cashDeskMoneyCountDefault;
    [BoxGroup("Mini Settings")] public IntVariable cashDeskMoneyCount;
    [BoxGroup("Mini Settings")] public int currentSlotOrderCountDefault;
    [BoxGroup("Mini Settings")] public IntVariable currentSlotOrderCount;
    [BoxGroup("Mini Settings")] public bool isCreateClientDefault;
    [BoxGroup("Mini Settings")] public BoolVariable isCreateClient;
    [BoxGroup("Mini Settings")] public int moneyVariableDefault;
    [BoxGroup("Mini Settings")] public IntVariable moneyVariable;

    [Button]
    public void RestartData()
    {
        foreach (var slot in Slots)
        {
            slot.RestartData();
        }
        cashDeskMoneyCount.Value = cashDeskMoneyCountDefault;
        currentSlotOrderCount.Value = currentSlotOrderCountDefault;
        isCreateClient.Value = isCreateClientDefault;
        moneyVariable.Value = moneyVariableDefault;
        PlayerPrefs.DeleteAll();
    }
}