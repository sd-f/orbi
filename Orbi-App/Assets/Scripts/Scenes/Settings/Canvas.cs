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
            textNumberOfObjects.text = "objects " + Game.GetWorld().GetStatistics().numberOfObjects.ToString();
            ServerModel.ServerInfo serverInfo = Game.GetClient().GetServerInfo();
            textMotd.text = "";
            if (serverInfo != null && serverInfo.messages != null)
                foreach(ServerModel.MessageOfTheDay motd in serverInfo.messages)
                {
                    textMotd.text = textMotd.text + "\n" + motd.message;
                }
            toggleHeightsEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsHeightsEnabled();
            toggleSetalliteOverlayEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsSatelliteOverlayEnabled();
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsHandheldInputEnabled();
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }


        public void OnHeightsEnabled(bool enabled)
        {
            Game.GetWorld().ForceRefreshOnNextLoading();
            if (Game.GetGame().GetSettings().IsHeightsEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetHeightsEnabled(enabled);
            }
                
        }

        public void OnSatelliteOverlayEnabled(bool enabled)
        {
            Game.GetWorld().ForceRefreshOnNextLoading();
            if (Game.GetGame().GetSettings().IsSatelliteOverlayEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetSatelliteOverlayEnabled(enabled);
            }
        }

        public void OnHandheldInputEnabled(bool enabled)
        {
            if (enabled && !Game.GetLocation().IsReady())
            {
                StartCoroutine(Game.GetLocation().Boot());
            }
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.GetGame().GetSettings().IsHandheldInputEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetHandheldInputEnabled(enabled);
                //Game.GetGame().GetSettings().SetDesktopInputEnabled(!enabled);
            }
                
        }

        public void OnDesktopInputEnabled(bool enabled)
        {
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.GetGame().GetSettings().IsDesktopInputEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetDesktopInputEnabled(enabled);
                //Game.GetGame().GetSettings().SetHandheldInputEnabled(!enabled);
            }
               
        }


        public void OnBack()
        {
            Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
        }

        public void OnExit()
        {
            Game.GetGame().Quit();
        }


    }
}
