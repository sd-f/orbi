﻿using System;
using CanvasUtility;
using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/CraftingCanvas")]
    public class CraftingCanvas : InputModeMonoBehaviour
    {

        public GameObject craftButton;
        public GameObject craftOkButton;
        public GameObject craftCancelButton;
        public GameObject craftingContainer;
        private PlayerConstructionController controller;

        void Start()
        {
            controller = craftingContainer.GetComponent<PlayerConstructionController>();
        }

        private void SetInputMode()
        {
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        public void OnCraft()
        {
            StartCrafting();
        }

        public void OnCraftOk()
        {
            controller.Craft();
            StopCrafting();
        }

        public void OnCraftCancel()
        {
            StopCrafting();
        }

        void Update()
        {
            if (Game.Instance.GetSettings().IsDesktopInputEnabled())
            {
                if (Input.GetKeyDown(KeyCode.C) && !Game.Instance.IsInTypingMode())
                    if (!Game.Instance.GetPlayer().GetCraftingController().IsCrafting())
                        OnCraft();
                    else
                        OnCraftCancel();
                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1")) && !Game.Instance.IsInTypingMode())
                    if (Game.Instance.GetPlayer().GetCraftingController().IsCrafting())
                        OnCraftOk();
            }
            
        }

        private void StartCrafting()
        { 
            ButtonUtility.SetButtonState(craftButton, false);
            ButtonUtility.SetButtonState(craftOkButton, true);
            ButtonUtility.SetButtonState(craftCancelButton, true);
            controller.StartCrafting();
        }

        private void StopCrafting()
        {
            ButtonUtility.SetButtonState(craftButton, true);
            ButtonUtility.SetButtonState(craftOkButton, false);
            ButtonUtility.SetButtonState(craftCancelButton, false);
            controller.StopCrafting();
        }

        

    }
}
