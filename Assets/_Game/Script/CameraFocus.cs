using System;
using System.Collections;
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


        public void Focus(float wait)
        {
            StartCoroutine(FocusWait(wait));
        }

        private IEnumerator FocusWait(float wait)
        {
            yield return new WaitForSeconds(wait);
            if (!isActive) yield break;

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
        }
    }
}