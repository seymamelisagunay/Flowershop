using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetScale : MonoBehaviour
{
    public GameObject targetObject;
    public float time;
    public float scale;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            targetObject.transform.DOScaleX(scale, duration: time);
            targetObject.transform.DOScaleZ(scale, duration: time);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.DOScale(new Vector3(1,1,1), duration: time);
    }
}
