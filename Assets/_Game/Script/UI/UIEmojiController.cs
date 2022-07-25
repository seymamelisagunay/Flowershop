using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Manager;
using Assets._Game.Script.Variable;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class UIEmojiController : MonoBehaviour
{
    private Transform _camera;
    public TMP_Text item;
    public Image itemIcon;
    public Image finishIcon;
    public GameObject smile;

    private void Start()
    {
        _camera = GameManager.instance.customCamera.transform;
        CloseFinishIcon();
    }

    private void Update()
    {
        transform.LookAt(_camera);
    }

    public void CloseItemPanel()
    {
        item.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
    }

    public void SetItemText(string itemText)
    {
        item.SetText(itemText);
    }

    public void SetItemIcon(Sprite icon)
    {
        itemIcon.sprite = icon;
    }

    public void CloseFinishIcon()
    {
        finishIcon.sprite = null;
    }

    public void ShowCashDeskIcon()
    {
        CloseItemPanel();
        finishIcon.sprite = GameManager.instance.cashDeskIcon.Value;
    }
    public void ShowSmile()
    {
        smile.SetActive(true);
        CloseItemPanel();
        var emoji = GameManager.instance.emojiIcons.RandomSelectObject();
        finishIcon.sprite = emoji.Value;
    }
    //
    // public void CloseEverything()
    // {
    //     item.enabled = false;
    // }
    // public void OpenRoseIcon()
    // {
    //     CloseEverything();
    //     item.enabled = true;
    //     item.text = "3" + "/" + "1";
    // }
    // [Button()]
    // public void OpenRoseWaterIcon()
    // {
    //     CloseEverything();
    //     item.enabled = true;
    //     item.text = "3" + "/" + "1";
    //     // roseWaterIcon.SetActive(true);
    // }
    // public void OpenDelightIcon()
    // {
    //     CloseEverything();
    //     item.enabled = true;
    //     item.text = "3" + "/" + "1";
    //     // delightIcon.SetActive(true);
    // }
    // public void OpenCaseEmoji()
    // {
    //     CloseEverything();
    //     // caseEmoji.SetActive(true);
    // }
    // [Button()]
    // public void OpenSmile1Emoji()
    // {
    //     CloseEverything();
    //     // smile1Emoji.SetActive(true);
    // }
    // public void OpenSmile2Emoji()
    // {
    //     CloseEverything();
    //     // smile2Emoji.SetActive(true);
    // }
}