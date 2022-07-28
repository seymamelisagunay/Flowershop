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
}