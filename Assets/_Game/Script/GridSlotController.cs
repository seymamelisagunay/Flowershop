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
    public Transform parent;
    public GameObject sampleObject;

    [Button]
    public void ReSize(bool isCounting = false)
    {
        var totalCounter = 0;
        if (!isCounting)
            slotList.Clear();

        for (var k = 0; k < h; k++)
        {
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    if (currentIndex >= totalCounter && isCounting)
                    {
                        totalCounter++;
                        continue;
                    }
                    var clone = new GridSlot();
                    clone.position = new Vector3(i * lengthSize, k * heightSize, j * widthSize);
                    slotList.Add(clone);
                }
            }
        }
    }

    [Button]
    public void CreateObject()
    {
        var clone = Instantiate(sampleObject, parent);
        clone.SetActive(true);
        var position = GetPosition();
        clone.transform.localPosition = position.position;
        position.isFull = true;
        position.slotInObject = clone;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GridSlot GetPosition()
    {
        if (currentIndex >= slotList.Count - 1)
        {
            h += h;
            ReSize(true);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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