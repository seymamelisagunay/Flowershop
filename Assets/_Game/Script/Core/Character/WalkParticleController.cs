using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Core.Character;
using UnityEngine;

public class WalkParticleController : MonoBehaviour
{
    public ParticleSystem walk;
    public MovementController movementController;
    private bool _isStop;

    private bool _playCheck;

    private void Update()
    {
        _isStop = movementController.moveDirection.magnitude < 0.1f;
        WalkParticle();
    }

    void WalkParticle()
    {
        if (_isStop)
        {
            if (_playCheck)
            {
                _playCheck = false;
                walk.Stop();
            }
        }
        else
        {
            if (!_playCheck)
            {
                _playCheck = true;
                walk.Play();
            }
        }
    }
}