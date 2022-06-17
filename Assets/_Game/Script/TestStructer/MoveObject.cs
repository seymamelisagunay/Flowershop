using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public void Play(Transform startPoint, Transform endPoint, float duration, bool isDelete = false)
    {
        DOVirtual.Float(0, 1, duration, (value) =>
        {
            var currentPos = Vector3.Lerp(startPoint.position, endPoint.position, value);
            transform.position = currentPos;
        }).SetEase(Ease.InOutBack).SetLink(gameObject).OnComplete(() =>
        {
            if (isDelete)
                Destroy(gameObject);
        });
    }
    
}
