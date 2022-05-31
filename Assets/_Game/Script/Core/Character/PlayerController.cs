using System;
using _Game.Script.Bot;
using _Game.Script.Manager;
using UnityEngine;

namespace _Game.Script.Core.Character
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsDeath { get; set; }
        public bool IsOwner => !Data.IsBot;
        public bool IsBot => Data.IsBot;
        public MatchData.PlayerData Data { get; private set; }
        public IInput Input { get; private set; }
        public Action<PlayerController> OnDeath { get; set; }
        private Vector3 _spawnPosition;
        private Animator _animator;

        public void Init(MatchData.PlayerData playerData, Transform spawnPoint)
        {
            Data = playerData;
            Input = GetComponent<IInput>();
            if (IsOwner)
            {
                var camera = Camera.main.GetComponent<CustomCameraFollow>();
                if (camera != null)
                    camera.SetTarget(this);
            }

            _spawnPosition = spawnPoint.transform.position;
            _spawnPosition.y += 5;
            Input.StartListen();
            Spawn();
        }

        private void Death()
        {
            IsDeath = true;
            Input.ClearDirection();
            OnDeath.Invoke(this);
        }

        private void Spawn()
        {
            transform.position = _spawnPosition;
        }
    }
}