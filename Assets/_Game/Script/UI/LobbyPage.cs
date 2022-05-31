using TMPro;
using UnityEngine;

namespace _Game.Script.UI
{
    public class LobbyPage : MonoBehaviour
    {
        [SerializeField] private TMP_Text userNameText;
        [SerializeField] private TMP_Text cupCountText;
        [SerializeField] private TMP_Text levelText;

        public void Start()
        {
            var user = UserManager.Instance.UserModel;
            userNameText.SetText(user.name);
            cupCountText.SetText(user.cups.ToString());
            levelText.SetText(user.level.ToString());

            if (UserManager.Instance.MatchCount == 0) // First Login
            {
                FindObjectOfType<MatchmakingPage>().Show();
            }
        }

        public void ButtonDiscord()
        {
            Application.OpenURL("https://discord.gg/ZcemyfVhqf");
        }
    }
}