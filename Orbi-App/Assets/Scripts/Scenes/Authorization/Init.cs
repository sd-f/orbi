using GameController;
using UnityEngine;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Init")]
    class Init : MonoBehaviour
    {

        void Awake()
        {
            //Game.GetLocation().Pause();
            Game.GetPlayer().Freeze();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            Game.GetGame().EnterTypingMode();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.GetGame().Quit();
        }

        void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}