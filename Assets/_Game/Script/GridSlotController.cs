using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GridSlotController : MonoBehaviour,IItemPlaceController
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
                    var clone = Instantiate(prefab,transform);
                    clone.slotPosition = new Vector3(i * lengthSize, k * heightSize, j * widthSize);
                    slotList.Add(clone);
                }
            }
        }
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
        var position = GetPosition();
        clone.transform.localPosition = position.slotPosition;
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

    /// <summary>
    /// 
    /// </summary>
    [Button]
    public void Clear()
    {
        currentIndex = 0;
        slotList.Clear();
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

    public GridSlot GetSlotObject(ItemType itemType)
    {
       var slot =  slotList.Find(x => x.isFull && x.slotInObject.itemType == itemType);
       currentIndex--;
       return slot;
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