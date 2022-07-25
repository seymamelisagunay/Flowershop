using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    private static System.Random _random = new System.Random();

    public static T RandomSelectObject<T>(this List<T> variable)
    {
        if (variable.Count <= 0)
            Debug.LogError("variable is below 0");

        var randomIndex = _random.Next(variable.Count);
        Debug.Log(randomIndex + " Random : " + variable.Count);
        return variable[randomIndex];
    }
}