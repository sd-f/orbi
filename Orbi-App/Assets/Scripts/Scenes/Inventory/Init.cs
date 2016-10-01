using UnityEngine;
using GameController;
using UnityEngine.SceneManagement;
using System.Collections;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/Init")]
    class Init : MonoBehaviour
    {
        public GameObject canvasGameObject;
        public static float OBJECT_PADDING = 10f;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // screen always awake
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            StartCoroutine(LoadInventory());
        }

        IEnumerator LoadInventory()
        {
            yield return Game.GetPlayer().LoadInventory();

            GameObject parent = GameObject.Find("InventoryGameObjects");
            GameObjectUtility.DestroyAllChildObjects(parent);

            Canvas canvasScript = canvasGameObject.GetComponent<Canvas>();

            int id = 0;
            int preselected = 0;
            // create inventory objects
            foreach (ServerModel.InventoryItem item in Game.GetPlayer().GetCraftingController().GetInventory().items) {
                canvasScript.GetObjectsList().Add(id, item);
                if (item.prefab == Game.GetPlayer().GetCraftingController().GetSelectedPrefab())
                    preselected = id;
                GameObject newObject = GameObjectFactory.CreateObject(parent.transform, item.prefab, id, "inventoryItem_" + id, null, LayerMask.NameToLayer("Default"));
                GameObjectUtility.Freeze(newObject);
                newObject.transform.localPosition = new Vector3(id * OBJECT_PADDING, 0f, 0f);
                id++;
            }
            canvasScript.SetSelected(preselected);

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
