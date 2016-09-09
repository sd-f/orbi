using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;
using System.Collections;
using ClientModel;

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

            StartCoroutine(Load());

            // load textures 
            // load terrain
            // reset player

            // finish loading animation
            
        }

        IEnumerator Load()
        {
            Game.GetWorld().SetCenterGeoPosition(new Position(Game.GetPlayer().GetPositionBeforeOutOfBounds()).ToGeoPosition());
            yield return Game.GetWorld().UpdateWorld();
            Game.GetGame().LoadScene(Game.GameScene.GameScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.GetGame().LoadScene(Game.GameScene.SettingsScene);
        }

    }
}
