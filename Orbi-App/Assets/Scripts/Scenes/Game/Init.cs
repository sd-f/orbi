using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Init")]
    class Init : MonoBehaviour
    {
        void Awake()
        {
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            Game.Instance.LeaveTypingMode();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Game.Instance.LoadScene(Game.GameScene.SettingsScene);
            }
        }

    }
}
