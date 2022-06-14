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
    [HideInInspector]
    public SlotHud slotHud;
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
                case SlotType.farm:
                    var farm = Instantiate(slot.farmControllerPrefab, transform);
                    farm.Init(this);
                    slot.stackData.OnChangeVariable.AddListener(SaveSlotStackData);
                    break;
                case SlotType.factory:
                    break;
                case SlotType.stand:
                    break;
                case SlotType.checkout:
                    break;
            }
        }
    }
    public void OpenEmpty()
    {
        var emptySlot = Instantiate(slot.slotEmptyPrefab, transform);
        emptySlot.Init(this);
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
        if (PlayerPrefs.HasKey(slot.Id))
        {
            var jsonValueEmptyData = PlayerPrefs.GetString(slot.Id + "-Empty");
            var jsonValueStackData = PlayerPrefs.GetString(slot.Id + "-StackData");
            slot.emptyData = JsonConvert.DeserializeObject<SlotEmptyData>(jsonValueEmptyData);
            slot.stackData = JsonConvert.DeserializeObject<StackData>(jsonValueStackData);
        }
    }
    /// <summary>
    /// Slot Datasını Kayıt ediyoruz !
    /// </summary>
    private void SaveSlotEmptyData(SlotEmptyData slotData)
    {
        var jsonValue = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString(slot.Id + "-Empty", jsonValue);
    }
    private void SaveSlotStackData(StackData stackData)
    {
        var jsonValue = JsonConvert.SerializeObject(stackData);
        PlayerPrefs.SetString(slot.Id + "-StackData", jsonValue);

    }
    [Button]
    private void SlotNameSet()
    {
        name = slot.slotName;
    }
}


