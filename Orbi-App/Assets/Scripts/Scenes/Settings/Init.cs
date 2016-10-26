using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;

namespace SettingsScene
{
    [AddComponentMenu("App/Scenes/Settings/Init")]
    class Init : MonoBehaviour
    {
        void Start()
        {
            Game.Instance.GetWorld().SkipRefreshOnNextLoading();
        }

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
                Game.Instance.LoadScene(Game.GameScene.LoadingScene);
        }

    }
}
