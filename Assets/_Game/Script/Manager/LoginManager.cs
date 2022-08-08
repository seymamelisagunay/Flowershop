using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Script.Manager
{
    public class LoginManager : MonoBehaviour
    {
        public GameObject privacyPolicy;
        public string nextSceneName;

        private void Start()
        {
            var isFirstLogin = PlayerPrefs.HasKey("FirstLogin");
            if (!isFirstLogin)
            {
                privacyPolicy.SetActive(true);
                PlayerPrefs.SetInt("FirstLogin", 1);
            }
            else
                OpenScene();
        }

        public void ButtonAcceptTerms()
        {
            privacyPolicy.SetActive(false);
            OpenScene();
        }
        private void OpenScene()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
            asyncOperation.completed -= OpennedGame;
            asyncOperation.completed += OpennedGame;
        }
        private void OpennedGame(AsyncOperation asyncValue)
        {
            Debug.Log("Test Deniyeceğiz !");
            var gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.Init();
            }
        }

        public void ButtonShowTerms()
        {
            Application.OpenURL("https://gnarlyteam.com/assets/img/company/terms_of_service.pdf");
            Application.OpenURL("https://gnarlyteam.com/assets/img/company/privacy_policy.pdf");
        }
    }
}