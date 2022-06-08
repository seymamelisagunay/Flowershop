using _Game.Script.Bot;
using _Game.Script.Character;
using _Game.Script.Manager;
using UnityEngine;

namespace _Game.Script.Core.Character
{
    public class MovementController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;
        private float _speed;
        private float _rotateLerpFactor;
        public GameSettings GameSettings;
        private static readonly int Run = Animator.StringToHash("run");
        private IInput _input;
        private PlayerController _playerController;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _rigidbody = GetComponent<Rigidbody>();
            _input = GetComponent<IInput>();
            _speed = GameSettings.playerSpeed * (_playerController.IsBot ? 0.9f : 1f);
            _rotateLerpFactor = GameSettings.rotateLerpFactor;
            _animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            // _animator.SetBool(Run, _input.Direction.magnitude > 0.05f);
            // if (_input.Direction.magnitude != 0)
            // {
            //     var forward = _input.Direction;
            //     forward.y = 0;
            //     transform.forward =
            //         Vector3.Lerp(transform.forward, forward, Time.fixedDeltaTime * _rotateLerpFactor);
            // }
            //
            // var velocity = _rigidbody.velocity;
            // var horizontalSpeed = velocity.y;
            // velocity += _input.Direction * _speed;
            // velocity = Vector3.ClampMagnitude(velocity, 6f);
            // velocity.y = horizontalSpeed;
            // _rigidbody.velocity = velocity;
        }
    }
}