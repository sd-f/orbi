using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas")]
    public class Canvas : MonoBehaviour
    {

        public void OnSettings()
        {
            Game.Instance.LoadScene(Game.GameScene.SettingsScene);
        }

        public void OnInventory()
        {
            Game.Instance.LoadScene(Game.GameScene.InventoryScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && !Game.Instance.IsInTypingMode())
                Game.Instance.LoadScene(Game.GameScene.InventoryScene);

        }

    }
}
