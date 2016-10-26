using GameController;
using UnityEngine;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Init")]
    class Init : MonoBehaviour
    {

        void Awake()
        {
            //Game.Instance.GetLocation().Pause();
            Game.Instance.GetPlayer().Freeze();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            Game.Instance.EnterTypingMode();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.Instance.Quit();
        }

        void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}