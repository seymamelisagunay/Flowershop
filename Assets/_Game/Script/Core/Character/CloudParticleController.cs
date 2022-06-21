using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CloudParticleController : MonoBehaviour
{
    public ParticleSystem cloud;
    public bool cloudWork;

    private void Awake()
    {
        cloud.Stop();
    }

    [Button()]
    public void test()
    {
        MakeCloud();
    }
    private void MakeCloud()
    {
        if (cloudWork)
        {
            cloud.Play();
        }
        else
        {
            cloud.Stop();
        }
    }
}
