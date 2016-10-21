using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas")]
    public class Canvas : MonoBehaviour
    {

        public void OnSettings()
        {
            Game.GetGame().LoadScene(Game.GameScene.SettingsScene);
        }

        public void OnInventory()
        {
            Game.GetGame().LoadScene(Game.GameScene.InventoryScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && !Game.GetGame().IsInTypingMode())
                Game.GetGame().LoadScene(Game.GameScene.InventoryScene);

        }

    }
}
