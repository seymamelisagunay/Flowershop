using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Sound
{
    public static class SoundManager
    {
        private static List<SoundController> _activeSounds = _activeSounds = new List<SoundController>();
        private static readonly SoundParent _soundParent;
        private static Dictionary<string, int> _lastPlayedIndex;

        static SoundManager()
        {
            _soundParent = new GameObject(name: "SoundParent", typeof(SoundParent)).GetComponent<SoundParent>();
            _lastPlayedIndex = new Dictionary<string, int>();
            GameObject.DontDestroyOnLoad(_soundParent);
        }

        public static void PlayMusic(string key)
        {
            if (!_activeSounds.Exists(x => x.key == key))
                Play(key, 0, (sound) => { }, null);
        }

        public static void PlayMusic(SoundController prefab)
        {
            if (!_activeSounds.Exists(x => x.key == prefab.key))
                Play(prefab);
        }


        public static void PlayMusic(string key, Action<SoundController> created)
        {
            if (_activeSounds.Exists(x => x.key == key)) return;
            Play(key, 0, null, created);
        }

        public static void PlayMusic(SoundController prefab, Action<SoundController> onCreated)
        {
            if (!_activeSounds.Exists(x => x.key == prefab.key))
                Play(prefab, 0, null, onCreated);
        }

        public static void PlaySoundEffect(SoundController prefab)
        {
            Play(prefab);
        }

        public static void PlaySoundEffect(string key)
        {
            PlaySoundEffect(key, 0, (sound) => { });
        }

        public static void PlaySoundEffect(string key, float pitchRange)
        {
            PlaySoundEffect(key, pitchRange, (sound) => { });
        }

        public static void PlaySoundEffect(string key, float randomPitchRange,
            Action<SoundController> onFinished, Action<SoundController> onCreated = null)
        {
            Play(key, randomPitchRange, onFinished, onCreated);
        }


        private static void Play(string key, float randomPitchRange, Action<SoundController> finished = null,
            Action<SoundController> created = null)
        {
            LoadResource(key, (prefab) =>
            {
                if (prefab == null)
                {
                    Debug.LogError($"Missing sound prefab key {key}");
                    return;
                }

                var sound = Object.Instantiate(prefab, _soundParent.transform);
                sound.transform.localPosition = Vector3.zero;
                sound.Play(key, OnFinished + finished, randomPitchRange);
                _activeSounds.Add(sound);
                created?.Invoke(sound);
            });
        }

        public static void LoadResource(string key, Action<SoundController> onLoad)
        {
            if (_soundParent)
                _soundParent.LoadPrefab(key, _lastPlayedIndex, onLoad);
        }

        private static void Play(SoundController soundPrefab, float randomPitchRange = 0,
            Action<SoundController> finished = null,
            Action<SoundController> created = null)
        {
            var sound = Object.Instantiate(soundPrefab, _soundParent.transform);
            sound.transform.localPosition = Vector3.zero;
            sound.Play(soundPrefab.key, OnFinished + finished, randomPitchRange);
            _activeSounds.Add(sound);
            created?.Invoke(sound);
        }

        public static void SetVolumeLevel(string key, float volumeLevelPercent, float fadeSpeed = 1)
        {
            foreach (var sound in _activeSounds)
            {
                if (sound.key == key)
                {
                    sound.SetVolumeLevel(volumeLevelPercent, fadeSpeed);
                }
            }
        }

        public static void StopSound(string key)
        {
            foreach (var sound in _activeSounds)
            {
                if (sound.key == key)
                {
                    sound.Stop();
                }
            }
        }

        public static void Kill(string key)
        {
            foreach (var sound in _activeSounds)
            {
                if (sound.key == key)
                {
                    sound.Kill();
                }
            }
        }

        private static void OnFinished(SoundController sound)
        {
            _activeSounds.Remove(sound);
        }

        public static SoundController GetSound(string key)
        {
            return _activeSounds.Find(x => x.key == key);
        }
    }
}