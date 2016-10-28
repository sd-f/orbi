using UnityEngine;
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
        public GameObject newItemPrefab;
        public InventoryCamera inventoryCamera;
        public InventoryCanvas canvas;
        private LayerMask layers;

        public static float OBJECT_PADDING_HORIZONTAL = 1.75f;
        public static float OBJECT_PADDING_VERTICAL = 2.75f;

        void Start()
        {
            layers = LayerMask.NameToLayer("Inventory");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            StartCoroutine(LoadInventory());
        }

        IEnumerator LoadInventory()
        {
            CraftingController controller = Game.Instance.GetPlayer().GetCraftingController();
            yield return controller.LoadInventory();

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

            List<ServerModel.InventoryItem> items = controller.GetInventory().items;
            List<string> itemsDiscovered = new List<string>();
            foreach (ServerModel.GameObjectTypeCategory category in controller.GetInventory().categories)
            {
                if (category.craftable)
                {
                    overall_items = overall_items + category.numberOfItems;
                    if (maxItems < category.numberOfItems)
                        maxItems = category.numberOfItems;
                    GameObject categoryContainer = new GameObject();
                    categoryContainer.layer = layers;
                    categoryContainer.name = "category_" + category.id;
                    categoryContainer.transform.SetParent(objectsContainer.transform);
                    categoryContainer.transform.localPosition = new Vector3(0.0f, paddingVertical, 5f);

                    items_collected = 0;
                    paddingHorizontal = 0;
                    foreach (ServerModel.InventoryItem item in items)
                        if (item.categoryId == category.id)
                        {
                            itemsDiscovered.Add(item.prefab);
                            GameObject itemContainer = new GameObject();
                            itemContainer.layer = layers;
                            itemContainer.name = "item_" + category.id + "_" + id;
                            itemContainer.transform.SetParent(categoryContainer.transform, false);
                            itemContainer.transform.localPosition = new Vector3(paddingHorizontal + 1f, -2f, 0.5f);
                            inventoryCamera.GetObjectsList().Add(id, item);

                            GameObject itemAmountText = GameObject.Instantiate(textPrefab) as GameObject;
                            GameObjectUtility.SetLayer(itemAmountText, layers);
                            itemAmountText.name = "itemamounttext_" + category.id + "_" + id;
                            itemAmountText.transform.localScale = itemAmountText.transform.localScale * 0.5f;
                            itemAmountText.transform.SetParent(itemContainer.transform, false);
                            itemAmountText.transform.localPosition = new Vector3(0f, -0.25f, 0f);
                            ShadowText text = itemAmountText.GetComponent<ShadowText>();
                            text.SetText("x " + item.amount);
                            text.SetForeGroundColor(Color.white);
                            text.SetShadowColor(Color.black);
                            text.SetAlignment(TextAlignment.Center, TextAnchor.MiddleCenter);
                            GameObject newObject = GameObjectFactory.CreateObject(itemContainer.transform, item.prefab, id, null, layers);
                            newObject.AddComponent<InventoryObjectSelected>();
                            newObject.transform.localRotation = Quaternion.Euler(-5f, -5f, 0f);
                            GameObjectUtility.NormalizeScale(newObject);
                            item.gameObject = newObject;
                            overall_items_collected++;
                            items_collected++;
                            id++;
                            paddingHorizontal = paddingHorizontal + OBJECT_PADDING_HORIZONTAL;
                        }
                    for (int i = (int)items_collected; i < (category.numberOfItems); i++)
                    {

                        GameObject itemContainer = new GameObject();
                        itemContainer.layer = layers;
                        itemContainer.name = "item_" + category.id + "_" + "unknown";
                        itemContainer.transform.SetParent(categoryContainer.transform, false);
                        itemContainer.transform.localPosition = new Vector3(paddingHorizontal + 0.7f, -1.7f, 0f);

                        GameObject itemAmountText = GameObject.Instantiate(textPrefab) as GameObject;
                        itemAmountText.name = "itemunknowntext_" + category.id + "_" + id;
                        itemAmountText.transform.localScale = itemAmountText.transform.localScale * 2f;
                        itemAmountText.transform.SetParent(itemContainer.transform, false);
                        GameObjectUtility.SetLayer(itemAmountText, layers);
                        ShadowText text = itemAmountText.GetComponent<ShadowText>();
                        text.SetText("?");
                        text.SetForeGroundColor(Color.gray);
                        text.SetShadowColor(Color.white);
                        text.SetAlignment(TextAlignment.Center, TextAnchor.MiddleCenter);
                        id++;
                        paddingHorizontal = paddingHorizontal + OBJECT_PADDING_HORIZONTAL;

                    }
                    GameObject categoryText = GameObject.Instantiate(textPrefab) as GameObject;
                    GameObjectUtility.SetLayer(categoryText, layers);
                    categoryText.name = "categorytext_" + category.id;
                    categoryText.transform.SetParent(categoryContainer.transform, false);
                    if (category.numberOfItems.Equals(items_collected))
                    {
                        categoryText.transform.localPosition = new Vector3(0.75f, 0f, 0f);
                        GameObject categoryCompletedIcon = GameObject.Instantiate(startPrefab) as GameObject;
                        GameObjectUtility.SetLayer(categoryCompletedIcon, layers);
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
                (Game.Instance.GetPlayer().GetCraftingController().GetInventory().categories.Count * OBJECT_PADDING_VERTICAL) / 1.75f);

            //canvasScript.SetSelected(preselected);
            canvas.SetStatusText("Collected: " + overall_items_collected + "/" + overall_items);

            if (controller.itemsDiscovered == null)
                controller.itemsDiscovered = itemsDiscovered;
            else if (itemsDiscovered.Count > controller.itemsDiscovered.Count)
            {
                GameObject newItemText = GameObject.Instantiate(newItemPrefab) as GameObject;
                GameObjectUtility.SetLayer(newItemText, layers);
                newItemText.transform.SetParent(inventoryCamera.transform, false);
                newItemText.transform.localPosition = new Vector3(0, 0, 2);
                newItemText.name = "newItemText";
                controller.itemsDiscovered = itemsDiscovered;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Game.Instance.GetWorld().SkipRefreshOnNextLoading();
                Game.Instance.LoadScene(Game.GameScene.LoadingScene);
            }
               
        }

    }
}
