using UnityEngine;
using GameController;
using System.Collections;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/PlayerConstructionController")]
    class PlayerConstructionController : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject mainCamera;
        public GameObject effectPrefab;
        public GameObject earnedXPTextPrefab;
        public Camera cam;
        private bool isDesktopMode = false;
        private bool crafting = false;
        private GameObject newObject;
        private Vector3 rotation = new Vector3(0, 0, 0);
        private float distance = 10f;
        private Vector3 firstpoint;
        private Vector3 secondpoint;

        void Start()
        {
            Game.GetPlayer().GetCraftingController().SetCrafting(false, null);
        }

        void Awake ()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }

        void Update()
        {
            if (crafting)
            {
                if (isDesktopMode)
                    desktopMovement();
                else
                    mobileMovement();
                newObject.transform.position = Vector3.Lerp(newObject.transform.position, 
                    CheckFloor(transform.position), Time.deltaTime * 20f);
                //newObject.transform.localPosition = Vector3.Lerp(newObject.transform.position, CheckFloor(transform.position), Time.deltaTime * 20f);
                //body.MovePosition(transform.position);
                //body.MoveRotation(Quaternion.Slerp(newObject.transform.rotation, Quaternion.Euler(rotation), Time.deltaTime));
                newObject.transform.rotation = Quaternion.Slerp(newObject.transform.rotation, Quaternion.Euler(rotation) , Time.deltaTime * 20f);
                transform.localPosition = new Vector3(0, 0, distance);
            }
            
            
        }

        private void mobileMovement()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    firstpoint = Input.GetTouch(0).position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    secondpoint = Input.GetTouch(0).position;
                    rotation.y += (secondpoint.x - firstpoint.x) / Screen.width * 45.0f;
                    float newDistance = (secondpoint.y - firstpoint.y) / Screen.height;
                    if (((newDistance < 0f) && (distance >= 5f)) || ((newDistance > 0f) && (distance <= 50f)))
                        distance += newDistance;
                }
            }
        }

        private void desktopMovement()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                rotation.y = rotation.y - 10f;
            if (Input.GetKeyDown(KeyCode.E))
                rotation.y = rotation.y + 10f;
            // object distance
            float d = Input.GetAxis("Mouse ScrollWheel");
            if ((d > 0f) && (distance <= 50f))
                distance += 0.25f;
            else if ((d < 0f) && (distance >= 5f))
                distance -= 0.25f;
        }

        public Vector3 CheckFloor(Vector3 newPosition)
        {
            float height = Game.GetWorld().GetMinHeightForObject(newObject);
            return new Vector3(newPosition.x, height, newPosition.z);
        }

        public void StartCrafting()
        {
            Game.GetPlayer().Freeze();
            CreateObjectToCraft();
            transform.localPosition = new Vector3(0,0,distance);
            newObject.transform.position = CheckFloor(transform.position);
            Game.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
            this.crafting = true;
        }

        public void StopCrafting()
        {
            Game.GetPlayer().Unfreeze();
            this.crafting = false;
            Game.GetPlayer().GetCraftingController().SetCrafting(false, null);
            CleanUp();
        }

        public void Craft()
        {
            StartCoroutine(CraftingProcess());
        }

        IEnumerator CraftingProcess()
        {
            GameObject effect = GameObject.Instantiate(effectPrefab) as GameObject;
            GameObject earnedText = GameObject.Instantiate(earnedXPTextPrefab) as GameObject;
            earnedText.GetComponent<XPEarnedText>().SetAmount(ServerModel.CharacterDevelopment.XP_CRAFT);
            earnedText.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            effect.transform.position = newObject.transform.position;
            earnedText.transform.position = newObject.transform.position + (Vector3.up * 2f);
            GameObject.Destroy(effect, 1.5f);
            yield return Game.GetPlayer().GetCraftingController().Craft(newObject);
            
            CleanUp();
            Game.GetPlayer().Unfreeze();
        }


        private void CreateObjectToCraft()
        {
            checkInventory();
            newObject = GameObjectFactory.CreateObject(transform, Game.GetPlayer().GetCraftingController().GetSelectedPrefab(), -1, null, LayerMask.NameToLayer("Default"));
            if (!String.IsNullOrEmpty(Game.GetPlayer().GetCraftingController().GetUserText()))
                GameObjectUtility.TrySettingTextInChildren(newObject, Game.GetPlayer().GetCraftingController().GetUserText());
            newObject.transform.rotation = Quaternion.Euler(rotation);
            Game.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
        }

        void checkInventory()
        {
            if (!Game.GetPlayer().GetCraftingController().HasInventoryItem(Game.GetPlayer().GetCraftingController().GetSelectedPrefab())) {
                Game.GetPlayer().GetCraftingController().SetSelectedPrefab(Game.GetPlayer().GetCraftingController().GetNextAvailableItem().prefab);
            }
        }

        private void CleanUp()
        {
            GameObjectUtility.DestroyAllChildObjects(this.gameObject);
        }

    }
}