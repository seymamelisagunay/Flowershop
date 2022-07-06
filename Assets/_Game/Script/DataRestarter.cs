using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Data Restarter", menuName = "Gnarly Team/Restarter", order = 0)]
public class DataRestarter : ScriptableObject
{
    [Foldout("Slot Settigns")] public SlotEmptyData cashDeskEmptyDefault;
    [Foldout("Slot Settigns")] public Slot cashDeskData;
    [Foldout("Slot Settigns")] public SlotEmptyData moneyFirstDefault;
    [Foldout("Slot Settigns")] public Slot moneyFirst;
    [Foldout("Slot Settigns")] public SlotEmptyData roseFarmOneDefault;
    [Foldout("Slot Settigns")] public Slot roseFarmOne;
    [Foldout("Slot Settigns")] public SlotEmptyData roseFarmSecondDefault;
    [Foldout("Slot Settigns")] public Slot roseFarmSecond;
    [Foldout("Slot Settigns")] public SlotEmptyData roseFarmThirdDefault;
    [Foldout("Slot Settigns")] public Slot roseFarmThird;
    [Foldout("Slot Settigns")] public SlotEmptyData rosePerfumeFactoryDefault;
    [Foldout("Slot Settigns")] public Slot rosePerfumeFactory;
    [Foldout("Slot Settigns")] public SlotEmptyData rosePerfumeStandDefault;
    [Foldout("Slot Settigns")] public Slot rosePerfumeStand;
    [Foldout("Slot Settigns")] public SlotEmptyData roseStandDefault;
    [Foldout("Slot Settigns")] public Slot roseStand;

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
        cashDeskData.emptyData = cashDeskEmptyDefault;
        moneyFirst.emptyData = moneyFirstDefault;
        roseFarmOne.emptyData = roseFarmOneDefault;
        roseFarmSecond.emptyData = roseFarmSecondDefault;
        roseFarmThird.emptyData = roseFarmThirdDefault;
        rosePerfumeFactory.emptyData = rosePerfumeFactoryDefault;
        rosePerfumeStand.emptyData = rosePerfumeStandDefault;
        roseStand.emptyData = roseStandDefault;

        cashDeskMoneyCount.Value = cashDeskMoneyCountDefault;
        currentSlotOrderCount.Value = currentSlotOrderCountDefault;
        isCreateClient.Value = isCreateClientDefault;
        moneyVariable.Value = moneyVariableDefault;
        PlayerPrefs.DeleteAll();
    }
}