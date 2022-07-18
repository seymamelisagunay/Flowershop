using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StandController Görevi Müşteriye ve Gelen Bot çalışana etrafından bir yer vermek 
/// </summary>
public class StandController : MonoBehaviour
{
    public List<GridSlot> customerSlot = new List<GridSlot>();

    public GridSlot GetCustomerSlot()
    {
        var resultObject = customerSlot.RandomSelectObject();
        return resultObject;
    }
}