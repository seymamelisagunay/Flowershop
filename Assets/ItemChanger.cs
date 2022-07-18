using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class ItemChanger : MonoBehaviour
{
    public GameObject rose;
    public GameObject roseBasket;
    public float time;

    [Button()]
    public void openRose()
    {
        rose.SetActive(true);
        roseBasket.SetActive(false);
    }

    [Button()]
    public void openRoseBasket()
    {
        roseBasket.transform.localScale = new Vector3(0,0,0);
        roseBasket.SetActive(true);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(rose.transform.DOScale(0f,time).SetEase(Ease.InOutBack));
        mySequence.Append(roseBasket.transform.DOScale(1f, time).SetEase(Ease.InOutBack));
    }
}
