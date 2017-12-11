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
        public MainCanvas canvas;
        public Text craftingAmount;
        
        public void OnCraftOk()
        {
            if (!Game.Instance.GetPlayer().GetConstructionController().IsColliding())
            {
                Game.Instance.GetPlayer().GetConstructionController().Craft();
                canvas.StopCrafting();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Game.Instance.GetPlayer().GetConstructionController().StartCrafting();
        }

        public void OnDisable()
        {
            Game.Instance.GetPlayer().GetConstructionController().StopCrafting();
        }

        public override void OnReady()
        {
            // ex check inventory?
        }

        private void SetAmount()
        {
            // TODO
            craftingAmount.text = "∞";
        }

        public void OnCraftCancel()
        {
            canvas.StopCrafting();
        }

        void Update()
        {
            if (desktopMode)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnCraftCancel();
                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1")) && !Game.Instance.IsInTypingMode())
                    if (Game.Instance.GetPlayer().GetConstructionController().IsCrafting())
                        OnCraftOk();
            }
            
        }

        

    }
}
