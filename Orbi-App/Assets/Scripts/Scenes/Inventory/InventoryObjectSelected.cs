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
            Game.GetPlayer().GetCraftingController().SetSelectedPrefab(inventoryCamera.GetObjectsList()[(int)id].prefab);
            Game.GetWorld().SkipRefreshOnNextLoading();
            Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
        }

    }
}
