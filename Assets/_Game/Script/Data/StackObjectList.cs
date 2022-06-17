using System.Collections;
using System.Collections.Generic;
using _Game.Script;
using UnityEngine;

[CreateAssetMenu(fileName = "Stack Object List", menuName = "Gnarly Team/StackObjectData")]
public class StackObjectList : ScriptableObject
{
    public List<StackObject> objects;

    public StackObject GetStackObject(ProductType productType)
    {
        var stackObject = objects.Find(x => x.productType == productType);
        return stackObject;
    }
}