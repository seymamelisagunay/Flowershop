using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

public class PickerController : MonoBehaviour
{
    public PlayerSettings playerSettings;
    [Tag] public string farmTag;
    public StackData playerStackData = new StackData();
    public List<IStackController> stackControllerList = new List<IStackController>();
    private bool isStayFarm;

    private Alarm _alarm;

    private void Start()
    {
        GetSaveData();
        playerStackData.OnChangeVariable.AddListener(SaveData);
        _alarm = new Alarm();
        _alarm.Start(playerSettings.pickingSpeed);
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
        if (other.CompareTag(farmTag))
        {
            Debug.Log(other.name);
            var farmController = other.GetComponent<IStackController>();
            stackControllerList.Add(farmController);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(farmTag)) return; //Multi Tag Test Edilecek
        Debug.Log(other.name);
        var farmController = other.GetComponent<IStackController>();
        stackControllerList.Remove(farmController);
    }

    private void Update()
    {
        if (!(stackControllerList.Count > 0)) return;
        if (!playerStackData.CheckMaxCount()) return;
        
        if (!_alarm.Check()) return;
        _alarm.Start(playerSettings.pickingSpeed);
        Debug.Log("Next");
        foreach (var stackController in stackControllerList)
        {
            // yield return new WaitForSeconds(playerSettings.pickingSpeed);
            if (!playerStackData.CheckMaxCount()) break;
            var (productType, stackObject,isValueFull) = stackController.GetValue();
            if (isValueFull)
            {
                playerStackData.AddProduct(productType);
                Destroy(stackObject);
            }
        }
    }
}