using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DelightWorker : Machine
{
    public Rig rig;
    private bool isStart;

    [Button]
    public void TestOverride()
    {
        Play(2, 0, 0);
    }

    public override void Play(float duration, float _strength, int _vibrato)
    {
        var cloudParticle = machine.GetComponent<CloudParticleController>();
        cloudParticle.Play(true);
        isStart = true;
        var waitTime = Random.Range(0.5f, 1f);
        DOVirtual.Float(1, 0.5f, waitTime, (v) => { rig.weight = v; }).SetLoops(4, LoopType.Yoyo);
        // StartCoroutine(HandEffect());
        DOVirtual.DelayedCall(duration, () =>
        {
            isStart = false;
            cloudParticle.Play(false);
        });
    }
}