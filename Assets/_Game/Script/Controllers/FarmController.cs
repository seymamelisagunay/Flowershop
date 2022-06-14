using NaughtyAttributes;
using System.Collections;
using UnityEngine;


public class FarmController : MonoBehaviour
{
    private SlotController _slotController;
    public ProductType productType;
    [ReadOnly]
    public StackData stackData;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotController"></param>
    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        stackData = _slotController.slot.stackData;

        StartCoroutine(Creator());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Creator()
    {
        while (true)
        {
            yield return new WaitForSeconds(stackData.ProductionRate);
            if (stackData.ProductTypes.Count < stackData.MaxProductCount)
            {
                stackData.ProductTypes.Add(productType);
                Debug.Log("Test ! bir");
            }
        }
    }
    [Button]
    public void TestRemove()
    {
        stackData.ProductTypes.Clear();
    }
}
