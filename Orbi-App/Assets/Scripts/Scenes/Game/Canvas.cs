using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas")]
    public class Canvas : MonoBehaviour
    {
        Button craftButton;
        Button craftOkButton;
        Button craftCancelButton;


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
                Game.GetGame().LoadScene(Game.GameScene.InventoryScene);

        }

    }
}
