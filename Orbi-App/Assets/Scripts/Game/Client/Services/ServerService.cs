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
            yield return Request("server/info", null, OnServerInf);
        }

        private void OnServerInf(string data)
        {
            ServerInfo info = JsonUtility.FromJson<ServerInfo>(data);
            Game.GetClient().SetServerInfo(info);
        }

    }
}
