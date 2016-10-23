using CanvasUtility;
using ServerModel;
using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class ServerService: AbstractHttpService
    {

        public IEnumerator RequestInfo()
        {
            WWW request = Request("server/info", null);
            yield return request;
            if (request.error == null)
            {
                ServerInfo info = JsonUtility.FromJson<ServerInfo>(request.text);
                Game.GetClient().SetServerInfo(info);
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
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
                if (message.status == 505)
                {
                    yield return new WaitForSeconds(3);
                    Application.OpenURL("https://softwaredesign.foundation/orbi/");
                    Application.Quit();
                }

                if (message != null)
                    Error.Show(message.message);
                else
                    Error.Show(request.error);
            }
               
        }

    }
}
