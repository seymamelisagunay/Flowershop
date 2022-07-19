using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using DG.Tweening;
using UnityEngine;


public class FarmController : MonoBehaviour, IItemController
{
    public ItemType itemType;
    public ItemList itemList;
    public List<GridSlot> gridSlots = new List<GridSlot>();
    public List<GridSlot> botSlot = new List<GridSlot>();

    [ReadOnly] public StackData stackData;
    private int _currentGridSlotCount;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotController"></param>
    public void Init(StackData stackData)
    {
        this.stackData = stackData;
        OpenFarmEffect();
        StartCoroutine(StackDataLoad());
        StartCoroutine(Creator());
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public (ItemType, Item, bool) GetValue()
    {
        return GetValue(itemType);
    }

    private IEnumerator StackDataLoad()
    {
        for (int i = 0; i < stackData.ProductTypes.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (i <= _currentGridSlotCount)
            {
                ItemMoveEffect(stackData.ProductTypes[i]);
                _currentGridSlotCount++;
            }
        }

        _currentGridSlotCount =
            _currentGridSlotCount > gridSlots.Count - 1 ? gridSlots.Count - 1 : _currentGridSlotCount;
    }

    private void ItemMoveEffect(ItemType itemType)
    {
        var farm = itemList.GetItemPrefab(itemType);
        var grid = gridSlots[_currentGridSlotCount];
        var cloneObject = Instantiate(farm, grid.transform);
        grid.isFull = true;
        grid.slotInObject = cloneObject;
        cloneObject.Play(Vector3.zero);
    }

    private void OpenFarmEffect()
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Creator()
    {
        while (true)
        {
            yield return new WaitForSeconds(stackData.ProductionRate);
            if (!stackData.CheckMaxCount()) continue;
            SetValue(ItemType.Rose);
        }
    }

    public (ItemType, Item, bool) GetValue(ItemType itemType)
    {
        if (stackData.ProductTypes.Count > 0)
        {
            var resultObject = gridSlots[_currentGridSlotCount].slotInObject;
            gridSlots[_currentGridSlotCount].isFull = false;
            gridSlots[_currentGridSlotCount].slotInObject = null;
            stackData.RemoveProduct(0);
            _currentGridSlotCount--;
            return (ItemType.Rose, resultObject, true);
        }

        return (ItemType.Rose, null, false);
    }

    public void SetValue(ItemType itemType)
    {
        _currentGridSlotCount++;
        _currentGridSlotCount =
            _currentGridSlotCount > gridSlots.Count - 1 ? gridSlots.Count - 1 : _currentGridSlotCount;
        stackData.AddProduct(itemType);
        ItemMoveEffect(itemType);
    }
    public GridSlot GetCustomerSlot()
    {
        var resultObject = botSlot.RandomSelectObject();
        return resultObject;
    }

    [Button]
    public void TestRemove()
    {
        stackData.ProductTypes.Clear();
    }
}

/// <summary>
/// Item Controller
/// Picker ulaşıp birikmiş olan itemleri aldığı yer oluyor;
/// İtemlerin Dizildiği yerde burada Bulunuyor 
/// </summary>
public interface IItemController
{
    void Init(StackData stackData);
    ItemType GetItemType();
    (ItemType, Item, bool) GetValue();
    (ItemType, Item, bool) GetValue(ItemType itemType);
    void SetValue(ItemType itemType);
}