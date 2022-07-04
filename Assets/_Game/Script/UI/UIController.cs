using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UIController : MonoBehaviour
{
    public GameObject upgradesPopup;
    public GameObject settingsPopup;
    public GameObject workersPart;
    public GameObject machinesPart;
    public GameObject carryPopupLocked;
    public GameObject shelverPopupLocked;
    public GameObject perfumePopupLocked;
    public GameObject delightPopupLocked;
    private void Awake()
    {
        UpgradesCloseButton();
        SettingsCloseButton();
    }

    public void UpgradeButton()
    {
        upgradesPopup.SetActive(true);
    }

    public void SettingsButton()
    {
        settingsPopup.SetActive(true);
    }

    public void UpgradesCloseButton()
    {
        upgradesPopup.SetActive(false);
    }

    public void SettingsCloseButton()
    {
        settingsPopup.SetActive(false);
    }

    public void WorkersButton()
    {
        machinesPart.SetActive(false);
        workersPart.SetActive(true);
    }

    public void MachinesButton()
    {
        workersPart.SetActive(false);
        machinesPart.SetActive(true);
    }

    //After that, there are functions to run when opening the slot.
    
    [Button()]
    public void CarryPopupUnlock()
    {
        carryPopupLocked.SetActive(false);
    }

    public void ShelverPopupUnlock()
    {
        shelverPopupLocked.SetActive(false);
    }

    public void PerfumePopupUnlock()
    {
        perfumePopupLocked.SetActive(false);
    }

    public void DelightPopupUnlock()
    {
        delightPopupLocked.SetActive(false);
    }
}
