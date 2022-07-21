using _Game.Script;
using _Game.Script.Controllers;
using _Game.Script.Manager;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Slot Yapısı bunun üzerine kurulacak
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SlotController : MonoBehaviour
{
    public Slot slot;
    [HideInInspector] public SlotHud slotHud;
    public Transform hudPoint;
    public IItemController activeItemController;
    [ReadOnly] public CameraFocus cameraFocus;

    public void Init()
    {
        cameraFocus = GetComponent<CameraFocus>();
        GetSaveData();
        CreateHud();
        OpenSlot();
    }

    public void OpenSlot()
    {
        if (!slot.emptyData.IsOpen) return;

        switch (slot.slotType)
        {
            case SlotType.Farm:
                var cloneFarm = slot.itemControllerPrefab.GetComponent<FarmController>();
                activeItemController = Instantiate(cloneFarm, transform);
                var cloneFarmItemController = (FarmController) activeItemController;
                cloneFarmItemController.name += slot.Id;
                break;
            case SlotType.Factory:
                var factory = slot.itemControllerPrefab.GetComponent<FactoryController>();
                var cloneFactory = Instantiate(factory, transform);
                cloneFactory.Init(this);
                activeItemController = cloneFactory.itemController;
                cloneFactory.name += slot.Id;
                break;
            case SlotType.Stand:
                GameManager.instance.isClientCreate.Value = true;
                var stand = slot.itemControllerPrefab.GetComponent<StandItemController>();
                activeItemController = Instantiate(stand, transform);
                var standController = (StandItemController) activeItemController;
                standController.name += slot.Id;
                GameManager.instance.ItemTypeList.Add(slot.itemType);
                break;
            case SlotType.CashDesk:
                var cashDeskPrefab = slot.itemControllerPrefab.GetComponent<CashDeskController>();
                var cloneCashDesk = Instantiate(cashDeskPrefab, transform);
                cloneCashDesk.name += slot.Id;
                activeItemController = cloneCashDesk;
                break;
            case SlotType.Shelver: //Shelver Bölümü oluyor Burası Shelver Üretiliyor ve devam ediyor
                var shelverController = slot.itemControllerPrefab.GetComponent<ShelverHrController>();
                shelverController.Init();
                break;
            default:
                return;
        }

        activeItemController?.Init(slot.stackData);
        slot.stackData.OnChangeVariable.AddListener(SaveSlotStackData);
    }

    public void OpenEmpty()
    {
        var emptySlot = Instantiate(slot.slotEmptyPrefab, transform);
        emptySlot.GetComponent<ISlotController>().Init(this);
        slot.emptyData.OnChangeVariable.AddListener(SaveSlotEmptyData);
        cameraFocus?.Focus(0.2f);
    }

    private void CreateHud()
    {
        slotHud = Instantiate(slot.slotHudPrefab, transform);
        slotHud.transform.position = hudPoint.position;
        slotHud.Init();
    }

    private void GetSaveData()
    {
        if (PlayerPrefs.HasKey(slot.Id + "-Empty"))
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