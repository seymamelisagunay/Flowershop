using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int Variable", menuName = "Gnarly Team/Variable/Int Variable")]
[Serializable]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            OnChangeVariable?.Invoke();
        }
    }
    [SerializeField]
    public UnityEvent OnChangeVariable;

    private void OnValidate()
    {
        Value = _value;
    }
}