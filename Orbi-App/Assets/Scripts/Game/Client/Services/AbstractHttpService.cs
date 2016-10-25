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
    public abstract class AbstractHttpService : MonoBehaviour
    {
        private static int numberOfUnknownErrors = 0;
        private static int MAX_REQUESTS = 3;
        private static int REQUEST_QUEUE_WAIT_TIME = 1;
        private static int REQUEST_QUEUE_MAX_WAITS = 3;
        private int waited = 0;

        System.Collections.IEnumerator CheckRequestQueue()
        {
            waited++;
            if (Game.GetClient().RunningRequests() > MAX_REQUESTS)
            {
                yield return new WaitForSeconds(REQUEST_QUEUE_WAIT_TIME);
            } else
            {
                if (waited <= REQUEST_QUEUE_MAX_WAITS)
                    yield return CheckRequestQueue();
            }
        }

        public System.Collections.IEnumerator Request(string apiPath, string jsonString, Action<string,object> onSuccess, object callbackLoad)
        {
            yield return CheckRequestQueue();
            IndicateRequestStart();
            WWW request = Request(apiPath, jsonString);
            yield return request;
            IndicateRequestFinished();
            if (request.error == null)
                onSuccess.DynamicInvoke(request.text, callbackLoad);
            else
                HandleError(request);
        }

        public System.Collections.IEnumerator Request(string apiPath, string jsonString, Action<string> onSuccess)
        {
            yield return CheckRequestQueue();
            IndicateRequestStart();
            WWW request = Request(apiPath, jsonString);
            yield return request;
            IndicateRequestFinished();
            if (request.error == null)
                    onSuccess.Invoke(request.text);
            else
                HandleError(request);
        }

        public WWW Request(string apiPath, string jsonString)
        {
            //Debug.Log(apiPath + "\n" + jsonString);
            string uri = ServerConstants.GetServerUrl(Game.GetClient().serverType) + "/" + apiPath;
            UTF8Encoding encoding = new UTF8Encoding();
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            headers.Add("X-App-Version", Client.VERSION.ToString());
            headers.Add("Authorization", "Bearer " + Game.GetGame().GetSettings().GetToken());
            //Debug.Log(apiPath + " token: " + Game.GetGame().GetSettings().GetToken());
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
                    if (SceneManager.GetActiveScene().name != Game.GameScene.AuthorizationScene.ToString())
                    {
                        Game.GetGame().LoadScene(Game.GameScene.AuthorizationScene);
                    }
                    Game.GetGame().GetSettings().SetToken(null);
                    Game.GetPlayer().SetLoggedIn(false);
                }
            } catch (Exception ex)
            {
                Debug.Log(request.text);
                Debug.LogError(ex);
            }
            HandleErrorCode(request.error, message);
        }

        private void HandleErrorCode(string error, ErrorMessage message)
        {
            if (message != null)
                CheckErrorCode((int)message.status, message.message, true);
            else
            {
                if (error.Contains("500"))
                    CheckErrorCode(500, "Server is doing weird stuff", false);
                else if (error.Contains("502"))
                    CheckErrorCode(502, "Server is sending weird stuff", false);
                else if (error.Contains("503"))
                    CheckErrorCode(503, "No connection or server is down", false);
                else if (error.Contains("504"))
                    CheckErrorCode(504, "Connection too slow", false);
                else if (error.Contains("400"))
                    CheckErrorCode(400, "App is sending weird stuff", false);
                else
                    CheckErrorCode(500, "Sorry something went wrong", false);
            }
        }

        private void CheckErrorCode(int code, string message, bool custommessage)
        {
            Debug.LogError("Orbi-Error: " + code + ": " + message);
            // silent numbers unknown errors
            if ((code == 502) || (code == 503) || (code == 504))
            {
                return;
            }
            if (!custommessage)
            {
                numberOfUnknownErrors++;
                if (numberOfUnknownErrors > 5)
                {
                    numberOfUnknownErrors = 0;
                    Error.Show(message);
                }
                        
            } else
            {
                // silent known numbers
                if ((code == 401))
                    return;
                if ((code == 505))
                {
                    StartCoroutine(RedirectToUpdateSite());
                    return;
                }
                Error.Show(message);
            }
        }

        System.Collections.IEnumerator RedirectToUpdateSite()
        {
            yield return new WaitForSeconds(3);
            Application.OpenURL("https://softwaredesign.foundation/orbi/");
            Application.Quit();
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
