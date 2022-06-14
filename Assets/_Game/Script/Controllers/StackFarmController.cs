using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// StackController kendi içinde bir save sistemi olacak
/// Bir tane kendi altýnda scriptable object yapýsý olacak 
/// </summary>
public class StackFarmController : MonoBehaviour, IStackController
{
    [HideInInspector]
    private SlotController _slotController;
    [ReadOnly]
    public StackData stackData;

    public void Init(SlotController slotController)
    {
        _slotController = slotController;

        stackData = _slotController.slot.stackData;

    }
    public void AddValue()
    {
    }

    public void RemoveValue()
    {
    }

    public void ReSize()
    {
    }
}
public interface IStackController
{
    void Init(SlotController slotController);
    void AddValue();
    void RemoveValue();
    void ReSize();

}
[Serializable]
public class StackData
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private int _maxProductCount;
    public int MaxProductCount
    {
        get => _maxProductCount;
        set
        {
            _maxProductCount = value;
            OnChangeVariable?.Invoke(this);
        }
    }
    [SerializeField]
    private float _productionRate;
    public float ProductionRate
    {
        get => _productionRate;
        set
        {
            _productionRate = value;
            OnChangeVariable?.Invoke(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private List<ProductType> _productTypes;
    public List<ProductType> ProductTypes
    {
        get => _productTypes;
        set
        {
            _productTypes = value;
            OnChangeVariable?.Invoke(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public UnityEvent<StackData> OnChangeVariable;

    public void OnValidate()
    {
        ProductTypes = _productTypes;
        MaxProductCount = _maxProductCount;
    }
}

