using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int price;
    public FloatVariable moveDuration;
    public AnimationCurve curve;

    private Action onComplete;

    public Item Play(Vector3 endPoint, bool isDelete = false)
    {
        transform.DOMove(endPoint,moveDuration.Value).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });
        return this;
    }

    public Item Play(Transform endPoint, bool isDelete = false)
    {
        DOVirtual.Float(0, 1, moveDuration.Value, (value) =>
        {
            transform.position = Vector3.Lerp(transform.position,
                endPoint.position + (Vector3.up * curve.Evaluate(value)), value);
            // var currentPos = Vector3.Lerp(transform.position, endPoint.position, value);
            // transform.position = currentPos;
        }).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });

        return this;
        // StartCoroutine(LerpToPosition(finishPoint, curved));
    }

    public void AddOnComplete(Action callback)
    {
        onComplete = callback;
    }

    public IEnumerator LerpToPosition(Vector3 pos, bool curved = false)
    {
        if (!curved)
        {
            var startingPosition = transform.localPosition;
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * moveDuration.Value;
                transform.localPosition = Vector3.Lerp(startingPosition, pos, t);
                yield return 0;
            }
        }
        else
        {
            var startingPosition = transform.localPosition;
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * moveDuration.Value;
                transform.localPosition = Vector3.Lerp(startingPosition, pos + Vector3.up * curve.Evaluate(t), t);
                yield return 0;
            }
        }
    }
}