using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHUD : MonoBehaviour
{
    public UIEmojiController uiEmojiController;
    private void OnEnable()
    {
        uiEmojiController = GetComponent<UIEmojiController>();
    }
}
