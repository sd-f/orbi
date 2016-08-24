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
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            
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
           
            return www;
        }

        public void IndicateRequestStart()
        {
            Server.RUNNING_REQUESTS++;
        }

        public void IndicateRequestFinished()
        {
            if (Server.RUNNING_REQUESTS > 0)
                Server.RUNNING_REQUESTS--;
        }
    }
}
