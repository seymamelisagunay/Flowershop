using NaughtyAttributes;
using System.Collections;
using _Game.Script.Controllers;
using DG.Tweening;
using UnityEngine;


public class FarmController : MonoBehaviour
{
    private SlotController _slotController;
    public ItemType itemType;
    private FarmStackController _farmStackController;
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
        OpenFarmEffect();
        _farmStackController = GetComponent<FarmStackController>();
        _farmStackController.Init(_slotController);
        StartCoroutine(Creator());
    }

    private void OpenFarmEffect()
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutElastic);
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
            
            if (!stackData.CheckMaxCount()) continue;
            
            _farmStackController.SetValue(itemType);
            Debug.Log("Test ! bir");
        }
    }
    
    
    [Button]
    public void TestRemove()
    {
        stackData.ProductTypes.Clear();
    }
}
