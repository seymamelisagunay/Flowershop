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
        slotHud = Instantiate(slot.slotHudPrefab,transform);
        slotHud.transform.position = hudPoint.position;
        if (slot.emptyData.isOpen)
        {
            Debug.Log("Slot is Open");
        }
        else
        {
            var emptySlot = Instantiate(slot.slotEmptyPrefab, transform);
            emptySlot.Init(slot, slotHud);
        }
        slot.emptyData.OnChangeVariable.AddListener(SaveSlotData);
    }

    private void GetSaveData()
    {
        if (PlayerPrefs.HasKey(slot.Id))
        {
            var jsonValue = PlayerPrefs.GetString(slot.Id);
            slot.emptyData = JsonConvert.DeserializeObject<SlotEmptyData>(jsonValue);
        }
    }
    /// <summary>
    /// Slot Datasını Kayıt ediyoruz !
    /// </summary>
    private void SaveSlotData(SlotEmptyData slotData)
    {
        var jsonValue = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString(slot.Id, jsonValue);
    }

    [Button]
    private void SlotNameSet()
    {
        name = slot.slotName;
    }
}


