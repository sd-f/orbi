using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/SettingsCanvasInfos")]
    class SettingsCanvasInfos : MonoBehaviour
    {

        public Text motdText;
        public Text version;
        public Text objects;

        void Start()
        {
            StartCoroutine(LoadInfos());
        }

        private IEnumerator LoadInfos()
        {
            yield return Game.Instance.GetServerService().RequestInfo();
            SetInfos();
        }

        private void SetInfos()
        {
            version.text = "v " + Client.VERSION.ToString();
            objects.text = Game.Instance.GetWorld().GetStatistics().numberOfObjects.ToString();
            ServerModel.ServerInfo serverInfo = Game.Instance.GetClient().GetServerInfo();
            motdText.text = "";
            if (serverInfo != null && serverInfo.messages != null)
                foreach (ServerModel.MessageOfTheDay motd in serverInfo.messages)
                {
                    motdText.text = motdText.text + "\n" + motd.message;
                }
        }
    }
}
