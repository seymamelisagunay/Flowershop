using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class Machine : MonoBehaviour
{
    public GameObject machine;
    public float strength;
    public int vibrato;

    [Button()]
    public void Test()
    {
        MachineShake(machine,strength, vibrato);
    }

    private void MachineShake(GameObject _machine,float _strength, int _vibrato)
    {
        _machine.transform.DOShakeScale(2f, _strength, _vibrato, 3f, false).SetLoops(-1, LoopType.Restart);
    }

}
