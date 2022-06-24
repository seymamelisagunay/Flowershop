using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerPickerController : MonoBehaviour
{
    public PlayerSettings playerSettings;
    [Tag] public string farmTag;
    public ItemList itemList;
    public StackData playerStackData = new StackData();
    public List<IStackController> farmControllerList = new List<IStackController>();
    private GridSlotController _gridSlotController;
    private bool _isStayFarm;
    private Alarm _alarm;

    private IEnumerator Start()
    {
        GetSaveData();
        playerStackData.OnChangeVariable.AddListener(SaveData);
        _gridSlotController = GetComponentInChildren<GridSlotController>();
        _gridSlotController.ReSize();

        _alarm = new Alarm();
        _alarm.Start(playerSettings.pickingSpeed);
        playerStackData.MaxItemCount = playerSettings.maxPickerCount;
        playerStackData.ProductionRate = playerSettings.pickingSpeed;
        foreach (var type in playerStackData.ProductTypes)
        {
            yield return new WaitForSeconds(playerSettings.pickingSpeed);
            var itemPrefab = itemList.GetItemPrefab(type);
            _gridSlotController.sampleObject = itemPrefab;
            _gridSlotController.CreateObject();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(farmTag)) return;
        Debug.Log(other.name);
        var farmController = other.GetComponent<IStackController>();
        farmControllerList.Add(farmController);
        StartCoroutine(StayInSlotCounter());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(farmTag)) return; //Multi Tag Test Edilecek

        Debug.Log(other.name);
        var farmController = other.GetComponent<IStackController>();
        farmControllerList.Remove(farmController);
        if (farmControllerList.Count != 0) return;

        _isStayFarm = false;
        StopCoroutine(StayInSlotCounter());
    }

    private void Update()
    {
        if (!_isStayFarm) return;
        if (!(farmControllerList.Count > 0)) return;
        if (!playerStackData.CheckMaxCount()) return;
        if (!_alarm.Check()) return;
        _alarm.Start(playerSettings.pickingSpeed);
        Debug.Log("Next");
        foreach (var stackController in farmControllerList)
        {
            // yield return new WaitForSeconds(playerSettings.pickingSpeed);
            if (!playerStackData.CheckMaxCount()) break;
            var (productType, stackObject, isValueFull) = stackController.GetValue();
            if (!isValueFull) continue;

            playerStackData.AddProduct(productType);
            var itemPrefab = itemList.GetItemPrefab(productType);
            _gridSlotController.sampleObject = itemPrefab;
            /// Yol alma burdan eklene bilri !
            _gridSlotController.CreateObject();
            Destroy(stackObject);
        }
    }

    // public List<GridSlot> GetItems(ItemType itemType, int count)
    // {
    //     var itemList = _gridSlotController.GetSlotObjects(itemType);
    //     var result = new List<GridSlot>();
    //     for (int i = 0; i < itemList.Count; i++)
    //     {
    //         if ((i + 1) > count)
    //         {
    //             break;
    //         }
    //         result.Add(itemList[i]);
    //     }
    //     return result;
    // }
    public GridSlot GetItem(ItemType itemType)
    {
        if (playerStackData.ProductTypes.Count == 0)
        {
            return null;
        }
        var slot = _gridSlotController.GetSlotObject(itemType);
        if (slot != null)
        {
            var index = playerStackData.ProductTypes.FindIndex(x => x == itemType);
            playerStackData.RemoveProduct(index);
        }

        return slot;
    }


    private IEnumerator StayInSlotCounter()
    {
        yield return new WaitForSeconds(playerSettings.firstTriggerCooldown);
        _isStayFarm = true;
    }
}