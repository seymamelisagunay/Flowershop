using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script;
using _Game.Script.Manager;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class HudDotIdle : MonoBehaviour
{
    private Transform _camera;
    public TMP_Text threeDot;
    public bool playEffect;

    public void Start()
    {
        _camera = GameManager.instance.customCamera.transform;
    }

    public void Play()
    {
        threeDot.text = "";
        playEffect = true;
        StartCoroutine(Effect());
    }

    public void Update()
    {
        transform.LookAt(_camera);
    }

    public void Stop()
    {
        threeDot.text = "";
        playEffect = false;
        StopCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        int i=1;
        while (playEffect)
        {
            threeDot.text += '.';
            yield return new WaitForSeconds(0.3f);
            if (i % 3 == 0)
            {
                threeDot.text = "";
                yield return new WaitForSeconds(0.3f);
            }
            i++;
        }
    }

}
