using System;
using System.Collections.Generic;
using _Game.Script;
using _Game.Script.Leaderboard;
using _Game.Script.Manager;
using _Game.Script.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.MatchResult.Scripts
{
    public enum MatchResultType
    {
        Draw,
        Win,
        Lose
    }

    public class MatchRewardPanel : MonoBehaviour
    {
        private readonly Dictionary<MatchResultType, int> _rewardCups = new Dictionary<MatchResultType, int>
        {
            {MatchResultType.Win, 10}, // Win cup
            {MatchResultType.Lose, 2}, // Lose cup
            {MatchResultType.Draw, 5} // Draw Cup
        };

        private readonly Dictionary<MatchResultType, int> _rewardExp = new Dictionary<MatchResultType, int>
        {
            {MatchResultType.Win, 50}, // Win
            {MatchResultType.Lose, 10}, // Lose
            {MatchResultType.Draw, 25} // Draw
        };

        [SerializeField] private GameObject winTitle;
        [SerializeField] private GameObject loseTitle;
        [SerializeField] private GameObject drawTitle;
        [SerializeField] private TMP_Text currentCupCountText;
        private CanvasGroup _canvasGroup;
        public Slider expBar;
        public TMP_Text levelText;
        public TMP_Text expPercentText;

        public void Show(MatchResultType result)
        {
            // _canvasGroup = GetComponent<CanvasGroup>();
            // _canvasGroup.blocksRaycasts = true;
            // _canvasGroup.DOFade(1, 0.2f);

            // var user = UserManager.Instance.UserModel;
            // var oldExp = user.exp;
            // var oldLevel = user.level;
            // var oldCup = user.cups;
            // UserManager.Instance.IncrementExp(_rewardExp[result]);
            // UserManager.Instance.IncrementCup(_rewardCups[result]);
            // var nextCup = user.cups;
            // var nextExp = user.exp;
            // var nextLevel = user.level;

            // winTitle.SetActive(result == MatchResultType.Win);
            // loseTitle.SetActive(result == MatchResultType.Lose);
            // drawTitle.SetActive(result == MatchResultType.Draw);
            // currentCupCountText.SetText((nextCup - oldCup).ToString());
            // ShowExpProgress(oldLevel, oldExp, nextLevel, nextExp);
            // TinySauce.OnGameFinished(nextCup - oldCup);
        }

        private void ShowExpProgress(int oldLevel, int oldExp, int nextLevel, int nextExp)
        {
            levelText.SetText(oldLevel.ToString());
            expBar.value = oldExp / (oldLevel * 100f);
            expPercentText.SetText($"{oldExp}/ {oldLevel * 100}");

            if (oldLevel == nextLevel) // No level up
            {
                DOVirtual.Int(oldExp, nextExp, 1f, value =>
                {
                    var expValue = value;
                    expPercentText.SetText($"{expValue}/ {oldLevel * 100}");
                    expBar.value = expValue / (oldLevel * 100f);
                });
            }
            else
            {
                var sequence = DOTween.Sequence();
                var levelUpTween = DOVirtual.Int(oldExp, oldLevel * 100, 0.5f,
                    value =>
                    {
                        expPercentText.SetText($"{value}/ {oldLevel * 100}");
                        expBar.value = value / (oldLevel * 100f);
                    });
                var setCurrentExpTween = DOVirtual.Int(0, nextExp, 0.5f,
                    value =>
                    {
                        var expValue = (int) value;
                        expPercentText.SetText($"{expValue}/ {nextLevel * 100}");
                        expBar.value = expValue / (nextLevel * 100f);
                    });

                sequence.Append(levelUpTween);
                sequence.AppendInterval(0.1f);
                sequence.AppendCallback(() => levelText.SetText(nextLevel.ToString()));
                sequence.Append(levelText.transform.DOScale(Vector3.one * 2f, 0.2f));
                sequence.Append(levelText.transform.DOScale(Vector3.one, 0.3f));
                sequence.AppendInterval(0.25f);
                sequence.Append(setCurrentExpTween);
            }
        }

        public void Hide()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0, 0.2f);
        }

        public void ButtonAgain()
        {
            Hide();
            FindObjectOfType<MatchmakingPage>().Show();
        }

        public void ButtonBack()
        {
            Hide();
            FindObjectOfType<LeaderboardPanel>().Init(true);
        }
    }
}