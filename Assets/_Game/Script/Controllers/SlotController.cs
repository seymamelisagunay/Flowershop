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
    [HideInInspector]
    public StackController stackController;

    public void Init()
    {
        GetSaveData();
        stackController = GetComponent<StackController>();

        slotHud = Instantiate(slot.slotHudPrefab, transform);
        slotHud.transform.position = hudPoint.position;
        slotHud.Init();

        if (slot.emptyData.IsOpen)
        {
            switch (slot.slotType)
            {
                case SlotType.farm:
                    var farm = Instantiate(slot.farmControllerPrefab);
                    farm.Init(this);
                    break;
                case SlotType.factory:
                    break;
                case SlotType.stand:
                    break;
                case SlotType.checkout:
                    break;
            }
        }
        else
        {
            var emptySlot = Instantiate(slot.slotEmptyPrefab, transform);
            emptySlot.Init(slot, slotHud);
            slot.emptyData.OnChangeVariable.AddListener(SaveSlotEmptyData);
        }
      
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
    private void SaveSlotEmptyData(SlotEmptyData slotData)
    {
        var jsonValue = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString(slot.Id+"-Empty", jsonValue);
    }

    [Button]
    private void SlotNameSet()
    {
        name = slot.slotName;
    }
}


