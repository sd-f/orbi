using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/CraftingCanvas")]
    public class CraftingCanvas : MonoBehaviour
    {

        public GameObject craftButton;
        public GameObject craftOkButton;
        public GameObject craftCancelButton;
        public GameObject craftingContainer;
        private PlayerConstructionController controller;
        private bool isDesktopMode = false;

        void Start()
        {
            controller = craftingContainer.GetComponent<PlayerConstructionController>();
        }

        void Awake()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
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
            if (isDesktopMode)
            {
                if (Input.GetKeyDown(KeyCode.C))
                    if (!Game.GetPlayer().GetCraftingController().IsCrafting())
                        OnCraft();
                    else
                        OnCraftCancel();
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1"))
                    if (Game.GetPlayer().GetCraftingController().IsCrafting())
                        OnCraftOk();
            }
            
        }

        private void StartCrafting()
        { 
            SetButtonState(craftButton, false);
            SetButtonState(craftOkButton, true);
            SetButtonState(craftCancelButton, true);
            controller.StartCrafting();
        }

        private void StopCrafting()
        {
            SetButtonState(craftButton, true);
            SetButtonState(craftOkButton, false);
            SetButtonState(craftCancelButton, false);
            controller.StopCrafting();
        }

        private void SetButtonState(GameObject button, bool state)
        {
            button.GetComponent<Button>().interactable = state;
            button.GetComponent<Image>().enabled = state;
            foreach(Image image in button.GetComponentsInChildren<Image>())
            {
                image.enabled = state;
            }
        }

    }
}
