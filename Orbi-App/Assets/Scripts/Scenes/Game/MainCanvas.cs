using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/MainCanvas")]
    public class MainCanvas : GameMonoBehaviour
    {
        public GameObject settingsCanvas;
        public GameObject gameCanvas;
        public GameObject objectInfoCanvas;
        public GameObject craftingCanvas;
        public GameObject splashscreen;


        public override void OnReady()
        {

            base.OnReady();
            Reset();
        }

        public void OpenSettings()
        {
            Game.Instance.EnterTypingMode();
            CloseAll();
            settingsCanvas.SetActive(true);
        }


        private void ToggleSettingsCanvas()
        {
            if (settingsCanvas.activeSelf)
                Reset();
            else
                OpenSettings();
        }

        public void CloseAll()
        {
            settingsCanvas.SetActive(false);
            splashscreen.SetActive(false);
            objectInfoCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            craftingCanvas.SetActive(false);
        }

        public void Reset()
        {
            Game.Instance.LeaveTypingMode();
            CloseAll();
            gameCanvas.SetActive(true);
        }


        public void OpenCharacterInfos(ServerModel.Character character)
        {
            if (!IsAnyOverlayCanvasActive())
            {
                CloseAll();
                objectInfoCanvas.SetActive(true);
                objectInfoCanvas.GetComponent<ObjectsInfoCanvas>().SetCharacter(character);
            }
            
        }

        public bool IsAnyOverlayCanvasActive()
        {
            return objectInfoCanvas.activeSelf || settingsCanvas.activeSelf || splashscreen.activeSelf || craftingCanvas.activeSelf;
        }

        public void OpenObjectInfos(ServerModel.GameObject obj)
        {
            if (!IsAnyOverlayCanvasActive())
            {
                CloseAll();
                objectInfoCanvas.SetActive(true);
                objectInfoCanvas.GetComponent<ObjectsInfoCanvas>().SetObject(obj);
            }
        }

        public void StartCrafting()
        {
            if (!IsAnyOverlayCanvasActive())
            {
                CloseAll();
                craftingCanvas.SetActive(true);
            }
        }

        public void StopCrafting()
        {
            Reset();
        }

        void Update()
        {

            if (desktopMode)
            {
                if (Input.GetKeyDown(KeyCode.C) && !Game.Instance.IsInTypingMode())
                    if (!Game.Instance.GetPlayer().GetConstructionController().IsCrafting())
                        StartCrafting();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                if (IsAnyOverlayCanvasActive())
                    Reset();
                else
                    OpenSettings();
        }

    }
}
