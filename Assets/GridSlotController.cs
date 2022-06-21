using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GridSlotController : MonoBehaviour
{
    public int x;
    public int y;
    public int h;
    public float widthSize;
    public float lengthSize;
    public float heightSize;
    public List<GridSlot> slotList = new List<GridSlot>();
    public int currentIndex;

    public GameObject sampleObject;

    [Button]
    public void ReSize()
    {
        slotList.Clear();
        for (var k = 0; k < h; k++)
        {
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    var clone = new GridSlot
                    {
                        position = transform.position + new Vector3(i * lengthSize, k * heightSize, j * widthSize)
                    };
                    slotList.Add(clone);
                }
            }
        }
    }

    [Button]
    public void CreateObject()
    {
        var clone = Instantiate(sampleObject, transform);
        clone.SetActive(true);
        var position = GetPosition();
        clone.transform.localPosition = position.position;
        position.isFull = true;
        position.slotInObject = clone;
    }

    public GridSlot GetPosition()
    {
        if (currentIndex >= slotList.Count - 1)
        {
            h += h;
            ReSize();
        }

        var result = slotList[currentIndex];
        currentIndex++;
        return result;
    }

    [Button]
    public void Clear()
    {
        currentIndex = 0;
    }

    public GridSlot GetSlotObject()
    {
        if (currentIndex == 0)
        {
            return null;
        }
        currentIndex--;
        var result = slotList[currentIndex];
        return result;
    }
}

[Serializable]
public class GridSlot
{
    public bool isFull;
    public Vector3 position;
    public GameObject slotInObject;
}