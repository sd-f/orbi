using CanvasUtility;
using ServerModel;
using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class AuthService: AbstractHttpService
    {
        public Settings settings;

        protected override bool IsReady()
        {
            return true;
        }

        public IEnumerator RequestAuthUser()
        {
            yield return Request("auth/user", null, OnAuthUser);
        }

        private void OnAuthUser(string data)
        {
            Game.Instance.GetPlayer().SetLoggedIn(true);
        }

        public IEnumerator RequestCode(String email)
        {
            RequestCodeInfo info = new RequestCodeInfo();
            info.email = email;
            info.player = Game.Instance.GetPlayer().GetModel();
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
            info.player = Game.Instance.GetPlayer().GetModel();

            yield return Request("auth/login", JsonUtility.ToJson(info), OnLoginSucceded, null);

        }

        public IEnumerator RequestLoginAndLoad(String email, String password)
        {
            LoginInfo info = new LoginInfo();
            info.email = email;
            info.password = password;
            info.player = Game.Instance.GetPlayer().GetModel();

            yield return Request("auth/login", JsonUtility.ToJson(info), OnLoginSucceded, true);

        }


        private void OnLoginSucceded(string data, object load)
        {
            AuthorizationInfo authInfo = JsonUtility.FromJson<AuthorizationInfo>(data);
            if (!String.IsNullOrEmpty(authInfo.token))
            {
                Game.Instance.GetSettings().SetToken(authInfo.token);
                Game.Instance.GetPlayer().SetLoggedIn(true);
            }
            if (load != null)
            {
                Game.Instance.LoadScene(Game.GameScene.GameScene);
            }
                
        }

        public IEnumerator LoadGameIfAuthorized()
        {
            yield return RequestAuthUser();
        }


    }
}
