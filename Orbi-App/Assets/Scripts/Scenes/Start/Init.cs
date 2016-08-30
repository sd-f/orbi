using CanvasUtility;
using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes.Start
{
    [AddComponentMenu("App/Scenes/Start/Init")]
    class Init : MonoBehaviour
    {

        void Awake()
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


            // TODO wait for location


            // check logged in
            yield return Game.GetPlayer().GetAuthService().LoadGameIfAuthorized();

            // if scene has not changed activate login form
            if (SceneManager.GetActiveScene().name == "StartScene") {
                GameObject.Find("Menu").SendMessage("SetFormEnabled", true);
            }
        }

    }
}