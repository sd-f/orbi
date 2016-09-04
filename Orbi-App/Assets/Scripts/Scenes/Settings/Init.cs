using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;

namespace SettingsScene
{
    [AddComponentMenu("App/Scenes/Settings/Init")]
    class Init : MonoBehaviour
    {
        void Awake()
        {
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("LoadingScene");
        }

    }
}
