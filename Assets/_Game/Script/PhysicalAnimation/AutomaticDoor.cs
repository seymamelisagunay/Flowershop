using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutomaticDoor : MonoBehaviour
{
    public GameObject doorRight;
    public Vector3 closedDoorRight;
    public Vector3 openDoorRight;
    
    public GameObject doorLeft;
    public Vector3 closedDoorLeft;
    public Vector3 openDoorLeft;
    
    public float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            doorRight.transform.DOLocalMove(openDoorRight, time);
            doorLeft.transform.DOLocalMove(openDoorLeft, time);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        doorRight.transform.DOLocalMove(closedDoorRight, time);
        doorLeft.transform.DOLocalMove(closedDoorLeft, time);
    }
}