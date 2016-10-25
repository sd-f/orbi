using CanvasUtility;
using ServerModel;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameController.Services
{

    public class AuthService: AbstractHttpService
    {

        public IEnumerator RequestAuthUser()
        {
            yield return Request("auth/user", null, OnAuthUser);
        }

        private void OnAuthUser(string data)
        {
            Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
            //Info.Show("Logged in successful");
            Game.GetPlayer().SetLoggedIn(true);
        }

        public IEnumerator RequestCode(String email)
        {
            RequestCodeInfo info = new RequestCodeInfo();
            info.email = email;
            info.player = Game.GetPlayer().GetModel();
            yield return Request("auth/requestcode", JsonUtility.ToJson(info), OnCodeRequests);
        }

        private void OnCodeRequests(string data)
        {
            Info.Show("Code has been sent - check your mail");
        }

        public IEnumerator RequestLogin(String email, String password)
        {
            LoginInfo info = new LoginInfo();
            info.email = email;
            info.password = password;
            info.player = Game.GetPlayer().GetModel();
            yield return Request("auth/login", JsonUtility.ToJson(info), OnLoginSucceded);

        }


        private void OnLoginSucceded(string data)
        {
            AuthorizationInfo authInfo = JsonUtility.FromJson<AuthorizationInfo>(data);
            if (!String.IsNullOrEmpty(authInfo.token))
            {
                Game.GetGame().GetSettings().SetToken(authInfo.token);
                Game.GetPlayer().SetLoggedIn(true);
            }
            StartCoroutine(BootLocationAndLoad());            
        }

        private IEnumerator BootLocationAndLoad()
        {
            yield return Game.GetLocation().Boot();
            Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
        }


        public IEnumerator LoadGameIfAuthorized()
        {
            yield return RequestAuthUser();
        }


    }
}
