using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Bool Variable", menuName = "Gnarly Team/Variable/Bool Variable", order = 1)]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool _value;

    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            OnChangeVariable?.Invoke();
        }
    }

    public Action OnChangeVariable;

    private void OnValidate()
    {
        Value = _value;
    }
}