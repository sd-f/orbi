using CanvasUtility;
using ServerModel;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController.Services
{

    public class AuthService: AbstractService
    {

        public IEnumerator RequestAuthUser()
        {
            WWW request = Request("auth/user", null);
            yield return request;
            if (request.error == null)
            {
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
                Info.Show("Logged in successful");
                // no errors
                IndicateRequestFinished();
            }
            else
                HandleError(request);
               
        }

        public IEnumerator RequestLogin()
        {
            WWW request = Request("auth/login", null);
            yield return request;
            if (request.error == null)
            {
                AuthorizationInfo authInfo = JsonUtility.FromJson<AuthorizationInfo>(request.text);
                if (authInfo.token != null && authInfo.token != "")
                {
                    Game.GetGame().GetSettings().SetToken(authInfo.token);
                }
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
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
