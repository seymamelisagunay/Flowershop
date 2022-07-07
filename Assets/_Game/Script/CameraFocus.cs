using _Game.Script.Manager;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Script
{
    public class CameraFocus : MonoBehaviour
    {
        public bool isActive;
        public float getBackDuration;
        public float duration;
        public float zoomInOut;
        public float zoomInOutDuration;


        [Button()]
        public void Focus()
        {
            if (!isActive) return;
           
            var customCamera = GameManager.instance.customCamera;
            customCamera.StopFollow();
            var customTransform = customCamera.transform;
            var targetPosition = transform.position + customCamera.offset;
            targetPosition.y = customTransform.position.y;
            customCamera.transform.DOMove(targetPosition, duration).OnComplete(() =>
            {
                customCamera.GetComponent<Camera>().DOFieldOfView(zoomInOut, zoomInOutDuration)
                    .SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(getBackDuration, () => { customCamera.Follow(true, duration); });
                    });
            });
            // customCamera.Speed = followSpeed;
            // customCamera.SetTarget(transform);


            // customCamera.StopFollow();

            // Vector3 pointOnside = target.position + new Vector3 (target.localScale.x * 4.78f, 14f, target.localScale.z * -8);
            // float aspect = (float)Screen.width / (float)Screen.height;
            // float maxDistance = (target.localScale.y * 0.5f) / Mathf.Tan (Mathf.Deg2Rad * (Camera.main.fieldOfView / aspect)); 
            // Camera.main.transform.position = Vector3.MoveTowards (pointOnside, target.position, -maxDistance);
            // Camera.main.transform.LookAt (target.position);
        }
    }
}