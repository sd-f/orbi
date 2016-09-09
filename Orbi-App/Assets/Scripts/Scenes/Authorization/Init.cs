using CanvasUtility;
using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Init")]
    class Init : MonoBehaviour
    {

        void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
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