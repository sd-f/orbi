using ServerModel;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameController.Services
{

    public class ServerService: AbstractHttpService
    {

        public IEnumerator RequestInfo()
        {
            yield return Request("server/info", null, OnServerInfo);
        }

        private void OnServerInfo(string data)
        {
            ServerInfo info = JsonUtility.FromJson<ServerInfo>(data);
            Game.Instance.GetClient().SetServerInfo(info);
        }

    }
}
