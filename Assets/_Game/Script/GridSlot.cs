using System;
using UnityEngine;

[Serializable]
public class GridSlot:MonoBehaviour
{
    public bool isFull;
    public Vector3 slotPosition;
    public Item slotInObject;
}