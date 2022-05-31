using System.Collections.Generic;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using _Game.Systems.Scoreboard1v1.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game.Script
{
    public class TestMatch : MonoBehaviour
    {
        [SerializeField] private Transform blueButtonTransform;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private List<Transform> spawnPoint;

        private void Start()
        {
            DOVirtual.DelayedCall(2f,
                () => Match.Instance.Begin());
            FindObjectOfType<ScoreboardManager>().OnTimeUp += TimeOut;

            var data = Match.Instance.Data;
            for (var i = 0; i < data.players.Count; i++)
            {
                var playerData = data.players[i];
                var player = Instantiate(playerPrefab, transform);
                player.Init(playerData, spawnPoint[i]);
            }

            Match.Instance.OnBegin += OnMatchBegin;
        }

        private void OnMatchBegin()
        {
        }

        private void TimeOut()
        {
            MatchEnd();
        }

        public void ButtonBlueScoreIncrease()
        {
            FindObjectOfType<ScoreboardManager>().IncrementScore(Team.Blue, 10, blueButtonTransform.position);
        }

        public void ButtonRedScoreIncrease()
        {
            FindObjectOfType<ScoreboardManager>().IncrementScore(Team.Red, 10, blueButtonTransform.position);
        }

        public void ButtonEndMatch()
        {
            MatchEnd();
        }

        private void MatchEnd()
        {
            var result = FindObjectOfType<ScoreboardManager>().GetResult();
            Match.Instance.End(result);
        }
    }
}