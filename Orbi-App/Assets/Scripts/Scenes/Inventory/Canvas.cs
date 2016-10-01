﻿using GameController;
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
        public UnityEngine.GameObject inventoryObjectsContainer;
        public Text amountText;
        

        void Awake()
        {
            this.inventoryObjectsScript = inventoryObjectsContainer.GetComponent<InventoryObjects>();
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
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnRight();
            if (Input.GetKeyDown(KeyCode.Return))
                OnOk();
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
