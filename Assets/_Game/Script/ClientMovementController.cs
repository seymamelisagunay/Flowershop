using System.Collections;
using System.Collections.Generic;
using _Game.Script.Bot;
using UnityEngine;

/// <summary>
/// Delete
/// </summary>
public class ClientMovementController : MonoBehaviour, IInput
{
    public Vector3 Direction { get; private set; }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public void ClearDirection()
    {
    }

    public void StartListen()
    {
    }

    public void StopListen()
    {
    }
}