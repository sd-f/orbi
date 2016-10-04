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
    [AddComponentMenu("App/Scenes/Inventory/Canvas")]
    public class Canvas : MonoBehaviour
    {
        private int currentIndex = 0;
        private SortedList<int, InventoryItem> objectsList = new SortedList<int, InventoryItem>();
        private InventoryObjects inventoryObjectsScript;
        private Vector3 firstpoint;
        private Vector3 secondpoint;
        private bool isDesktopMode = false;
        public UnityEngine.GameObject inventoryObjectsContainer;
        public Text amountText;
        

        void Awake()
        {
            this.inventoryObjectsScript = inventoryObjectsContainer.GetComponent<InventoryObjects>();
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }

        public void SetSelected(int selectedIndex)
        {
            currentIndex = selectedIndex;
            InventoryItem item = objectsList[currentIndex];
            SetAmountText(item.amount);
            inventoryObjectsScript.MoveToPosition(currentIndex * Init.OBJECT_PADDING);
        }

        public void OnLeft()
        {
            if (objectsList.ContainsKey(currentIndex - 1))
                SetSelected(currentIndex - 1);
        }

        public void OnRight()
        {
            if (objectsList.ContainsKey(currentIndex + 1))
                SetSelected(currentIndex + 1);
        }

        private void SetAmountText(int amount)
        {
            amountText.text = "x " + amount;
        }

        void Update()
        {
            if (isDesktopMode)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    OnLeft();
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    OnRight();
                if (Input.GetKeyDown(KeyCode.Return))
                    OnOk();
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        firstpoint = Input.GetTouch(0).position;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {

                        secondpoint = Input.GetTouch(0).position;
                        float moved = (secondpoint.x - firstpoint.x) / Screen.width * 20;
                        /*
                        Text text = UnityEngine.GameObject.Find("DebugText").GetComponent<Text>();
                        text.text = "secondpoint.x: " + secondpoint.x
                            + "\nmoved: " + moved; */
                        if (moved >= 5)
                            OnLeft();
                        if (moved <= -5)
                            OnRight();
                        if (moved > -5 && moved < 5)
                        {
                            checkTouchObjectSingleTouch(secondpoint);
                        }
                    }
                    

                }
            }
        }

        private void checkTouchObjectSingleTouch(Vector2 position)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit))
            {
                UnityEngine.GameObject container = GameObjectUtility.GetObjectContainer(hit.transform.gameObject);
                container.SendMessage("OnTouched", this);
            }
        }

        private void checkTouchObject()
        {
            int touchCorrection = 1;
            Info.Show("checkTouchObject");
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i + touchCorrection < Input.touchCount; ++i)
            {
                Info.Show("checkTouchObject touch");
                if (Input.GetTouch(i).phase.Equals(TouchPhase.Ended))
                {
                    Info.Show("checkTouchObject ended");
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Info.Show("checkTouchObject hit");
                        hit.transform.gameObject.SendMessage("OnTouched");
                    }
                }
            }
        }

        public void OnOk()
        {
            InventoryItem item = objectsList[currentIndex];
            Game.GetPlayer().GetCraftingController().SetSelectedPrefab(item.prefab);
            Game.GetWorld().SkipRefreshOnNextLoading();
            Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
        }

        public SortedList<int, InventoryItem> GetObjectsList()
        {
            return this.objectsList;
        }

    }
}
