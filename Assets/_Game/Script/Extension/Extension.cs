using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T RandomSelectObject<T>(this List<T> variable)
    {
        if (variable.Count <= 0)
            Debug.LogError("variable is below 0");

        var randomIndex = Random.Range(0, variable.Count);
        return variable[randomIndex];
    }
}