using System.Collections;
using System.Collections.Generic;
using _Game.Script;
using UnityEngine;

[CreateAssetMenu(fileName = "Stack Object List", menuName = "Gnarly Team/StackObjectData")]
public class ItemList : ScriptableObject
{
    public List<Item> objects;

    public Item GetStackObject(ItemType itemType)
    {
        var stackObject = objects.Find(x => x.itemType == itemType);
        return stackObject;
    }
}