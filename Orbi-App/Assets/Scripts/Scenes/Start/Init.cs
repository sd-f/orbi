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
            Game.GetClient().Log("Debug: Orbi started.", this);
            print("Orbi started.");

            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            // load settings
            Game.GetGame().GetSettings().Init();

            // udpating game settings
            Game.GetGame().GetSettings().SetClientVersion(Client.VERSION);

            StartCoroutine(Game.GetWorld().GetGameObjectService().RequestStatistics());
            StartCoroutine(Boot());
        }

        IEnumerator Boot()
        {
            // check client version
            yield return Game.GetGame().GetServerService().RequestInfo();

            // boot gps location
            yield return Game.GetLocation().Boot();

            // init player
            yield return Game.GetPlayer().GetPlayerService().RequestUpdate();

            // reset terrain
            yield return Game.GetWorld().GetTerrainService().ResetTerrain();

            // check logged in
            yield return Game.GetGame().GetAuthService().LoadGameIfAuthorized();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.GetGame().Quit();
        }
    }
}
