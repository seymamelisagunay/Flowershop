using System.Collections;
using _Game.Script.Controllers;
using _Game.Script.Core.Character;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerPickerController : MonoBehaviour, IPickerController
{
    public PlayerSettings playerSettings;
    [Tag] public string slotTag;
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
        if (!other.CompareTag(slotTag)) return;
        var slotController = other.GetComponent<IItemController>();
        SelectSlot(slotController);
        _isStayFarm = true;

        // Debug.Log(other.name);
        // var farmController = other.GetComponent<IStackController>();
        // farmControllerList.Add(farmController);
        // StartCoroutine(StayInSlotCounter());
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(slotTag)) return; //Multi Tag Test Edilecek
        var slotController = other.GetComponent<IItemController>();
        _isStayFarm = false;
        StopCoroutine(GetItem(slotController));

        // Debug.Log(other.name);
        // var farmController = other.GetComponent<IStackController>();
        // farmControllerList.Remove(farmController);
        // if (farmControllerList.Count != 0) return;
        //
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
                
                _playerItemController.SetValue(productType);
                var gridSlot = _gridSlotController.GetPosition();
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