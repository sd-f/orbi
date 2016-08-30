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
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            // load settings
            Game.GetGame().GetSettings().Load();
            // udpating game settings
            Game.GetGame().GetSettings().SetClientVersion(Client.VERSION);

            StartCoroutine(Boot());
        }

        IEnumerator Boot()
        {
            // check client version
            yield return Game.GetGame().GetServerService().RequestVersion();

            // boot gps location
            yield return Game.GetLocation().Boot();

            // check logged in
            yield return Game.GetPlayer().GetAuthService().LoadGameIfAuthorized();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

    }
}
