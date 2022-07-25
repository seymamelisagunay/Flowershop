using System;
using _Game.Script.Bot;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using DG.Tweening;
using ECM.Controllers;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Game.Script.Character
{
    public class PlayerController : MonoBehaviour
    {
        public Transform hudPoint;
        public PlayerSettings playerSettings;
        public IInput Input { get; private set; }
        public Action OnSpawn { get; set; }
        private Transform _spawnPoint;
        public MovementController characterController { get; private set; }
        public Rig rig;
        private PlayerItemController _playerItemController;
        private Tweener _virtualTweener;
        private bool _isRigActive;
        private CustomCameraFollow _cameraFollow;
        public HudDotIdle hudDotIdle;

        private void Awake()
        {
            characterController = GetComponent<MovementController>();
            _playerItemController = GetComponent<PlayerItemController>();
            if (playerSettings.isBot) return;
            rig.weight = 0;
            hudDotIdle.gameObject.SetActive(false);
        }

        public void Init(Transform spawnPoint)
        {
            Input = GetComponent<IInput>();
            _spawnPoint = spawnPoint;
            OnOpenLevel();
            characterController.cameraFollow = _cameraFollow;
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (playerSettings.isBot) return;
            CharacterMoveEffect();
        }

        private void CharacterMoveEffect()
        {
            if (_playerItemController.stackData.ProductTypes.Count > 0)
            {
                if (!_isRigActive)
                {
                    _virtualTweener?.Kill();
                    _virtualTweener = DOVirtual.Float(0f, 1f, 0.5f, (value) => { rig.weight = value; });
                }

                _isRigActive = true;
            }
            else
            {
                if (_isRigActive)
                {
                    _virtualTweener?.Kill();
                    _virtualTweener = DOVirtual.Float(1f, 0f, 0.5f, (value) => { rig.weight = value; });
                }

                _isRigActive = false;
            }
        }

        /// <summary>
        ///  Oyun Açıldığında Burası tetiklencek Playerda.
        /// </summary>
        private void OnOpenLevel()
        {
            if (!playerSettings.isBot)
            {
                _cameraFollow = GameManager.instance.customCamera;
                if (_cameraFollow != null)
                    _cameraFollow.SetTarget(transform);
            }

            Input.StartListen();
            Spawn(_spawnPoint.position, _spawnPoint.rotation);
        }

        /// <summary>
        /// Player Controllerı spawn yaptığımız yer oluyor .
        /// </summary>
        /// <param name="spawnPoint">spawn Noktasını Vector3 cinsinden veriyoruz
        /// Spawn noktasının yukarı yönde 5 ile çarpıyoruz yukarıdan düşme etkisi
        /// yapıyor .</param>
        /// <param name="spawnRotation"></param>
        public void Spawn(Vector3 spawnPoint, Quaternion spawnRotation)
        {
            var position = spawnPoint + (Vector3.up * 5f);
            transform.position = position;
            transform.rotation = spawnRotation;
            characterController.pause = false;
            OnSpawn?.Invoke();
        }
    }
}