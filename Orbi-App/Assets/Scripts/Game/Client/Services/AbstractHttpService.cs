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
        public static int MAX_REQUESTS = 3;
        private static int REQUEST_QUEUE_WAIT_TIME = 1;
        private static int REQUEST_QUEUE_MAX_WAITS = 3;
        private int waited = 0;

        private static List<string> runningRequests = new List<string>();

        public delegate void OnNumberOfRequestsChangedEventHandler();
        public static event OnNumberOfRequestsChangedEventHandler OnNumberOfRequestsChanged;

        protected virtual bool IsReady()
        {
            return Game.Instance.IsReady();
        }

        private void SendOnNumberOfRequestsChanged()
        {
            // Send Event
            if (OnNumberOfRequestsChanged != null)
            {
                OnNumberOfRequestsChanged();
            }
        }

        System.Collections.IEnumerator CheckRequestQueue()
        {
            waited++;
            if (Game.Instance.GetClient().RunningRequests() > MAX_REQUESTS)
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
            if (runningRequests.Exists(l => l.Equals(apiPath)))
            {
                yield break;
            }
            yield return CheckRequestQueue();
            IndicateRequestStart(apiPath);
            WWW request = Request(apiPath, jsonString);
            yield return request;
            
            try
            {
                if (request.error == null)
                    onSuccess.DynamicInvoke(request.text, callbackLoad);
                else
                    HandleError(request);
            } catch (Exception e)
            {
                Debug.LogError(e);
            } finally
            {
                IndicateRequestFinished(apiPath);
            }
            
        }

        public System.Collections.IEnumerator Request(string apiPath, string jsonString, Action<string> onSuccess)
        {
            if (runningRequests.Exists(l => l.Equals(apiPath)))
            {
                yield break;
            }
            yield return CheckRequestQueue();
            IndicateRequestStart(apiPath);
            WWW request = Request(apiPath, jsonString);
            yield return request;
            
            try
            {
                if (request.error == null)
                    onSuccess.Invoke(request.text);
                else
                    HandleError(request);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                IndicateRequestFinished(apiPath);
            }
        }

        public WWW Request(string apiPath, string jsonString)
        {

            //Debug.Log(apiPath + "\n" + jsonString);
            string uri = ServerConstants.GetServerUrl(Game.Instance.GetClient().serverType) + "/" + apiPath;
            UTF8Encoding encoding = new UTF8Encoding();
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            headers.Add("X-App-Version", Client.VERSION.ToString());
            headers.Add("Authorization", "Bearer " + Game.Instance.GetSettings().GetToken());
            //Debug.Log(apiPath + " token: " + Game.Instance.GetSettings().GetToken());
            WWW www;
            if (jsonString != null)
            {
                headers.Add("Content-Type", "application/json");
                //headers.Add("Content-Length", encoding.GetByteCount(jsonString).ToString());
                www = new WWW(uri, encoding.GetBytes(jsonString), headers);
            } else
            {
                www = new WWW(uri, null, headers);
            }
            return www;
        }

        public void HandleError(WWW request)
        {
            ErrorMessage message = null;
            try
            {
                message = JsonUtility.FromJson<ErrorMessage>(request.text);
                if (message.status == 401)
                {
                    Game.Instance.LoadScene(Game.GameScene.AuthorizationScene);
                    Game.Instance.GetSettings().SetToken(null);
                    Game.Instance.GetPlayer().SetLoggedIn(false);
                }
            } catch (Exception ex)
            {
                Debug.Log(request.text);
                Debug.LogError(ex);
            }
            HandleErrorCode(request.error, message, request.url);
        }

        private void HandleErrorCode(string error, ErrorMessage message, string api)
        {
            if (message != null)
                CheckErrorCode((int)message.status, message.message, true, api);
            else
            {
                if (error.Contains("500"))
                    CheckErrorCode(500, "Server is doing weird stuff", false, api);
                else if (error.Contains("502"))
                    CheckErrorCode(502, "Server is sending weird stuff", false, api);
                else if (error.Contains("503"))
                    CheckErrorCode(503, "No connection or server is down", false, api);
                else if (error.Contains("504"))
                    CheckErrorCode(504, "Connection too slow", false, api);
                else if (error.Contains("400"))
                    CheckErrorCode(400, "App is sending weird stuff", false, api);
                else
                    CheckErrorCode(500, "Sorry something went wrong", false, api);
            }
        }

        private void CheckErrorCode(int code, string message, bool custommessage, string api)
        {
            Debug.LogError("Orbi-Error: " + code + "("+api+"): " + message);
            // silent numbers unknown errors
            if ((code == 502) || (code == 504))
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
                    StartCoroutine(ShowUpdateInfo());
                    return;
                }
                Error.Show(message);
            }
        }

        System.Collections.IEnumerator ShowUpdateInfo()
        {
            Warning.Show("Update required");
            yield return new WaitForSeconds(5);
            StartCoroutine(RedirectToUpdateSite());
            Application.Quit();
        }

        System.Collections.IEnumerator RedirectToUpdateSite()
        {
            yield return new WaitForSeconds(1);
            Application.OpenURL("https://softwaredesign.foundation/orbi/");
            yield break;
        }

        public void IndicateRequestStart(string api)
        {
            runningRequests.Add(api);
            Game.Instance.GetClient().IncRunningRequests();
            SendOnNumberOfRequestsChanged();
        }

        public void IndicateRequestFinished(string api)
        {
            runningRequests.RemoveAll(l => api.Equals(l));
            Game.Instance.GetClient().DecRunningRequests();
            SendOnNumberOfRequestsChanged();
        }
    }
}
