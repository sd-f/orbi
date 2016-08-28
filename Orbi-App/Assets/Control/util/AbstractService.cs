using Assets.Model;
using CanvasUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Control
{
    abstract class AbstractService
    {

        public WWW Request(string apiPath, string jsonString)
        {
            IndicateRequestStart();

            string uri = Game.GetInstance().GetServerUrl() + "/" + apiPath;
            UTF8Encoding encoding = new UTF8Encoding();
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

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
            //Debug.Log(uri);
            return www;
        }

        public void HandleError(WWW request)
        {
            IndicateRequestFinished();
            ErrorMessage message = JsonUtility.FromJson<ErrorMessage>(request.text);
            if (message != null)
                Error.Show(message.status + ": " + message.message);
            else
                Error.Show(request.error);
           
            
        }

        public void IndicateRequestStart()
        {
            Assets.Control.util.Server.RUNNING_REQUESTS++;
        }

        public void IndicateRequestFinished()
        {
            if (Assets.Control.util.Server.RUNNING_REQUESTS > 0)
                Assets.Control.util.Server.RUNNING_REQUESTS--;
        }
    }
}
