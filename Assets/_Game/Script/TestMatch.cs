using System.Collections.Generic;
using _Game.Script.Character;
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
            // FindObjectOfType<ScoreboardManager>().IncrementScore(Team.Blue, 10, blueButtonTransform.position);
        }

        public void ButtonRedScoreIncrease()
        {
            // FindObjectOfType<ScoreboardManager>().IncrementScore(Team.Red, 10, blueButtonTransform.position);
        }

        public void ButtonEndMatch()
        {
            MatchEnd();
        }

        private void MatchEnd()
        {
            // var result = FindObjectOfType<ScoreboardManager>().GetResult();
            // Match.Instance.End(result);
        }
    }
}