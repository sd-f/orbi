using UnityEngine;
using System.Collections;
using GameController;
using CanvasUtility;

namespace StartScene
{
    [AddComponentMenu("App/Scenes/Start/Init")]
    class Init : MonoBehaviour
    {

        void Start()
        {
            Game.Instance.GetClient().Log("Debug: Orbi started.", this);
            print("Orbi started.");

            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            // load settings
            Game.Instance.GetSettings().Init();

            // udpating game settings
            Game.Instance.GetSettings().SetClientVersion(Client.VERSION);

            StartCoroutine(Game.Instance.GetWorld().GetGameObjectService().RequestStatistics());
            StartCoroutine(Boot());
        }

        IEnumerator Boot()
        {
            // check client version
            yield return Game.Instance.GetServerService().RequestInfo();

            // boot gps location
            yield return Game.Instance.GetLocation().Boot();

            // init player
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestPlayer();

            // reset terrain
            yield return Game.Instance.GetWorld().GetTerrainService().ResetTerrain();

            // check logged in
            yield return Game.Instance.GetAuthService().LoadGameIfAuthorized();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.Instance.Quit();
        }
    }
}
