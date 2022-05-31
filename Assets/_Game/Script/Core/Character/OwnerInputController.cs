using _Game.Script.Bot;
using _Game.Script.Manager;
using UnityEngine;

namespace _Game.Script.Core.Character
{
    public class OwnerInputController : MonoBehaviour, IInput
    {
        public Vector3 Direction { get; private set; }
        private PlayerController _playerController;
        private FloatingJoystick _joystick;
        private bool _listenInput;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            if (_playerController.IsOwner)
                _joystick = FindObjectOfType<FloatingJoystick>();
        }

        private void Update()
        {
            if (Match.Instance.State != MatchState.Begin) return;
            if (_playerController.IsOwner)
            {
                SetDirection(new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y));
            }
        }

        public void SetDirection(Vector3 direction)
        {
            if (!_listenInput) return;
            if (_playerController.IsDeath) return;
            Direction = direction;
        }

        public void ClearDirection()
        {
            Direction = Vector3.zero;
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
    }
}