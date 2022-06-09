using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace _Game.Script
{
    public class MatchmakingPage : MonoBehaviour
    {
        public TMPro.TMP_Text playerNameText;
        public TMPro.TMP_Text enemyNameText;
        public TMPro.TMP_Text playerCupText;
        public TMPro.TMP_Text enemyCupText;
        private int _enemyCups;
        public TextAsset userNamePool;
        private List<string> _enemyNames;
        private CanvasGroup _canvasGroup;
        private int _playerCups;

        public void Show()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1, 0.2f);
            _enemyNames = userNamePool.text.Split('\n').ToList();
            gameObject.SetActive(true);
            // _playerCups = UserManager.Instance.UserModel.cups;
            playerNameText.text = UserManager.Instance.UserModel.name;
            playerCupText.text = _playerCups.ToString();
            StartCoroutine(MatchmakingProgress());
        }

        private IEnumerator MatchmakingProgress()
        {
            var randomEnemyIndex = Random.Range(20, 50);
            var enemyName = "none";
            for (var i = 0; i < randomEnemyIndex; i++)
            {
                var nameIndex = Random.Range(0, _enemyNames.Count);
                enemyName = _enemyNames[nameIndex];
                enemyNameText.text = enemyName;
                _enemyCups = Random.Range(_playerCups < 15 ? 0 : _playerCups - 15, _playerCups + 15);
                enemyCupText.text = _enemyCups.ToString();
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1f);

            CreateMatch(enemyName);
        }

        private void CreateMatch(string enemyName)
        {
            // var matchData = new MatchData()
            // {
            //     players = new List<MatchData.PlayerData>()
            //     {
            //         new MatchData.PlayerData()
            //         {
            //             UserID = UserManager.Instance.UserModel.id,
            //             Name = UserManager.Instance.UserModel.name
            //         },
            //         new MatchData.PlayerData()
            //         {
            //             UserID = "Bot",
            //             Name = enemyName
            //         },
            //     }
            // };
            //
            // var match = new GameObject("MatchController").AddComponent<Match>();
            // match.Init(matchData);
        }
    }
}