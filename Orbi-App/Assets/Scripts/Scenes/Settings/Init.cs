using UnityEngine;
using GameController;

namespace Assets.Scripts.Scenes.Settings
{
    [AddComponentMenu("App/Scenes/Settings/Init")]
    class Init : MonoBehaviour
    {
        void Awake()
        {
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

        }
        
    }
}
