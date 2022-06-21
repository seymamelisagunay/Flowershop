using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int price;
    public float moveSpeedMultiplier;

    public AnimationCurve curve;

    public void Play(Transform endPoint, bool isDelete = false)
    {   
        DOVirtual.Float(0, 1, moveSpeedMultiplier, (value) =>
        {
            var currentPos = Vector3.Lerp(transform.position, endPoint.position, value);
            transform.position = currentPos;
        }).SetEase(Ease.InOutBack).SetLink(gameObject).OnComplete(() =>
        {
            if (isDelete)
                Destroy(gameObject);
        });
        // StartCoroutine(LerpToPosition(finishPoint, curved));
    }

    public IEnumerator LerpToPosition(Vector3 pos, bool curved = false)
    {
        if (!curved)
        {
            var startingPosition = transform.localPosition;
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * moveSpeedMultiplier;
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
                t += Time.deltaTime * moveSpeedMultiplier;
                transform.localPosition = Vector3.Lerp(startingPosition, pos + Vector3.up * curve.Evaluate(t), t);
                yield return 0;
            }
        }
    }
}