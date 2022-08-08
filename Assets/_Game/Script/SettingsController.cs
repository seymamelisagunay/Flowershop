using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance;
    public AudioSource music;
    public bool isMusicOpen;
    public bool isSoundOpen;
    public bool isVibrationOpen;

    private void Awake()
    {
        isMusicOpen = PlayerPrefs.GetInt("isMusicOpen", 1) > 0 ? true : false;
        isVibrationOpen = PlayerPrefs.GetInt("isVibrationOpen", 1) > 0 ? true : false;
        isSoundOpen = PlayerPrefs.GetInt("isSoundOpen", 1) > 0 ? true : false;

        instance = this;


        if(isMusicOpen)
        {
            OpenMusic();
        }else{
            CloseMusic();
        }
        if(isVibrationOpen)
        {
            OpenVibration();
        }else{
            CloseVibration();
        }
    }

    public void CloseMusic()
    {
        PlayerPrefs.SetInt("isMusicOpen", 0);
        isMusicOpen = false;
        music.mute = true;
    }

    public void CloseSound()
    {
        PlayerPrefs.SetInt("isSoundOpen", 0);
        isSoundOpen = false;
    }

    public void CloseVibration()
    {
        PlayerPrefs.SetInt("isVibrationOpen", 0);
        isVibrationOpen = false;
        MMVibrationManager.SetHapticsActive(false);
    }

    public void OpenMusic()
    {
        PlayerPrefs.SetInt("isMusicOpen", 1);
        isMusicOpen = true;
        music.mute = false;
    }

    public void OpenSound()
    {
        PlayerPrefs.SetInt("isSoundOpen", 1);
        isSoundOpen = true;
    }

    public void OpenVibration()
    {
        PlayerPrefs.SetInt("isVibrationOpen", 1);
        isVibrationOpen = true;
        MMVibrationManager.SetHapticsActive(true);

    }







}
