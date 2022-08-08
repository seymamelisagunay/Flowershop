using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UIController : MonoBehaviour
{
    public GameObject settingsPopup;
    private void Awake()
    {
        SettingsCloseButton();
    }
    public void SettingsButton()
    {
        settingsPopup.GetComponent<SettingsPopup>().Init();
        settingsPopup.SetActive(true);
    }
    public void SettingsCloseButton()
    {
        settingsPopup.SetActive(false);
    }

}
