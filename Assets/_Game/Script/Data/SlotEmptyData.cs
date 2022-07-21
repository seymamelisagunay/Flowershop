using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEngine.Events;

[Serializable]
public class SlotEmptyData
{
    /// <summary>
    /// Slot satın alındımı alınmadımı onu anladımığımız yer oluyor 
    /// </summary>
    [SerializeField]
    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
                OnChangeVariable?.Invoke(this);
            OnChangeIsOpenVarible?.Invoke(this);
        }
    }
    /// <summary>
    /// Slot satın alma fiatı değişken adı ileride değişmesi daha açıklayıcı olması gerek 
    /// </summary>
    [SerializeField]
    private int _price = 10;
    public int Price
    {
        get => _price;
        set
        {
            _price = value;
            OnChangeVariable?.Invoke(this);
        }
    }
    /// <summary>
    /// Ödenene Slota Ödenen para miktarı açmak için
    /// </summary>
    [SerializeField]
    private int _currenctPrice = 0;
    public int CurrenctPrice
    {
        get => _currenctPrice;
        set
        {
            _currenctPrice = value;
            OnChangeVariable?.Invoke(this);
        }
    }

    [InfoBox("Değer Değiştirildiğinde Data Döndürür")]
    [Space]
    public UnityEvent<SlotEmptyData> OnChangeVariable;
    [HideInInspector]
    public UnityEvent<SlotEmptyData> OnChangeIsOpenVarible;

    public void OnValidate()
    {
        IsOpen = _isOpen;
        Price = _price;
        CurrenctPrice = _currenctPrice;
    }
}



