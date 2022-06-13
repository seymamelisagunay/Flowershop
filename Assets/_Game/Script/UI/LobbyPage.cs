using TMPro;
using UnityEngine;

namespace _Game.Script.UI
{
    public class LobbyPage : MonoBehaviour
    {
        public TMP_Text moneyText;
        public IntVariable moneyVariable;
        public void Awake()
        {
            moneyVariable.OnChangeVariable.AddListener(OnChangeVariable);
        }
        private void OnChangeVariable()
        {
            moneyText.SetText(moneyVariable.Value.ToString());
        }
    }
}