using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierHrController : MonoBehaviour
{
    public CashierController prefab;
    public void Init()
    {
        var clone = Instantiate(prefab);
    }
}
