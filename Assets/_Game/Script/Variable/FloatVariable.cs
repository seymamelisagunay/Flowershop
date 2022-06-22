using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Variable", menuName = "Gnarly Team/Variable/Float Variable")]
[Serializable]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float value;
    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            OnChangeVariable?.Invoke();
        }
    }

    public Action OnChangeVariable;

    private void OnValidate()
    {
        Value = value;
    }
}