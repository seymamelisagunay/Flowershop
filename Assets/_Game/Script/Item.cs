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
    // public FloatVariable moveDuration;
    public float moveDurationValue;
    public AnimationCurve curve;

    public UnityEvent onComplete;
    
    
    public Item Play(Vector3 endPoint, bool isDelete = false)
    {
        // transform.DOLocalJump(endPoint, 0.7f, 2, moveDuration.Value).OnComplete(() =>
        // {
        //     onComplete?.Invoke();
        //     if (isDelete)
        //         Destroy(gameObject);
        // });
        DOVirtual.DelayedCall(moveDurationValue, () =>
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Debug.Log("Rose Test : " + transform.localScale);
            var endValue = transform.localScale * 2.4f;
            transform.DOScale(endValue, 0.35f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                Debug.Log("Rose Test finish : " + transform.localScale);
            });
        });

        DOVirtual.Float(0, 1, moveDurationValue, (value) =>
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                endPoint, value) + new Vector3(0, curve.Evaluate(value), 0);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (isDelete)
                Destroy(gameObject);
        });


        return this;
    }


    public Item Play(Transform endPoint, bool isDelete = false)
    {
        DOVirtual.Float(0, 1, moveDurationValue, (value) =>
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
                t += Time.deltaTime * moveDurationValue;
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
                t += Time.deltaTime * moveDurationValue;
                transform.localPosition = Vector3.Lerp(startingPosition, pos + Vector3.up * curve.Evaluate(t), t);
                yield return 0;
            }
        }
    }
}