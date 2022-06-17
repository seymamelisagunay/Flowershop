using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Yok Edelim
/// </summary>
public class TransferEffect : MonoBehaviour
{
    public List<MoveObject> effects = new List<MoveObject>();
    public int effectObjectCount = 35;
    public float duration;
    public Transform startPoint;
    public Transform endPoint;

    public MoveObject moveObject;

    public void SetEffectObject(int count,MoveObject effectPrefab = null,bool isPlay = false)
    {
        count = count > effectObjectCount ? effectObjectCount : count;
        for (int i = 0; i < count; i++)
        {
            effectPrefab = effectPrefab ? effectPrefab : moveObject;
            var cloneEffectPrefab = Instantiate(effectPrefab, transform);
            cloneEffectPrefab.transform.position = startPoint.position;
            effects.Add(cloneEffectPrefab);
        }

        if (isPlay)
        {
            EffectPlay();
        }
    }
    [Button]
    public void TestStart()
    {
        SetEffectObject(effectObjectCount,moveObject);
    }
    [Button]
    public void EffectPlay()
    {
        StartCoroutine(StartEffect());
    }
    private IEnumerator StartEffect()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            effects[i].Play(startPoint, endPoint,duration);
        }
    }
}
