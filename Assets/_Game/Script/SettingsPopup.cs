using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    public GameObject openMusic;
    public GameObject closeMusic;

    public GameObject openSound;
    public GameObject closeSound;

    public GameObject openVibration;
    public GameObject closeVibration;

    /// <summary>
    /// Panel açıldığı 
    /// </summary>
    public void Init()
    {
        if (SettingsController.instance.isMusicOpen)
        {
            openMusic.SetActive(true);
            closeMusic.SetActive(false);
        }
        else
        {
            openMusic.SetActive(false);
            closeMusic.SetActive(true);
        }

        if (SettingsController.instance.isSoundOpen)
        {
            openSound.SetActive(true);
            closeSound.SetActive(false);
        }
        else
        {
            openSound.SetActive(false);
            closeSound.SetActive(true);
        }

        if (SettingsController.instance.isVibrationOpen)
        {
            openVibration.SetActive(true);
            closeVibration.SetActive(false);
        }
        else
        {
            openVibration.SetActive(true);
            closeVibration.SetActive(true);
        }

    }

    public void Music()
    {
        if (!SettingsController.instance.isMusicOpen)
        {
            openMusic.SetActive(true);
            closeMusic.SetActive(false);
            SettingsController.instance.OpenMusic();
        }
        else
        {
            openMusic.SetActive(false);
            closeMusic.SetActive(true);
            SettingsController.instance.CloseMusic();

        }
    }

    public void Sound()
    {
        if (!SettingsController.instance.isSoundOpen)
        {
            openSound.SetActive(true);
            closeSound.SetActive(false);
            SettingsController.instance.OpenSound();
        }
        else
        {
            openSound.SetActive(false);
            closeSound.SetActive(true);
            SettingsController.instance.CloseSound();
        }
    }

    public void Vibration()
    {
        if (!SettingsController.instance.isVibrationOpen)
        {
            openVibration.SetActive(true);
            closeVibration.SetActive(false);
            SettingsController.instance.OpenVibration();
        }
        else
        {
            openVibration.SetActive(true);
            closeVibration.SetActive(true);
            SettingsController.instance.CloseVibration();
        }
    }

}
