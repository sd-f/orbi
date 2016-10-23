﻿using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/Init")]
    class Init : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject objectsContainer;
        public GameObject textPrefab;
        public GameObject startPrefab;
        public InventoryCamera inventoryCamera;
        public InventoryCanvas canvas;

        public static float OBJECT_PADDING_HORIZONTAL = 1.75f;
        public static float OBJECT_PADDING_VERTICAL = 2.75f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            StartCoroutine(LoadInventory());
        }

        IEnumerator LoadInventory()
        {
            yield return Game.GetPlayer().GetCraftingController().LoadInventory();

            GameObjectUtility.DestroyAllChildObjects(objectsContainer);

            // todo categories
            // header (found/remaining) + star + effect if complete
            // selection
            float paddingVertical = 0f;
            float paddingHorizontal = 0f;
            long overall_items = 0;
            long maxItems = 1;
            long overall_items_collected = 0;
            long items_collected = 0;
            int id = 0;
            int preselected = 0;
            List<ServerModel.InventoryItem> items = Game.GetPlayer().GetCraftingController().GetInventory().items;
            foreach (ServerModel.GameObjectTypeCategory category in Game.GetPlayer().GetCraftingController().GetInventory().categories)
            {
                if (category.craftable)
                {
                    overall_items = overall_items + category.numberOfItems;
                    if (maxItems < category.numberOfItems)
                        maxItems = category.numberOfItems;
                    GameObject categoryContainer = new GameObject();
                    categoryContainer.name = "category_" + category.id;
                    categoryContainer.transform.SetParent(objectsContainer.transform);
                    categoryContainer.transform.localPosition = new Vector3(0.0f, paddingVertical, 0f);
                   
                    items_collected = 0;
                    paddingHorizontal = 0;
                    foreach (ServerModel.InventoryItem item in items)
                        if (item.categoryId == category.id)
                        {
                            
                            GameObject itemContainer = new GameObject();
                            itemContainer.name = "item_" + category.id + "_" + id;
                            itemContainer.transform.SetParent(categoryContainer.transform, false);
                            itemContainer.transform.localPosition = new Vector3(paddingHorizontal + 1f, -2f, 0.5f);
                            inventoryCamera.GetObjectsList().Add(id, item);

                            GameObject itemAmountText = GameObject.Instantiate(textPrefab) as GameObject;
                            itemAmountText.name = "itemamounttext_" + category.id + "_" + id;
                            itemAmountText.transform.localScale = itemAmountText.transform.localScale * 0.5f;
                            itemAmountText.transform.SetParent(itemContainer.transform, false);
                            itemAmountText.transform.localPosition = new Vector3(0f, -0.25f, 0f);
                            ShadowText text = itemAmountText.GetComponent<ShadowText>();
                            text.SetText("x "+item.amount);
                            text.SetForeGroundColor(Color.white);
                            text.SetShadowColor(Color.black);
                            text.SetAlignment(TextAlignment.Center, TextAnchor.MiddleCenter);

                            if (item.prefab == Game.GetPlayer().GetCraftingController().GetSelectedPrefab())
                                preselected = id;
                            GameObject newObject = GameObjectFactory.CreateObject(itemContainer.transform, item.prefab, id, null, LayerMask.NameToLayer("Default"));
                            newObject.AddComponent<InventoryObjectSelected>();
                            newObject.transform.localRotation = Quaternion.Euler(-5f, -5f, 0f);
                            GameObjectUtility.NormalizeScale(newObject);
                            item.gameObject = newObject;
                            overall_items_collected++;
                            items_collected++;
                            id++;
                            paddingHorizontal = paddingHorizontal + OBJECT_PADDING_HORIZONTAL;
                        }
                    for (int i = (int)items_collected; i < (category.numberOfItems); i++ )
                    {

                        GameObject itemContainer = new GameObject();
                        itemContainer.name = "item_" + category.id + "_" + "unknown";
                        itemContainer.transform.SetParent(categoryContainer.transform, false);
                        itemContainer.transform.localPosition = new Vector3(paddingHorizontal + 0.7f, -1.7f, 0f);

                        GameObject itemAmountText = GameObject.Instantiate(textPrefab) as GameObject;
                        itemAmountText.name = "itemunknowntext_" + category.id + "_" + id;
                        itemAmountText.transform.localScale = itemAmountText.transform.localScale * 2f;
                        itemAmountText.transform.SetParent(itemContainer.transform, false);
                        ShadowText text = itemAmountText.GetComponent<ShadowText>();
                        text.SetText("?");
                        text.SetForeGroundColor(Color.gray);
                        text.SetShadowColor(Color.white);
                        text.SetAlignment(TextAlignment.Center, TextAnchor.MiddleCenter);
                        id++;
                        paddingHorizontal = paddingHorizontal + OBJECT_PADDING_HORIZONTAL;

                    }
                    GameObject categoryText = GameObject.Instantiate(textPrefab) as GameObject;
                    categoryText.name = "categorytext_" + category.id;
                    categoryText.transform.SetParent(categoryContainer.transform, false);
                    if (category.numberOfItems.Equals(items_collected))
                    {
                        categoryText.transform.localPosition = new Vector3(0.75f, 0f, 0f);
                        GameObject categoryCompletedIcon = GameObject.Instantiate(startPrefab) as GameObject;
                        categoryCompletedIcon.name = "categoryicon_" + category.id;
                        categoryCompletedIcon.transform.localPosition = new Vector3(0.3f, -0.3f, 0f);
                        categoryCompletedIcon.transform.SetParent(categoryContainer.transform, false);
                        categoryCompletedIcon.transform.localScale = new Vector3(3, 3, 3);
                    }
                    categoryText.GetComponent<ShadowText>().SetText(category.name + " (" + items_collected + "/" + category.numberOfItems + ")");
                    paddingVertical = paddingVertical - OBJECT_PADDING_VERTICAL;
                }
               

            }

            inventoryCamera.SetBounds(((maxItems) * OBJECT_PADDING_HORIZONTAL) / 1.25f, 
                (Game.GetPlayer().GetCraftingController().GetInventory().categories.Count * OBJECT_PADDING_VERTICAL) / 1.75f);

            //canvasScript.SetSelected(preselected);
            canvas.SetStatusText( "Collected: " + overall_items_collected + "/" + overall_items);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Game.GetWorld().SkipRefreshOnNextLoading();
                Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
            }
               
        }

    }
}
