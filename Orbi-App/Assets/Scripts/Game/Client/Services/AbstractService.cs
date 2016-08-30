using CanvasUtility;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameController.Services
{
    public abstract class AbstractService
    {

        public WWW Request(string apiPath, string jsonString)
        {
            IndicateRequestStart();

            string uri = ServerConstants.GetServerUrl(Game.GetClient().serverType) + "/" + apiPath;
            UTF8Encoding encoding = new UTF8Encoding();
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            headers.Add("X-App-Version", Client.VERSION.ToString());
            headers.Add("Authorization", "Bearer " + Game.GetGame().GetSettings().GetToken());
            WWW www;
            if (jsonString != null)
            {
                headers.Add("Content-Type", "application/json");
                headers.Add("Content-Length", encoding.GetByteCount(jsonString).ToString());
                www = new WWW(uri, encoding.GetBytes(jsonString), headers);
            } else
            {
                www = new WWW(uri, null, headers);
            }
            return www;
        }

        public void HandleError(WWW request)
        {
            IndicateRequestFinished();
            ErrorMessage message = null;
            try
            {
                message = JsonUtility.FromJson<ErrorMessage>(request.text);
                if (message.status == 401)
                {
                    if (SceneManager.GetActiveScene().name != "AuthorizationScene")
                    {
                        SceneManager.LoadScene("AuthorizationScene");
                    }
                    Game.GetGame().GetSettings().SetToken(null);
                }
            } catch (Exception ex)
            {
                Debug.Log(request.text);
                Debug.LogError(ex);
            }
            
            if (message != null)
                Error.Show(message.message);
            else
                Error.Show(request.error);
           
            
        }

        public void IndicateRequestStart()
        {
            Game.GetClient().IncRunningRequests();
        }

        public void IndicateRequestFinished()
        {
            Game.GetClient().DecRunningRequests();
        }
    }
}
