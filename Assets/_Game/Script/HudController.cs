using _Game.Script.Character;
using TMPro;
using UnityEngine;

namespace _Game.Script
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        private PlayerController _player;
        private CanvasGroup _canvasGroup;
        private Camera _camera;
        private bool _fixed;
        private int _fixAlpha;
        private float _defaultFontSize;

        public void Init(PlayerController playerController)
        {
            _player = playerController;
            _canvasGroup = GetComponent<CanvasGroup>();

            // if (!_player.IsOwner)
            //     nameText.SetText(_player.Data.Name);

            transform.SetParent(null);
            _defaultFontSize = nameText.fontSizeMax;
        }

        private void OnCreateLevel()
        {
            SetCamera(Camera.main);
            _fixed = false;
            nameText.fontSizeMax = _defaultFontSize;
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
            transform.eulerAngles = new Vector3(_camera.transform.eulerAngles.x, 0f, 0f);
        }

        private void OnEliminated()
        {
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            if (_camera == null) return;
            
            if (_fixed)
            {
                _canvasGroup.alpha = _fixAlpha;
            }
            else
            {
                var distance = Vector3.Distance(_camera.transform.position, transform.position);
                _canvasGroup.alpha = Mathf.InverseLerp(50, 10, distance) - 0.2f;
            }

            transform.position = _player.hudPoint.position;
        }
    }
}