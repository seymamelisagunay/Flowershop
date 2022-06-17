using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarDoor : MonoBehaviour
{
    public GameObject door;
    public Vector3 closedDoor;
    public Vector3 openDoor;
    public float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            door.transform.DOLocalRotate(openDoor, time);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        door.transform.DOLocalRotate(closedDoor, time);
    }
}
