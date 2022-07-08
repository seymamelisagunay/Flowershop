using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class UIItemEmojiController : MonoBehaviour
{
    private Transform _camera;
    public TMP_Text itemNumber;
    public GameObject roseIcon;
    public GameObject roseWaterIcon;
    public GameObject delightIcon;
    public GameObject caseEmoji;
    public GameObject smile1Emoji;
    public GameObject smile2Emoji;

    private void Start()
    {
        _camera = Camera.main.transform;
        
        CloseEverything();
    }

    private void Update()
    {
        transform.LookAt(_camera);
    }

    public void CloseEverything()
    {
        itemNumber.enabled = false;
        roseIcon.SetActive(false);
        roseWaterIcon.SetActive(false);
        delightIcon.SetActive(false);
        caseEmoji.SetActive(false);
        smile1Emoji.SetActive(false);
        smile2Emoji.SetActive(false);
    }
    public void OpenRoseIcon()
    {
        CloseEverything();
        itemNumber.enabled = true;
        itemNumber.text = "3" + "/" + "1";
        roseIcon.SetActive(true);
    }
    [Button()]
    public void OpenRoseWaterIcon()
    {
        CloseEverything();
        itemNumber.enabled = true;
        itemNumber.text = "3" + "/" + "1";
        roseWaterIcon.SetActive(true);
    }
    public void OpenDelightIcon()
    {
        CloseEverything();
        itemNumber.enabled = true;
        itemNumber.text = "3" + "/" + "1";
        delightIcon.SetActive(true);
    }
    public void OpenCaseEmoji()
    {
        CloseEverything();
        caseEmoji.SetActive(true);
    }
    [Button()]
    public void OpenSmile1Emoji()
    {
        CloseEverything();
        smile1Emoji.SetActive(true);
    }
    public void OpenSmile2Emoji()
    {
        CloseEverything();
        smile2Emoji.SetActive(true);
    }
}
