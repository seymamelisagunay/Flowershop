using _Game.Script.Controllers;
using _Game.Script.Manager;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Slot Yapısı bunun üzerine kurulacak
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SlotController : MonoBehaviour
{
    public Slot slot;
    [HideInInspector] public SlotHud slotHud;
    public Transform hudPoint;

    public void Init()
    {
        GetSaveData();
        CreateHud();
        OpenSlot();
    }

    public void OpenSlot()
    {
        if (slot.emptyData.IsOpen)
        {
            switch (slot.slotType)
            {
                case SlotType.Farm:
                    var farm = Instantiate(slot.farmControllerPrefab, transform);
                    farm.name += slot.Id;
                    farm.Init(this);
                    break;
                case SlotType.Factory:
                    break;
                case SlotType.Stand:
                    GameManager.instance.isClientCreate.Value = true;
                    var stand = Instantiate(slot.standControllerPrefab, transform);
                    stand.name += slot.Id;
                    stand.Init(this);
                    break;
                case SlotType.CashDesk:
                    var cashDesk = Instantiate(slot.cashDeskControllerPrefab, transform);
                    cashDesk.name += slot.Id;
                    cashDesk.Init(this);
                    break;
                default:
                    return;
            }
            slot.stackData.OnChangeVariable.AddListener(SaveSlotStackData);
        }
    }

    public void OpenEmpty()
    {
        var emptySlot =  Instantiate(slot.slotEmptyPrefab, transform);
        emptySlot.GetComponent<ISlotController>().Init(this);
        slot.emptyData.OnChangeVariable.AddListener(SaveSlotEmptyData);
    }

    private void CreateHud()
    {
        slotHud = Instantiate(slot.slotHudPrefab, transform);
        slotHud.transform.position = hudPoint.position;
        slotHud.Init();
    }

    private void GetSaveData()
    {
        if (PlayerPrefs.HasKey(slot.Id + "-StackData"))
        {
            var jsonValueEmptyData = PlayerPrefs.GetString(slot.Id + "-Empty");
            slot.emptyData = JsonConvert.DeserializeObject<SlotEmptyData>(jsonValueEmptyData);
        }
        else
            SaveSlotEmptyData(slot.emptyData);

        if (PlayerPrefs.HasKey(slot.Id + "-StackData"))
        {
            var jsonValueStackData = PlayerPrefs.GetString(slot.Id + "-StackData");
            slot.stackData = JsonConvert.DeserializeObject<StackData>(jsonValueStackData);
        }
        else
            SaveSlotStackData(slot.stackData);
    }

    /// <summary>
    /// Slot Datasını Kayıt ediyoruz !
    /// </summary>
    private void SaveSlotEmptyData(SlotEmptyData slotData)
    {
        if (!PlayerPrefs.HasKey(slot.Id))
            PlayerPrefs.SetInt(slot.Id, 1);

        var jsonValue = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString(slot.Id + "-Empty", jsonValue);
    }

    public void SaveSlotStackData(StackData stackData)
    {
        if (!PlayerPrefs.HasKey(slot.Id))
            PlayerPrefs.SetInt(slot.Id, 1);

        var jsonValue = JsonConvert.SerializeObject(stackData);
        PlayerPrefs.SetString(slot.Id + "-StackData", jsonValue);
    }

    [Button]
    private void SlotNameSet()
    {
        name = slot.slotName;
    }
}
