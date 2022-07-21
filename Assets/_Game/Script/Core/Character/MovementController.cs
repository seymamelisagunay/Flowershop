using _Game.Script.Bot;
using _Game.Script.Character;
using _Game.Script.Manager;
using ECM.Controllers;
using UnityEngine;

namespace _Game.Script.Core.Character
{
    public class MovementController : BaseCharacterController
    {
        [Space]
        [Header("Player Settings")]
        public PlayerSettings playerSettings;
        private IInput _input;
        public bool inMotion;
        public CustomCameraFollow cameraFollow;

        public override void Awake()
        {
            base.Awake();
            _input = GetComponent<IInput>();
        }

        protected override void Move()
        {
            speed = playerSettings.speed;
            base.Move();
        }
        protected override void HandleInput()
        {
            if (!playerSettings.isBot)
            {
                if (!cameraFollow.isFollow)
                {
                    moveDirection = Vector3.zero;
                    return;
                }
            }
            var isRun = _input.Direction.magnitude > 0.1;
            inMotion = isRun;
            animator.SetBool("run", isRun);
            moveDirection = _input.Direction;
        }
    }
}