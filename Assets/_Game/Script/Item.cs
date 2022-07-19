using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int price;
    public FloatVariable moveDuration;
    public AnimationCurve curve;

    public UnityEvent onComplete;

    public Item Play(Vector3 endPoint, bool isDelete = false)
    {
        transform.DOLocalJump(endPoint, 0.7f, 2, moveDuration.Value).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });
        // DOVirtual.Float(0, 1, moveDuration.Value, (value) =>
        // {
        //     endPoint = transform.TransformPoint(transform.localPosition + endPoint);
        //     transform.position = Vector3.Lerp(transform.localPosition, endPoint, value) +
        //                          new Vector3(0, curve.Evaluate(value), 0);
        // }).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
        // {
        //     onComplete?.Invoke();
        //     if (isDelete)
        //         Destroy(gameObject);
        // });

        return this;
    }

    public Item Play(Transform endPoint, bool isDelete = false)
    {
        DOVirtual.Float(0, 1, moveDuration.Value, (value) =>
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                endPoint.localPosition + (Vector3.up * curve.Evaluate(value)), value);
        }).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });
        return this;
    }

    public void AddOnComplete(UnityAction callback)
    {
        onComplete.AddListener(callback);
    }

    public void PlayScaleEffect(float duration, bool isDelete = false)
    {
        var originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });
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