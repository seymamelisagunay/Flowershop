using System;
using _Game.Script.Bot;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using DG.Tweening;
using ECM.Controllers;
using UnityEngine;

namespace _Game.Script.Character
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _animator;
        public Transform hudPoint;
        public PlayerSettings playerSettings;
        public IInput Input { get; private set; }
        public Action OnSpawn { get; set; }
        public PlayerHudController hudController;
        private Transform _spawnPoint;
        public MovementController CharacterController { get; private set; }

        private void Awake()
        {
            CharacterController = GetComponent<MovementController>();
        }
        public void Init(Transform spawnPoint)
        {
            Input = GetComponent<IInput>();
            _spawnPoint = spawnPoint;
            // hudController.Init(this);
            OnOpenLevel();
        }
        private void OnDestroy()
        {
        }
        /// <summary>
        ///  Oyun Açıldığında Burası tetiklencek Playerda.
        /// </summary>
        private void OnOpenLevel()
        {
            if (!playerSettings.isBot)
            {
                var camera = Camera.main.GetComponent<CustomCameraFollow>();
                if (camera != null)
                    camera.SetTarget(this);
            }
            Input.StartListen();
            
            Spawn(_spawnPoint.position,_spawnPoint.rotation);
        }
        /// <summary>
        /// Player Controllerı spawn yaptığımız yer oluyor .
        /// </summary>
        /// <param name="spawnPoint">spawn Noktasını Vector3 cinsinden veriyoruz
        /// Spawn noktasının yukarı yönde 5 ile çarpıyoruz yukarıdan düşme etkisi
        /// yapıyor .</param>
        /// <param name="spawnRotation"></param>
        public void Spawn(Vector3 spawnPoint,Quaternion spawnRotation)
        {
            var position = spawnPoint + (Vector3.up * 5f);
            transform.position = position;
            transform.rotation = spawnRotation;
            CharacterController.pause = false;
            OnSpawn?.Invoke();
        }
        public void OnTriggerEnter(Collider other)
        {
        }
        private void OnCollisionEnter(Collision collision)
        {
        }
    }
}