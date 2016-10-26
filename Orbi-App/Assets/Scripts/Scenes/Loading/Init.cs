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
            StartCoroutine(Load());
            // TODO loading animation
        }

        IEnumerator Load()
        {
            
            yield return Game.Instance.GetWorld().UpdateWorld();
            yield return Game.Instance.GetPlayer().GetCraftingController().LoadInventory();
            Game.Instance.GetLocation().Resume();
            Game.Instance.GetPlayer().Unfreeze();
            Game.Instance.GetWorld().ForceRefreshOnNextLoading();
            Game.Instance.LoadScene(Game.GameScene.GameScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.Instance.LoadScene(Game.GameScene.SettingsScene);
        }

    }
}
