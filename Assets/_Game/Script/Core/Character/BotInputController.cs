using _Game.Script.Bot;
using _Game.Script.Manager;
using Sources.Bot;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Script.Core.Character
{
    public class BotInputController : MonoBehaviour, IInput
    {
        public Vector3 Direction { get; private set; }
        private StateMachine _stateMachine;
        private PlayerController _playerController;
        private bool _listenInput;

        private void Awake()
        {
            _stateMachine = new StateMachine();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (Match.Instance.State != MatchState.Begin) return;
            _stateMachine?.Update();
        }

        public void SetDirection(Vector3 direction)
        {
            if (_playerController.IsDeath) return;
            if (!_listenInput) return;
            Direction = Quaternion.Euler(0, Random.Range(-10f, 10f), 0) * direction;
        }

        public void StartListen()
        {
            _listenInput = true;
        }

        public void StopListen()
        {
            ClearDirection();
            _listenInput = false;
        }

        public void ClearDirection()
        {
            Direction = Vector3.zero;
        }
    }
}