using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource _defaultAudioSource;
    public List<SoundData> sounds = new List<SoundData>();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _defaultAudioSource = GetComponent<AudioSource>();
    }
    public void Play(string key)
    {
        var sound = sounds.Find(x => x.key.Equals(key));

        if (sound != null)
        {
            _defaultAudioSource.Stop();
            _defaultAudioSource.clip = sound.value;
            _defaultAudioSource.Play();
        }
    }
}
[Serializable]
public class SoundData
{
    public string key;
    public AudioClip value;
}
