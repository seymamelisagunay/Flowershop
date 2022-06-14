using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// StackController kendi i�inde bir save sistemi olacak
/// Bir tane kendi alt�nda scriptable object yap�s� olacak 
/// </summary>
public class StackController : MonoBehaviour
{

    public void Init()
    {

    }





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

