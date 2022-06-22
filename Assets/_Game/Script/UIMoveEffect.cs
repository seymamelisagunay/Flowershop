using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIMoveEffect : MonoBehaviour
{
    public Vector3 offset;
    public float duration;
    void Start()
    {
        var rect = (RectTransform) transform;
        var targetPos = transform.position;
        targetPos += offset;
        rect.DOMove(targetPos, duration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
}
