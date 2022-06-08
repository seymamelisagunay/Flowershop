using System;
using _Game.Script.Bot;
using _Game.Script.Manager;
using DG.Tweening;
using ECM.Controllers;
using UnityEngine;

namespace _Game.Script.Character
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _animator;
        public bool IsQualified { get; set; }
        public bool IsEliminated { get; set; }
        public bool IsLoser { get; set; }
        public bool IsDeath { get; set; }
        public Transform hudPoint;
        public bool isOwner;
        public bool IsBot => false;
        public IInput Input { get; private set; }
        public Action OnLose { get; set; }
        public Action OnSpawn { get; set; }
        public GameObject ownerIndicator;
        public Action<PlayerController> OnDeath { get; set; }
        [SerializeField] private GameSettings gameSettings;
        public HudController hudController;
        private Transform _spawnPoint;
        public BaseCharacterController CharacterController { get; private set; }

        private void Awake()
        {
            CharacterController = GetComponent<BaseCharacterController>();
        }
        public void Init()
        {
            Input = GetComponent<IInput>();
            hudController.Init(this);
            CharacterController.speed = IsBot ? gameSettings.botSpeed : gameSettings.playerSpeed;
        }
        private void OnDestroy()
        {
        }
        private void OnCreateLevel()
        {
            if (isOwner)
            {
                var camera = Camera.main.GetComponent<CustomCameraFollow>();
                if (camera != null)
                    camera.SetTarget(this);
            }
            IsQualified = false;
            IsEliminated = false;
            Input.StartListen();
            if (isOwner)
            {
                ownerIndicator.SetActive(true);
                ownerIndicator.transform.DOLocalMoveY(3, 0.5f)
                    .SetLoops(6, LoopType.Yoyo)
                    .OnComplete(() => ownerIndicator.SetActive(false));
            }
            Spawn();
        }
        public void Spawn()
        {
            var position = _spawnPoint.transform.position + Vector3.up * 5f;
            transform.position = position;
            transform.rotation = _spawnPoint.transform.rotation;
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