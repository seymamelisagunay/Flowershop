using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemTypeList", menuName = "Gnarly Team/Variable/ItemType List")]
public class ItemTypeList : ScriptableObject
{
    public List<ItemType> value;
    public UnityEvent OnChangeVariable;
    
    public void Add(ItemType type)
    {
        value.Add(type);
        OnChangeVariable?.Invoke();
    }
    public void Remove(int index)
    {
        value.RemoveAt(index);
        OnChangeVariable?.Invoke();
    }
}