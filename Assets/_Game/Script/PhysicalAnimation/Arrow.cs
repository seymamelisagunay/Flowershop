using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public GameObject endValue;
    public float time;

    void Start()
    {
        gameObject.transform.DOLocalMove(endValue.transform.localPosition, time)
            .SetLoops(-1, LoopType.Yoyo);
    }
}