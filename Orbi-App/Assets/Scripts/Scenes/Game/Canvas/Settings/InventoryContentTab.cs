using GameController;
using GameController.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/InventoryContentTab")]
    class InventoryContentTab : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject inventoryContent;
        public GameObject categoryPrefab;
        public GameObject inventoryItemPrefab;
        private float categoryOffsetY = 0f;
        private List<GameObject> categoriesObjects = new List<GameObject>();
        private List<ServerModel.GameObjectTypeCategory> categories;
        private List<ServerModel.InventoryItem> items;
        private static int ITEMS_PER_LINE = 4;
        private static float ITEM_SIZE = 275f;
        private InventoryService service;

        void OnEnable()
        {
            service = Game.Instance.GetPlayer().GetInventoryService();
            Init();
            foreach (ServerModel.GameObjectTypeCategory category in categories)
                if (category.craftable)
                    CreateCategory(category);
            StartCoroutine(DelayedPositioning());
        }

        private void CreateCategory(ServerModel.GameObjectTypeCategory category)
        {
            GameObject categoryObject = Instantiate(categoryPrefab) as GameObject;
            categoryObject.transform.SetParent(inventoryContent.transform, false);
            CategoryContainerScript categoryScript = categoryObject.GetComponent<CategoryContainerScript>();
            categoryScript.SetCategory(category);
            
            categoryObject.transform.Find("CategoryContent").GetComponent<RectTransform>().sizeDelta = new Vector3(1000f, ((category.types.Count / ITEMS_PER_LINE) + 1) * ITEM_SIZE);
            categoriesObjects.Add(categoryObject);
            float offsetY = 0;
            float offsetX = 0;
            int itemOffset = 0;
            int itemsDiscovered = 0;
            foreach (ServerModel.GameObjectType type in category.types)
            {
                offsetY = ITEM_SIZE * (itemOffset / ITEMS_PER_LINE);
                offsetX = ITEM_SIZE * (itemOffset % ITEMS_PER_LINE);
                if (CreateInventoryItem(type, offsetX, offsetY, categoryObject.transform.Find("CategoryContent")))
                    itemsDiscovered++;
                itemOffset++;
            }
            categoryScript.SetItemsDiscovered(itemsDiscovered);
        }


        private bool CreateInventoryItem(ServerModel.GameObjectType type, float offsetX, float offsetY, Transform parent)
        {
            GameObject itemObject = Instantiate(inventoryItemPrefab) as GameObject;
            InventoryItemScript itemScript = itemObject.GetComponent<InventoryItemScript>();
            itemObject.name = "categoryItem_" + type.id;
            itemObject.transform.localPosition = new Vector2(offsetX, -offsetY);
            itemObject.transform.SetParent(parent, false);
            ServerModel.InventoryItem item = service.GetInventoryItem(type.prefab);
            itemScript.SetItem(item);
            //itemScript.SetOffset(offsetX, offsetY);
            return (item != null);
        }

        private void Init()
        {
            Game.Instance.GetPlayer().GetInventoryService().ResetNew();
            categoryOffsetY = 0f;
            categoriesObjects.Clear();
            items = service.GetInventory().items;
            categories = service.GetInventory().categories;
            GameObjectUtility.DestroyAllChildObjects(inventoryContent);
        }

        private IEnumerator DelayedPositioning()
        {
            yield return new WaitForEndOfFrame();
            foreach (GameObject category in categoriesObjects)
            {
                category.transform.localPosition = new Vector2(0, -categoryOffsetY);
                categoryOffsetY += (category.transform.Find("CategoryContent").GetComponent<RectTransform>().rect.height + ITEM_SIZE); // - header
            }
            inventoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, categoryOffsetY);
        }

        public void OnItemSelected(GameObject item)
        {

        }
    }
}
