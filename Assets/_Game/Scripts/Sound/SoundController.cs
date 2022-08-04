using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        private bool _isRun;
        private Action<SoundController> _onFinished;
        [HideInInspector] public string key;
        private bool _isClosing;
        private float _volumeValue;
        private float _volumeTarget;
        private float _fadeSpeed = 1;
        private float _defaultPitch;

        public void Play(string key, Action<SoundController> onFinished, float randomPitchRange = 0)
        {
            this.key = key;
            _defaultPitch = source.pitch;
            source.pitch += Random.Range(-randomPitchRange, randomPitchRange);
            source.Play();
            _volumeValue = source.volume;
            _volumeTarget = source.volume;
            _isRun = true;
            _onFinished = onFinished;
        }

        private void Update()
        {
            if (_isRun)
            {
                if (!source.isPlaying || source.volume <= 0f)
                {
                    _isRun = false;
                    _onFinished.Invoke(this);
                    source.pitch = _defaultPitch;
                    Destroy(gameObject);
                }
                else if (_volumeTarget != source.volume)
                {
                    source.volume = Mathf.MoveTowards(source.volume, _volumeTarget, Time.deltaTime * _fadeSpeed);
                }
            }
        }

        public void Stop()
        {
            _volumeTarget = 0;
        }

        public void Kill()
        {
            source.volume = 0;
        }

        public void SetVolumeLevel(float volumeLevelPercent, float fadeSpeed = 1f)
        {
            _volumeTarget = _volumeValue * volumeLevelPercent;
            _fadeSpeed = fadeSpeed;
        }

        public float GetCurrentTime()
        {
            return source.time;
        }
    }
}