using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas")]
    public class Canvas : MonoBehaviour
    {

        public GameObject settingsCanvas;
        public GameObject gameCanvas;

        public void OpenSettings()
        {
            Game.Instance.EnterTypingMode();
            settingsCanvas.SetActive(true);
            gameCanvas.SetActive(false);
        }

        public void CloseSettings()
        {
            Game.Instance.LeaveTypingMode();
            settingsCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }

        private void ToggleSettingsCanvas()
        {
            if (settingsCanvas.activeSelf)
                CloseSettings();
            else
                OpenSettings();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ToggleSettingsCanvas();
        }

    }
}
