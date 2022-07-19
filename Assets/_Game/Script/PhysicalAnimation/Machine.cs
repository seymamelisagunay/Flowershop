using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class Machine : MonoBehaviour
{
    public GameObject machine;
    public float strength;
    public int vibrato;

    [Button()]
    public virtual void Test()
    {
        Play(2,strength, vibrato);
    }

    public virtual void Play(float duration, float _strength, int _vibrato)
    {
        transform.DOShakeScale(duration, _strength, _vibrato, 3f, false);
    }
}