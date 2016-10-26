using CanvasUtility;
using GameController;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SettingsScene
{
    [AddComponentMenu("App/Scenes/Settings/Canvas")]
    public class Canvas : MonoBehaviour
    {
        public GameObject toggleHeightsEnabled;
        public GameObject toggleSetalliteOverlayEnabled;
        public GameObject toggleHandheldInputEnabled;
        public GameObject toggleDesktopInputEnabled;

        public Text textNumberOfObjects;
        public Text textVersion;
        public Text textMotd;



        void Start()
        {
            textVersion.text = "version " + Client.VERSION.ToString();
            textNumberOfObjects.text = "objects " + Game.Instance.GetWorld().GetStatistics().numberOfObjects.ToString();
            ServerModel.ServerInfo serverInfo = Game.Instance.GetClient().GetServerInfo();
            textMotd.text = "";
            if (serverInfo != null && serverInfo.messages != null)
                foreach(ServerModel.MessageOfTheDay motd in serverInfo.messages)
                {
                    textMotd.text = textMotd.text + "\n" + motd.message;
                }
            toggleHeightsEnabled.GetComponent<Toggle>().isOn = Game.Instance.GetSettings().IsHeightsEnabled();
            toggleSetalliteOverlayEnabled.GetComponent<Toggle>().isOn = Game.Instance.GetSettings().IsSatelliteOverlayEnabled();
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = Game.Instance.GetSettings().IsHandheldInputEnabled();
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }


        public void OnHeightsEnabled(bool enabled)
        {
            Game.Instance.GetWorld().ForceRefreshOnNextLoading();
            if (Game.Instance.GetSettings().IsHeightsEnabled() != enabled)
            {
                Game.Instance.GetSettings().SetHeightsEnabled(enabled);
            }
                
        }

        public void OnSatelliteOverlayEnabled(bool enabled)
        {
            Game.Instance.GetWorld().ForceRefreshOnNextLoading();
            if (Game.Instance.GetSettings().IsSatelliteOverlayEnabled() != enabled)
            {
                Game.Instance.GetSettings().SetSatelliteOverlayEnabled(enabled);
            }
        }

        public void OnHandheldInputEnabled(bool enabled)
        {
            if (enabled && !Game.Instance.GetLocation().IsReady())
            {
                StartCoroutine(Game.Instance.GetLocation().Boot());
            }
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.Instance.GetSettings().IsHandheldInputEnabled() != enabled)
            {
                Game.Instance.GetSettings().SetHandheldInputEnabled(enabled);
                //Game.Instance.GetSettings().SetDesktopInputEnabled(!enabled);
            }
                
        }

        public void OnDesktopInputEnabled(bool enabled)
        {
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.Instance.GetSettings().IsDesktopInputEnabled() != enabled)
            {
                Game.Instance.GetSettings().SetDesktopInputEnabled(enabled);
                //Game.Instance.GetSettings().SetHandheldInputEnabled(!enabled);
            }
               
        }


        public void OnBack()
        {
            Game.Instance.LoadScene(Game.GameScene.LoadingScene);
        }

        public void OnExit()
        {
            Game.Instance.Quit();
        }


    }
}
