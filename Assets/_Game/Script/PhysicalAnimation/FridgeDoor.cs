using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FridgeDoor : MonoBehaviour
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
            doorRight.transform.DOLocalRotate(openDoorRight, time);
            doorLeft.transform.DOLocalRotate(openDoorLeft, time);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        doorRight.transform.DOLocalRotate(closedDoorRight, time);
        doorLeft.transform.DOLocalRotate(closedDoorLeft, time);
    }
}