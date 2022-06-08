using _Game.Script.Character;
using UnityEngine;

namespace _Game.Script
{
    public class CustomCameraFollow : MonoBehaviour
    {
        public float Speed;
        public Vector3 offset;
        public bool isSetYReset;
        private PlayerController _target;

        public void SetTarget(PlayerController player)
        {
            _target = player;
        }

        private void LateUpdate()
        {
            if (_target == null) return;
            var targetPos = _target.transform.position + offset;
            targetPos.y = isSetYReset ? 0 : targetPos.y;
            transform.position = Vector3.Lerp(transform.position, targetPos,
                Time.deltaTime * Speed);
        }
    }
}