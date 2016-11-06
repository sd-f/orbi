using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/SettingsContentTab")]
    class SettingsContentTab : MonoBehaviour
    {

        public void OnExit()
        {
            Game.Instance.Quit();
        }
    }
}
