using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StandController Görevi Müþteriye ve Gelen Bot çalýþana etrafýndan bir yer vermek 
/// </summary>
public class StandController : MonoBehaviour
{
    public List<GridSlot> customerSlot = new List<GridSlot>();
    public GridSlot GetCustomerSlot()
    {
        var resultObject = customerSlot.Find(x => !x.isFull);
        return resultObject;
    }


}
