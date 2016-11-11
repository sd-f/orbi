using GameController;
using System.Collections;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Init")]
    class Init : MonoBehaviour
    {
        private int waited = 0;

        void Awake()
        {
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        }

        IEnumerator Start()
        {
            Game.Instance.GetClient().Log("Debug: Orbi started.", this);
            print("Orbi started.");
            Game.Instance.SetReady(false);

            Game.Instance.EnterTypingMode();

            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            // load settings
            Game.Instance.GetSettings().Init();

            // udpating game settings
            Game.Instance.GetSettings().SetClientVersion(Client.VERSION);
            yield return StartCoroutine(Boot());
            waited = 0;
            Game.Instance.LeaveTypingMode();
            if (!Game.Instance.GetPlayer().IsLoggedIn())
            {
                Game.Instance.LoadScene(Game.GameScene.AuthorizationScene);
               
            } else
            {
                Game.Instance.SetReady(true);
            }
                
        }

        IEnumerator Boot()
        {
            if ((Application.GetStreamProgressForLevel(1) == 1) || (waited > 20))
            {
                yield return Load();
            } else
            {
                yield return new WaitForSeconds(1);
                yield return Boot();
            }
        }

        IEnumerator Load()
        {
            yield return Game.Instance.GetServerService().RequestInfo();
            yield return Game.Instance.GetWorld().GetGameObjectService().RequestStatistics();
            yield return Game.Instance.GetWorld().GetTerrainService().ResetTerrain();
            yield return Game.Instance.GetAuthService().LoadGameIfAuthorized();
            yield return Game.Instance.GetLocation().Boot();
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestPlayer();
            yield return Game.Instance.GetPlayer().LoadInventory();
            yield return new WaitForSeconds(1);
        }





    }
}
