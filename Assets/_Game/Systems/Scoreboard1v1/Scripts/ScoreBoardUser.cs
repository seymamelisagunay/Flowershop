using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Systems.Scoreboard1v1.Scripts
{
    public class ScoreBoardUser : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text playerScoreText;
        public ScoreBoardUserData Data;

        public void Init(ScoreBoardUserData data)
        {
            Data = data;
            playerNameText.SetText(Data.Name);
            playerScoreText.SetText(Data.Score.ToString());
        }

        public void IncrementScore(int score, Vector3 hitPosition)
        {
            var oldValue = Data.Score;
            Data.Score += score;

            if (Data.IsMine)
            {
                var hitPoint = Instantiate(Resources.Load<TMP_Text>("ScoreHitPoint"), transform);
                hitPoint.gameObject.SetActive(true);
                hitPoint.SetText($"+{score}");
                hitPoint.transform.position = hitPosition;
                hitPoint.transform.DOScale(Vector3.one * 0.3f, 1.5f).SetEase(Ease.InCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
                hitPoint.DOColor(new Color(1, 1, 1, 0), 1.5f).SetEase(Ease.InCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
                hitPoint.transform.DOMove(playerScoreText.transform.position, 1.5f).SetEase(Ease.InCirc).OnComplete(
                    () =>
                    {
                        DOVirtual.Int(oldValue, Data.Score, 0.5f,
                                (value) => { playerScoreText.SetText(value.ToString()); })
                            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
                        Destroy(hitPoint.gameObject, 0.1f);
                    }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            else
            {
                DOVirtual.Int(oldValue, Data.Score, 0.5f,
                        (value) => { playerScoreText.SetText(value.ToString()); })
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }
    }
}