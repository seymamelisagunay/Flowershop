using TMPro;
using UnityEngine;

namespace _Game.Script.UI
{
    public class LobbyPage : MonoBehaviour
    {
        [SerializeField] private TMP_Text userNameText;
        [SerializeField] private TMP_Text levelText;

        public void Start()
        {
            var user = UserManager.Instance.UserModel;
            userNameText.SetText(user.name);
            levelText.SetText(user.level.ToString());
        }

        public void ButtonDiscord()
        {
            Application.OpenURL("https://discord.gg/ZcemyfVhqf");
        }
    }
}