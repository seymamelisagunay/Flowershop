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
        public PlayerControllerSettings playerSettings;
        public override void OnValidate()
        {
            base.OnValidate();

            speed = playerSettings.speed;

        }
    }
}