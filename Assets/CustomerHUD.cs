using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHUD : MonoBehaviour
{
    public UIEmojiController uiEmojiController;
    public HudDotIdle hudDotIdle;

    private void OnEnable()
    {
        uiEmojiController = GetComponent<UIEmojiController>();
        hudDotIdle = GetComponent<HudDotIdle>();
    }
}
