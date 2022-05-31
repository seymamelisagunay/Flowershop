using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Script.Manager
{
    public class LoginManager : MonoBehaviour
    {
        public GameObject privacyPolicy;
        public NameInputPanel nameInputPanel;

        private void Start()
        {
            var isFirstLogin = UserManager.Instance.MatchCount == 0;
            if (isFirstLogin)
                privacyPolicy.SetActive(true);
            else
                SceneManager.LoadSceneAsync("Lobby");
        }

        public void ButtonAcceptTerms()
        {
            privacyPolicy.SetActive(false);
            nameInputPanel.Show(() => SceneManager.LoadSceneAsync("Lobby"));
        }

        public void ButtonShowTerms()
        {
            Application.OpenURL("https://www.gnarlygamestudio.com/assets/images/Terms%20Of%20Service.pdf");
            Application.OpenURL("https://www.gnarlygamestudio.com/assets/images/Privacy%20Policy.pdf");
        }
    }
}