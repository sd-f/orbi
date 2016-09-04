using CanvasUtility;
using GameController;
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

        void Awake()
        {
            toggleHeightsEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsHeightsEnabled();
            toggleSetalliteOverlayEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsSatelliteOverlayEnabled();
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsHandheldInputEnabled();
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }

        public void OnHeightsEnabled(bool enabled)
        {
            if (Game.GetGame().GetSettings().IsHeightsEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetHeightsEnabled(enabled);
            }
                
        }

        public void OnSatelliteOverlayEnabled(bool enabled)
        {
            if (Game.GetGame().GetSettings().IsSatelliteOverlayEnabled() != enabled)
            {
                Game.GetGame().GetSettings().SetSatelliteOverlayEnabled(enabled);
            }
        }

        public void OnHandheldInputEnabled(bool enabled)
        {
            toggleDesktopInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.GetGame().GetSettings().IsHandheldInputEnabled() != enabled)
                Game.GetGame().GetSettings().SetHandheldInputEnabled(enabled);
        }

        public void OnDesktopInputEnabled(bool enabled)
        {
            toggleHandheldInputEnabled.GetComponent<Toggle>().isOn = !enabled;
            if (Game.GetGame().GetSettings().IsDesktopInputEnabled() != enabled)
                Game.GetGame().GetSettings().SetDesktopInputEnabled(enabled);
        }


        public void OnBack()
        {
            SceneManager.LoadScene("LoadingScene");
        }

        public void OnExit()
        {
            Game.GetGame().Quit();
        }

    }
}
