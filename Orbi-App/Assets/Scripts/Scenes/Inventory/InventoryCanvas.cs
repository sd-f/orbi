using CanvasUtility;
using GameController;
using ServerModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/InventoryCanvas")]
    public class InventoryCanvas : MonoBehaviour
    {

        public InputField userTextInputField;
        public UnityEngine.GameObject userTextForm;
        public Text statusText;
        public InventoryCamera inventoryCamera;

        public void ShowUserForm(int selectedIndex)
        {
            userTextForm.SetActive(true);
            userTextInputField.text = Game.GetPlayer().GetCraftingController().GetUserText();
        }

        public void SetStatusText(string text)
        {
            statusText.text = text;
        }

        public void OnSaveUserText()
        {
            userTextForm.SetActive(false);
            Game.GetPlayer().GetCraftingController().SetUserText(userTextInputField.text);
            foreach(KeyValuePair<int, InventoryItem> item in inventoryCamera.GetObjectsList())
                if (item.Value.supportsUserText)
                    GameObjectUtility.TrySettingTextInChildren(item.Value.gameObject, Game.GetPlayer().GetCraftingController().GetUserText());
           // 
                
        }

    }
}
