using System;
using CanvasUtility;
using GameController;
using UnityEngine;
using UnityEngine.UI;
using GameController.Services;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/CraftingCanvas")]
    public class CraftingCanvas : GameMonoBehaviour
    {

        public GameObject craftButton;
        public GameObject craftOkButton;
        public GameObject craftCancelButton;
        public GameObject craftingContainer;
        public Text craftingAmount;
        public GameObject crosshair;

        public void OnCraft()
        {
            StartCrafting();
        }

        public void OnCraftOk()
        {
            Game.Instance.GetPlayer().GetConstructionController().Craft();
            StopCrafting();
        }

        public override void OnReady()
        {
            Game.Instance.GetPlayer().GetConstructionController().CheckInventory();
        }

        public override void Awake()
        {
            base.Awake();
            crosshair.SetActive(desktopMode);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            crosshair.SetActive(desktopMode);
        }

        public override void OnInputModeChanged()
        {
            crosshair.SetActive(desktopMode);
        }


        private void SetAmount()
        {
            string prefab = Game.Instance.GetPlayer().GetConstructionController().GetSelectedType().prefab;
            long amount = Game.Instance.GetPlayer().GetInventoryService().GetInventoryItem(prefab).amount;
            craftingAmount.text = "x " + amount;
            if (prefab.Equals(InventoryService.ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB))
                craftingAmount.text = "∞";
        }

        public void OnCraftCancel()
        {
            StopCrafting();
        }

        void Update()
        {
            if (desktopMode)
            {
                if (Input.GetKeyDown(KeyCode.C) && !Game.Instance.IsInTypingMode())
                    if (!Game.Instance.GetPlayer().GetConstructionController().IsCrafting())
                        OnCraft();
                    else
                        OnCraftCancel();
                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1")) && !Game.Instance.IsInTypingMode())
                    if (Game.Instance.GetPlayer().GetConstructionController().IsCrafting())
                        OnCraftOk();
            }
            
        }

        private void StartCrafting()
        {
            ButtonUtility.SetButtonState(craftButton, false);
            ButtonUtility.SetButtonState(craftOkButton, true);
            ButtonUtility.SetButtonState(craftCancelButton, true);
            Game.Instance.GetPlayer().GetConstructionController().StartCrafting();
            craftingAmount.gameObject.SetActive(true);
            SetAmount();
        }

        private void StopCrafting()
        {
            ButtonUtility.SetButtonState(craftButton, true);
            ButtonUtility.SetButtonState(craftOkButton, false);
            ButtonUtility.SetButtonState(craftCancelButton, false);
            Game.Instance.GetPlayer().GetConstructionController().StopCrafting();
            craftingAmount.gameObject.SetActive(false);
        }

        

    }
}
