using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierHrController : MonoBehaviour
{
    public CashierController prefab;
    public void Init(Vector3 createPosition)
    {
        Debug.Log("Test !"+createPosition);
        var clone = Instantiate(prefab);
        clone.transform.position = createPosition;
    }
}
