using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerPickerController : MonoBehaviour, IPickerController
{
    public PlayerSettings playerSettings;
    [Tag] public List<string> slotsTag;
    public ItemList itemList;
    public StackData playerStackData = new StackData();
    private GridSlotController _gridSlotController;
    private bool _isStayFarm;
    private PlayerItemController _playerItemController;

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
        var slotController = other.GetComponent<IItemController>();
        _isStayFarm = false;
        StopCoroutine(GetItem(slotController));
    }


    private void SelectSlot(IItemController slotController)
    {
        StartCoroutine(GetItem(slotController));
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var slotItemController = itemController;
        yield return new WaitForSeconds(playerSettings.firstTriggerCooldown);
        while (_isStayFarm)
        {
            if (playerStackData.CheckMaxCount())
            {
                yield return new WaitForSeconds(playerSettings.pickingSpeed);
                //Toplama yapılacak 
                var (productType, item, isItemFinish) = slotItemController.GetValue();
                if (!isItemFinish) continue;
                Debug.Log("Product Count : " + _playerItemController.stackData.ProductTypes.Count);
                if (item == null)
                {
                    Debug.Log("item + null");
                    continue;
                }
                var gridSlot = _gridSlotController.GetPosition();
                if (gridSlot == null)
                {
                    Debug.Log("null");
                    continue;
                }

                _playerItemController.SetValue(productType);
                item.transform.parent = gridSlot.transform;
                gridSlot.isFull = true;
                gridSlot.slotInObject = item;
                item.Play(Vector3.zero);
            }
            else
                yield break;
        }
    }
}

public interface IPickerController
{
    IEnumerator GetItem(IItemController itemController);
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}