using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Variable", menuName = "Gnarly Team/Variable/Float Variable")]
[Serializable]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float _value;
    public float Value
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