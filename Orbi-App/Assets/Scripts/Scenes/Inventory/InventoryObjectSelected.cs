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
        void OnTouched(Canvas canvas)
        {
            long id = GameObjectUtility.GetId(this.gameObject);
            canvas.SetSelected((int)id);
            canvas.OnOk();
        }

    }
}
