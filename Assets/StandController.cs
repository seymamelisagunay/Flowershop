using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StandController G�revi M��teriye ve Gelen Bot �al��ana etraf�ndan bir yer vermek 
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
