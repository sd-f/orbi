﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GameController;
using UnityEngine.UI;
using CanvasUtility;
using System;

namespace StartScene
{
    [AddComponentMenu("App/Scenes/Authorization/Menu")]
    public class Menu : MonoBehaviour
    {
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
            Info.Show("Logging in...");
            SetFormEnabled(false);
            Game.Instance.GetSettings().SetEmail(emailField.text);
            StartCoroutine(Login());
        }

        public void OnRequestCode()
        {
            Info.Show("Code requested...");
            SetFormEnabled(false);
            Game.Instance.GetSettings().SetEmail(emailField.text);
            StartCoroutine(RequestCode());
        }

        private IEnumerator RequestCode()
        {
            yield return Game.Instance.GetAuthService().RequestCode(emailField.text);
            SetFormEnabled(true);
        }

        private IEnumerator Login()
        {
            yield return Game.Instance.GetAuthService().RequestLogin(emailField.text, codeField.text);
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestPlayer();
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
