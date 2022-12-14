using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class PlayerPickerController : MonoBehaviour, IPickerController
{
    public PlayerSettings playerSettings;
    [Tag] public List<string> slotsTag;
    public ItemList itemList;
    public StackData playerStackData = new StackData();
    private GridSlotController _gridSlotController;
    private bool _isStayFarm;
    private PlayerItemController _playerItemController;
    private IItemController _activeItemController;
    public float exitDistance;

    private IEnumerator Start()
    {
        GetSaveData();
        playerStackData.OnChangeVariable.AddListener(SaveData);
        _gridSlotController = GetComponentInChildren<GridSlotController>();
        _gridSlotController.h = playerSettings.maxPickerCount;
        _gridSlotController.ReSize();

        playerStackData.MaxItemCount = playerSettings.maxPickerCount;
        playerStackData.ProductionRate = playerSettings.pickingSpeed;
        // playerStackData.OnChangeVariable.AddListener();
        foreach (var type in playerStackData.ProductTypes)
        {
            yield return new WaitForSeconds(playerSettings.pickingSpeed);
            var itemPrefab = itemList.GetItemPrefab(type);
            _gridSlotController.sampleObject = itemPrefab;
            _gridSlotController.CreateObject();
        }

        _playerItemController = GetComponent<PlayerItemController>();
        _playerItemController.playerSettings = playerSettings;
        _playerItemController.Init(playerStackData);
    }

    public void GetSaveData()
    {
        if (!PlayerPrefs.HasKey("Player-StackData")) return;
        var jsonValue = PlayerPrefs.GetString("Player-StackData");
        playerStackData = JsonConvert.DeserializeObject<StackData>(jsonValue);
    }

    public void SaveData(StackData stackData)
    {
        var jsonValue = JsonConvert.SerializeObject(stackData);
        PlayerPrefs.SetString("Player-StackData", jsonValue);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!slotsTag.Contains(other.tag)) return;
        var slotController = other.GetComponent<IItemController>();
        SelectSlot(slotController);
        _isStayFarm = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!slotsTag.Contains(other.tag)) return; //Multi Tag Test Edilecek
        // var slotController = other.GetComponent<IItemController>();
    }

    private void SelectSlot(IItemController slotController)
    {
        StartCoroutine(GetItem(slotController));
    }

    public void SetStay(bool isStay)
    {
        _isStayFarm = isStay;
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var slotItemController = itemController;
        yield return new WaitForSeconds(playerSettings.firstTriggerCooldown);
        var slot = (MonoBehaviour)slotItemController;

        while (true)
        {
            var distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance >= exitDistance)
                yield break;

            yield return new WaitForSeconds(0.01f);

            yield return new WaitForSeconds(playerSettings.pickingSpeed);
            if (!playerStackData.IsAvailable()) yield break;
            //Toplama yap??lacak 
            var (productType, item, isItemFinish) = slotItemController.GetValue();
            if (!isItemFinish) continue;
            if (item == null)
            {
                Debug.LogError("item + null");
                continue;
            }

            var gridSlot = _gridSlotController.GetPosition();
            if (gridSlot == null)
            {
                Debug.LogError(" : gridSlot null");
                continue;
            }

            _playerItemController.SetValue(productType);
            item.transform.parent = gridSlot.transform;
            gridSlot.isFull = true;
            gridSlot.slotInObject = item;

            item.Play(Vector3.zero);
            item.transform.localEulerAngles = Vector3.zero;
            if (!playerSettings.isBot)
            {
                SoundManager.instance.Play(item.soundKey);
                MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            }

        }
    }
}

public interface IPickerController
{
    IEnumerator GetItem(IItemController itemController);
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}