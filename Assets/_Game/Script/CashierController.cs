using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Bot;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using UnityEngine;
using UnityEngine.AI;

public class CashierController : MonoBehaviour
{
    private IInput _input;

    private NavMeshPath _path;
    private int _pathIndex;


    private void Start()
    {
        _input = GetComponent<IInput>();
        _input.StartListen();

        StartCoroutine(BotProgress());
    }

    private IEnumerator BotProgress()
    {
        var slotController = SlotManager.instance.GetSlotController(SlotType.CashDesk);

        var standGridSlot = slotController[0].GetComponentInChildren<CashDeskController>().cashierPoint;
        _path = new NavMeshPath();
        _pathIndex = 1;
        GameManager.instance.NavMesh.CalculatePath(transform.position, standGridSlot.transform.position, _path);
        yield return MoveToPoint(_path);
        transform.localEulerAngles = Vector3.zero;
    }

    private IEnumerator MoveToPoint(NavMeshPath path)
    {
        while (path.corners.Length != _pathIndex)
        {
            yield return new WaitForSeconds(0.1f);
            if (_pathIndex >= path.corners.Length) continue;

            if (path.corners.Length > 0)
            {
                var direction = path.corners[_pathIndex] - transform.position;
                _input.SetDirection(direction.normalized);
                if (direction.magnitude < 0.5f)
                    _pathIndex++;
            }
        }

        _input.ClearDirection();
    }
}