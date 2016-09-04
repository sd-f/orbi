using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LoadingScene
{
    [AddComponentMenu("App/Scenes/Loading/Init")]
    class Init : MonoBehaviour
    {
        void Awake()
        {
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

            // set center

            StartCoroutine(UpdateWorld());

            // load textures 
            // load terrain
            // reset player

            // finish loading animation
            
        }

        IEnumerator UpdateWorld()
        {
            yield return Game.GetWorld().UpdateWorld();
            SceneManager.LoadScene("GameScene");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("GameScene");
        }

    }
}
