using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CloudParticleController : MonoBehaviour
{
    public ParticleSystem cloud;

    private void Awake()
    {
        cloud.Stop();
    }

    [Button()]
    public void Play(bool isStart)
    {
        MakeCloud(isStart);
    }
    private void MakeCloud(bool isStart)
    {
        if (isStart)
        {
            cloud.Play();
        }
        else
        {
            cloud.Stop();
        }
    }
}
