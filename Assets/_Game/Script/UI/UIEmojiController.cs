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
    public Image cashDesk;
    public Image smile;

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
        cashDesk.gameObject.SetActive(false);
        smile.gameObject.SetActive(false);
        cashDesk.sprite = null;
    }

    public void ShowCashDeskIcon()
    {
        cashDesk.gameObject.SetActive(true);
        smile.gameObject.SetActive(false);
        CloseItemPanel();
        cashDesk.sprite = GameManager.instance.cashDeskIcon.Value;
    }
    public void ShowSmile()
    {
        cashDesk.gameObject.SetActive(false);
        smile.gameObject.SetActive(true);
        CloseItemPanel();
        var emoji = GameManager.instance.emojiIcons.RandomSelectObject();
        smile.sprite = emoji.Value;
    }
}