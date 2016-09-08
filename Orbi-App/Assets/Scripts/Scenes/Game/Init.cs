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
            Game.GetPlayer().RestoreRotation();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("SettingsScene");
        }
        
    }
}
