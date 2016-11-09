using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GameController;
using UnityEngine.UI;
using CanvasUtility;
using System;
using System.Net.Mail;
using GameController.Services;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Menu")]
    public class Menu : MonoBehaviour
    {
        public AuthService authService;

        private InputField emailField;
        private InputField codeField;
        private Button loginButton;


        void Start()
        {
            emailField = GameObject.Find("InputFieldEmail").GetComponent<InputField>();
            emailField.text = Game.Instance.GetSettings().GetEmail();
            codeField = GameObject.Find("InputFieldCode").GetComponent<InputField>();
            loginButton = GameObject.Find("ButtonLogin").GetComponent<Button>();
            if (!Game.Instance.GetPlayer().IsLoggedIn())
                SetFormEnabled(true);
            else
                SetFormEnabled(false);
        }

        public void OnLogin()
        {
            
            if (!IsValidEmail(emailField.text))
            {
                Error.Show("Email not valid");
                SetFormEnabled(false);
                return;
            }

            Info.Show("Logging in...");
            Game.Instance.GetSettings().SetEmail(emailField.text);
            StartCoroutine(Login());
            
        }

        bool IsValidEmail(string emailAddress)
        {
            bool MethodResult = false;
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                MethodResult = m.Address == emailAddress;
            }
            catch //(Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }

        public void OnRequestCode()
        {
            if (!IsValidEmail(emailField.text))
            {
                Error.Show("Email not valid");
                SetFormEnabled(true);
                return;
            }

            Game.Instance.GetSettings().SetEmail(emailField.text);
            
            StartCoroutine(RequestCode());
        }

        private IEnumerator RequestCode()
        {
            yield return Game.Instance.GetAuthService().RequestCode(emailField.text);
            SetFormEnabled(true);
           // Info.Show("Code requested...");
        }

        private IEnumerator Login()
        {
            yield return Game.Instance.GetAuthService().RequestLoginAndLoad(emailField.text, codeField.text);
            //yield return Game.Instance.GetPlayer().GetPlayerService().RequestPlayer();
            SetFormEnabled(true);
        }

        public void SetFormEnabled(Boolean enabled)
        {
            emailField.interactable = enabled;
            codeField.interactable = enabled;
            codeField.interactable = enabled;
            loginButton.interactable = enabled;
        }

    }
}
