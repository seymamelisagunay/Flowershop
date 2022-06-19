using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;

public class CashDeskController : MonoBehaviour
{
    [Tag]
    public string playerTag;
    [Tag]
    public string clientTag;
    public StackData stackData; 

    private SlotController _slotController;
    private CashDeskStackController _cashDeskStackController;
    
    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        stackData = _slotController.slot.stackData;
        // Burada Client 
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Player trigger içinde ise işlem yapılır
        if (other.CompareTag(playerTag))
        {
            
        }
    }
}


public class CashDeskStackController : MonoBehaviour, IStackController
{
    
    public (ProductType, GameObject, bool) GetValue()
    {

        return (ProductType.Money, gameObject, true);
    }

    public void SetValue(ProductType productType)
    {
    }
}