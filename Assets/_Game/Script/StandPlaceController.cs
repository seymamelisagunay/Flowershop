using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class StandPlaceController : MonoBehaviour, IItemPlaceController
{
    private StackData _stackData;
    private ItemList _itemList;
    public List<GridSlot> slotList = new List<GridSlot>();
    public int currentIndex;
    [Space] public int firstFloorCount;
    public int secondFloorCount;
    public float firstFloorCircleRadius;
    public float firstFloorHeight;
    public float secondFloorCircleRadius;
    public float secondFloorHeight;
    private float currentRadius;
    private int currentCount;
    private float currentHeight;
    public Transform parent;
    public GridSlot prefab;
    [Space] public Item sampleObject;

    public void Init(ItemList itemList, StackData stackData)
    {
        _itemList = itemList;
        _stackData = stackData;
        FirstSizer();
        foreach (var type in _stackData.ProductTypes)
        {
            var item = itemList.GetItemPrefab(type);
            sampleObject = item;
            CreateObject();
        }
    }

    [Button]
    private void FirstSizer()
    {
        currentRadius = secondFloorCircleRadius;
        currentCount = secondFloorCount;
        currentHeight = secondFloorHeight;
        ReSize();
        currentHeight = firstFloorHeight;
        currentRadius = firstFloorCircleRadius;
        currentCount = firstFloorCount;
        ReSize(true);
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
    
    [Button]
    public void Clear()
    {
        currentIndex = 0;
        foreach (var grid in slotList)
        {
            DestroyImmediate(grid.gameObject);
        }

        slotList.Clear();
    }

    public GridSlot GetPosition()
    {
        if (currentIndex > slotList.Count - 1)
        {
            return null;
        }

        var result = slotList[currentIndex];
        currentIndex++;
        return result;
    }

    [Button]
    public void CreateObject()
    {
        var position = GetPosition();
        if (position == null)
            return;

        var clone = Instantiate(sampleObject, parent);
        clone.transform.parent = position.transform;
        clone.Play(Vector3.zero);
        position.isFull = true;
        position.slotInObject = clone;
    }

    [Button]
    public void ReSize(bool isCounting = false)
    {
        if (!isCounting)
            slotList.Clear();

        for (int i = 0; i < currentCount; ++i)
        {
            var num = (float) (i * 1.0) / currentCount;
            var angle = (float) num * Mathf.PI * 2;
            var x = Mathf.Sin(angle) * currentRadius;
            var y = Mathf.Cos(angle) * currentRadius;
            var pos = new Vector3(x, currentHeight, y) + transform.position;
            var clone = Instantiate(prefab, transform);
            clone.transform.position = pos;
            clone.slotPosition = pos;
            slotList.Add(clone);
        }
    }

    public int AvailableSlotCount()
    {
        var list = slotList.FindAll(x => !x.isFull);
        return list.Count;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        var pos = transform.position;
        pos.y = firstFloorHeight;
        Gizmos.DrawWireSphere(pos, firstFloorCircleRadius);
        pos.y = secondFloorHeight;
        Gizmos.DrawWireSphere(pos, secondFloorCircleRadius);
    }
#endif
}