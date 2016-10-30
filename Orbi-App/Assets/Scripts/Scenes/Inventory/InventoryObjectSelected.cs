using CanvasUtility;
using GameController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/InventoryObjectSelected")]
    public class InventoryObjectSelected : MonoBehaviour
    {
        void OnTouched(InventoryCamera inventoryCamera) { 
            long id = GameObjectUtility.GetId(this.gameObject);
            Game.Instance.GetPlayer().GetCraftingController().SetSelectedType(inventoryCamera.GetObjectsList()[(int)id].type);
            Game.Instance.GetWorld().SkipRefreshOnNextLoading();
            Game.Instance.LoadScene(Game.GameScene.LoadingScene);
        }

    }
}
