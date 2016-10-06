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
            WWW request = Request("auth/user", null);
            yield return request;
            if (request.error == null)
            {
                Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
                //Info.Show("Logged in successful");
                Game.GetPlayer().SetLoggedIn(true);
                // no errors
                IndicateRequestFinished();
            }
            else
                HandleError(request);
               
        }

        public IEnumerator RequestCode(String email)
        {
            RequestCodeInfo info = new RequestCodeInfo();
            info.email = email;
            info.player = Game.GetPlayer().GetModel();
            WWW request = Request("auth/requestcode", JsonUtility.ToJson(info));
            yield return request;
            if (request.error == null)
            {
                Info.Show("Code has been sent - check your mail");
                // no errors
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestLogin(String email, String password)
        {
            LoginInfo info = new LoginInfo();
            info.email = email;
            info.password = password;
            info.player = Game.GetPlayer().GetModel();
            WWW request = Request("auth/login", JsonUtility.ToJson(info));
            yield return request;
            if (request.error == null)
            {
                AuthorizationInfo authInfo = JsonUtility.FromJson<AuthorizationInfo>(request.text);
                if (authInfo.token != null && authInfo.token != "")
                {
                    Game.GetGame().GetSettings().SetToken(authInfo.token);
                    Game.GetPlayer().SetLoggedIn(true);
                }
                Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
                Info.Show("Login successful");
                // no errors
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }


        public IEnumerator LoadGameIfAuthorized()
        {
            yield return RequestAuthUser();
        }


    }
}
