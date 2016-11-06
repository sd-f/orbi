using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Init")]
    class Init : MonoBehaviour
    {

        public GameObject splashScreen;

        void Start()
        {
            Game.Instance.GetClient().Log("Debug: Orbi started.", this);
            print("Orbi started.");

            Game.Instance.EnterTypingMode();

            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            // load settings
            Game.Instance.GetSettings().Init();

            // udpating game settings
            Game.Instance.GetSettings().SetClientVersion(Client.VERSION);

            StartCoroutine(Boot());
        }

        IEnumerator Boot()
        {
            // check client version
            yield return Game.Instance.GetServerService().RequestInfo();

            // check logged in
            yield return Game.Instance.GetAuthService().LoadGameIfAuthorized();

            yield return Game.Instance.GetLocation().Boot();

            while (!Game.Instance.IsReady())
            {
                yield return new WaitForSeconds(1);
            }
            Game.Instance.LeaveTypingMode();
            splashScreen.SetActive(false);
        }

        void Awake()
        {
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        }

        

    }
}
