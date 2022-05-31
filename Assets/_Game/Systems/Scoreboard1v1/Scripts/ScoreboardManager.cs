using System;
using System.Collections;
using _Game.Script.Manager;
using _Game.Systems.MatchResult.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Systems.Scoreboard1v1.Scripts
{
    public class ScoreBoardUserData
    {
        public string ID;
        public string Name;
        public int Score;
        public bool IsMine;
    }

    public enum Team
    {
        Blue,
        Red
    }

    public class ScoreboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text remainingTimeText;
        [SerializeField] private ScoreBoardUser blueUser;
        [SerializeField] private ScoreBoardUser redUser;
        private CanvasGroup _canvasGroup;
        private int _remainingTime;
        private Coroutine _timerProgress;
        public Action OnTimeUp { get; set; }


        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _remainingTime = 60;
            remainingTimeText.SetText($"{_remainingTime}s");
            Match.Instance.OnEnd += OnEndMatch;
            Match.Instance.OnBegin += OnBeginMatch;
            Show();
        }

        private void Show()
        {
            var players = Match.Instance.Data.players;
            var bluePlayerData = players[0];
            var redPlayerData = players[1];
            Init(new ScoreBoardUserData()
            {
                Name = bluePlayerData.Name,
                Score = 0,
                ID = bluePlayerData.UserID,
                IsMine = true
            }, new ScoreBoardUserData()
            {
                Name = redPlayerData.Name,
                Score = 0,
                ID = redPlayerData.UserID,
                IsMine = false
            });

            _canvasGroup.DOFade(1, 0.5f);
        }

        private void OnBeginMatch()
        {
            StartTimer();
        }

        private void OnEndMatch()
        {
            StopCoroutine(_timerProgress);
        }

        private void Init(ScoreBoardUserData blue, ScoreBoardUserData red)
        {
            blueUser.Init(blue);
            redUser.Init(red);
        }

        public void IncrementScore(Team team, int score, Vector3 hitPosition)
        {
            if (team == Team.Blue)
                blueUser.IncrementScore(score, hitPosition);
            else
                redUser.IncrementScore(score, hitPosition);
        }

        private void StartTimer()
        {
            _timerProgress = StartCoroutine(TimerProgress());
        }

        private IEnumerator TimerProgress()
        {
            _remainingTime = Match.Instance.Settings.MatchTime;
            while (true)
            {
                yield return new WaitForSeconds(1);
                _remainingTime--;
                remainingTimeText.SetText($"{_remainingTime}s");

                if (_remainingTime == 0)
                {
                    OnTimeUp?.Invoke();
                    yield break;
                }
            }
        }

        public MatchResultType GetResult()
        {
            if (blueUser.Data.Score > redUser.Data.Score)
                return MatchResultType.Win;

            if (blueUser.Data.Score < redUser.Data.Score)
                return MatchResultType.Lose;

            return MatchResultType.Draw;
        }
    }
}