using UnityEngine;
using NaughtyAttributes;
namespace _Game.Script.Core.Character
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Gnarly Team/Player Settings")]
    public class PlayerControllerSettings : ScriptableObject
    {

        public bool isOwner;
        public bool IsBot = false;
        [ShowIf("isOwner")]
        [InfoBox("The field gets in code name speed !")]
        [SerializeField]
        private float playerSpeed = 2.5f;
        [ShowIf("IsBot")]
        [InfoBox("The field gets in code name speed !")]
        [SerializeField]
        private float botSpeed;
        public float speed => IsBot ? playerSpeed : botSpeed;
        public float rotateLerpFactor = 10;
    }
}
