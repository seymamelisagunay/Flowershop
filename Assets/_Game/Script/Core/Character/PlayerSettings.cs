using UnityEngine;
using NaughtyAttributes;

namespace _Game.Script.Core.Character
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Gnarly Team/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        public bool isBot = false;
        [HideIf("isBot")]
        [InfoBox("The field gets in code name speed !")]
        [SerializeField]
        private float playerSpeed = 2.5f;
        [ShowIf("isBot")]
        [InfoBox("The field gets in code name speed !")]
        [SerializeField]
        private float botSpeed;
        public float speed => !isBot ? playerSpeed : botSpeed;
        public float rotateLerpFactor = 10;
    }
}
