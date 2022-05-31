using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Script.Leaderboard
{
    public class LeaderboardPanel : MonoBehaviour
    {
        [SerializeField] private Transform contextPanel;
        [SerializeField] private LeaderboardItem selfItemPrefab;
        [SerializeField] private LeaderboardItem otherItemPrefab;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private Transform jumpObjectParent;
        [SerializeField] private RectTransform emptyObject;

        private List<LeaderboardItem> _items;
        private LeaderboardItem _selfItem;

        private void Awake()
        {
            _items = new List<LeaderboardItem>();
        }

        public async void Init(bool autoShow)
        {
            _items.ForEach(x => Destroy(x.gameObject));
            _items.Clear();
            var leaderboardManager = UserManager.Instance.LeaderboardManager;
            leaderboardManager.leaderboard
                .users.ForEach(user =>
                {
                    var item = Instantiate(user.id == "self" ? selfItemPrefab : otherItemPrefab
                        , contextPanel);
                    item.Init(user);
                    _items.Add(item);
                    if (user.id == "self")
                        _selfItem = item;
                });

            var user = leaderboardManager.selfUser;
            var oldRank = user.rank;
            //Update current cup for self users
            leaderboardManager.SetCurrentCup(UserManager.Instance.UserModel.cups);
            var currentRank = user.rank;
            var hasRankJump = oldRank > currentRank;
            if (autoShow && hasRankJump)
            {
                Show();
                canvasGroup.blocksRaycasts = false;
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
                Jump();
            }
            else
            {
                _selfItem.Init(user);
            }
        }

        public void Show()
        {
            if (_items.Count == 0)
                Init(false);

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            StartCoroutine(LookTarget(_selfItem.transform));
        }

        private IEnumerator LookTarget(Transform target, float duration = 0f)
        {
            yield return null;
            var startPosition = contextPanel.transform.position;
            var deltaYPosition = target.position.y - centerPoint.position.y;
            var targetPosition = startPosition - Vector3.up * deltaYPosition;
            var startTime = Time.time;

            if (duration == 0)
            {
                contextPanel.transform.position = targetPosition;
            }
            else
            {
                while (true)
                {
                    yield return null;
                    var deltaTime = Time.time - startTime;
                    contextPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, deltaTime / duration);
                    if (deltaTime > duration)
                    {
                        break;
                    }
                }
            }

            canvasGroup.blocksRaycasts = true;
        }

        public async void Jump()
        {
            _selfItem.UpdateView();
            LeaderboardItem targetRankItem = null;
            for (var i = 0; i < _items.Count; i++)
            {
                if (_selfItem.Data.cup > _items[i].Data.cup)
                {
                    targetRankItem = _items[i];
                    break;
                }
            }

            if (targetRankItem == null)
                return;

            emptyObject.transform.SetParent(contextPanel);
            emptyObject.transform.SetSiblingIndex(_selfItem.transform.GetSiblingIndex());
            _selfItem.transform.SetParent(jumpObjectParent);
            _selfItem.transform.DOScale(Vector3.one * 1.2f, 0.25f);
            StartCoroutine(SetEmptySpaceHeight(152, 0, 0.25f));
            await Task.Delay(TimeSpan.FromSeconds(0.25f));
            _selfItem.transform.DOMove(targetRankItem.transform.position, 0.5f);
            await Task.Delay(TimeSpan.FromSeconds(0.4f));
            emptyObject.transform.SetSiblingIndex(targetRankItem.transform.GetSiblingIndex());
            StartCoroutine(SetEmptySpaceHeight(0, 152, 0.25f));
            _selfItem.transform.DOScale(Vector3.one, 0.25f);
            await Task.Delay(TimeSpan.FromSeconds(0.25f));
            _selfItem.transform.SetParent(contextPanel);
            _selfItem.transform.SetSiblingIndex(emptyObject.transform.GetSiblingIndex());
            emptyObject.transform.SetParent(jumpObjectParent);
            StartCoroutine(LookTarget(_selfItem.transform, 0.5f));
            _items.ForEach(x => x.UpdateView());
        }

        private IEnumerator SetEmptySpaceHeight(float startValue, float finalValue, float duration = 1)
        {
            emptyObject.sizeDelta = new Vector2(0, startValue);
            var startTime = Time.time;
            while (true)
            {
                yield return null;
                var deltaTime = Time.time - startTime;
                emptyObject.sizeDelta = new Vector2(0, Mathf.Lerp(startValue, finalValue, deltaTime / duration));
                LayoutRebuilder.ForceRebuildLayoutImmediate(emptyObject.parent as RectTransform);
                if (deltaTime / duration > 1)
                    break;
            }
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}