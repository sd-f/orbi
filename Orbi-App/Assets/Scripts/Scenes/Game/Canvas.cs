using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (Input.GetKeyDown(KeyCode.I))
            {
                Game.GetGame().LoadScene(Game.GameScene.InventoryScene);
            }
        }

    }
}
