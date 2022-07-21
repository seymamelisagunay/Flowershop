using System;
using _Game.Script.Character;
using DG.Tweening;
using UnityEngine;

namespace _Game.Script
{
    public class CustomCameraFollow : MonoBehaviour
    {
        public float speed;
        public Vector3 offset;
        public bool isSetYReset;
        public Transform target;
        public bool isFollow;

        private void Start()
        {
            Follow(false, 0);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void Follow(bool isSlowGetBack, float duration)
        {
            if (isSlowGetBack)
            {
                var targetPos = target.transform.position + offset;
                transform.DOMove(targetPos, duration).OnComplete(() => { isFollow = true; });
            }
            else
            {
                isFollow = true;
            }
        }

        public void StopFollow()
        {
            isFollow = false;
        }

        private void LateUpdate()
        {
            if (!isFollow) return;
            if (target == null) return;
            var targetPos = target.transform.position + offset;
            targetPos.y = isSetYReset ? 0 : targetPos.y;
            transform.position = Vector3.Lerp(transform.position, targetPos,
                Time.deltaTime * speed);
        }
    }
}