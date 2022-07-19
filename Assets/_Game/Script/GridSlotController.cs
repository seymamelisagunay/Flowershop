using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class GridSlotController : MonoBehaviour, IItemPlaceController
{
    public int x;
    public int y;
    public int h;
    public float widthSize;
    public float lengthSize;
    public float heightSize;
    public GridSlot prefab;
    public List<GridSlot> slotList = new List<GridSlot>();
    public int currentIndex;
    public Transform parent;
    public Item sampleObject;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isCounting"></param>
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

                    var clone = Instantiate(prefab, parent);
                    clone.slotPosition = new Vector3(i * lengthSize, k * heightSize, j * widthSize);
                    clone.transform.localPosition = clone.slotPosition;
                    slotList.Add(clone);
                }
            }
        }
    }

    public void Increase()
    {
        h += h;
        ReSize(true);
    }

    public void ReOrder()
    {
        var isFullList = slotList.FindAll(x => x.isFull);
        currentIndex = 0;
        foreach (var gridSlot in isFullList)
        {
            var item = gridSlot.slotInObject;
            var grid = GetPosition();
            item.Play(grid.slotPosition);
        }
    }

    [Button]
    public void CreateObject()
    {
        var clone = Instantiate(sampleObject, parent);
        clone.gameObject.SetActive(true);
        var gridSlot = GetPosition();
        clone.Play(gridSlot.slotPosition);
        gridSlot.isFull = true;
        gridSlot.slotInObject = clone;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GridSlot GetPosition()
    {
        return slotList.Find(x => !x.isFull);
    }

    /// <summary>
    /// 
    /// </summary>
    [Button]
    public void Clear()
    {
        currentIndex = 0;
        slotList.Clear();
    }

    public void ClearAll()
    {
        foreach (var grid in slotList)
        {
            DestroyImmediate(grid.gameObject);
        }

        currentIndex = 0;
        slotList.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GridSlot GetSlotObject()
    {
        return slotList.Find(x => x.isFull);
    }

    public GridSlot GetSlotObject(ItemType itemType)
    {
        var index = slotList.FindIndex(x => x.isFull && x.slotInObject.itemType == itemType);
        return slotList[index];
    }

    public List<GridSlot> GetSlotObjects(ItemType itemType)
    {
        var result = slotList.FindAll(x => x.isFull && x.slotInObject.itemType == itemType);
        return result;
    }
}

public interface IItemPlaceController
{
    GridSlot GetSlotObject();
    void Clear();
    GridSlot GetPosition();
    void CreateObject();
    void ReSize(bool isCounting = false);
}