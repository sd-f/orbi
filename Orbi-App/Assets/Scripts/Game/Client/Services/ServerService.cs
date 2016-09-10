using CanvasUtility;
using ServerModel;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController.Services
{

    public class ServerService: AbstractService
    {

        public IEnumerator RequestVersion()
        {
            WWW request = Request("server/version", JsonUtility.ToJson(Game.GetPlayer().GetModel()));
            yield return request;
            if (request.error == null)
            {
                //Info.Show("Application is up-to-date");
                // no errors
                IndicateRequestFinished();
            }
            else
            {
                IndicateRequestFinished();
                ErrorMessage message = null;
                try
                {
                    message = JsonUtility.FromJson<ErrorMessage>(request.text);
                    Debug.Log(message.status);
                    if (message.status == 505)
                    {
                        Application.OpenURL("https://softwaredesign.foundation/orbi/");
                        Application.Quit();
                    }
                    
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }

                if (message != null)
                    Error.Show(message.message);
                else
                    Error.Show(request.error);
            }
               
        }

    }
}
