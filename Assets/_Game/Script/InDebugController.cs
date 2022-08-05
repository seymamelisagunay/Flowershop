using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDebugController : MonoBehaviour
{
    public GameObject inDebug;
    private int _openClickCount = 10;
    private int _clickCounter;

    public void ClickUI()
    {
        _clickCounter++;
        Debug.Log(_clickCounter);
        if(_clickCounter > _openClickCount)
        {
            _clickCounter = 0;
            inDebug.SetActive(!inDebug.activeSelf);
        }
    }

}
