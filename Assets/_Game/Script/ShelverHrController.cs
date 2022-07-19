using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelverHrController : MonoBehaviour
{
    public ShelverController prefab;

    public void Init()
    {
        var clone = Instantiate(prefab);
    }
}