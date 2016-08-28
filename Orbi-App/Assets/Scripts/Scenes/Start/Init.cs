using CanvasUtility;
using GameController;
using UnityEngine;

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

            // check client version



            // check logged in

        }

    }
}