using System;
using TMPro;
using UnityEngine;

namespace _Game.Script.Manager
{
    public class NameInputPanel : MonoBehaviour
    {
        public TMP_InputField nameInputField;
        private Action _onSubmit;

        public void Show(Action onSubmit)
        {
            _onSubmit = onSubmit;
            gameObject.SetActive(true);
            nameInputField.text = UserManager.Instance.UserModel.name;
        }

        public void ButtonSubmitName()
        {
            UserManager.Instance.UserModel.name = nameInputField.text;
            UserManager.Instance.SaveUser();
            nameInputField.gameObject.SetActive(false);
            _onSubmit?.Invoke();
        }
    }
}