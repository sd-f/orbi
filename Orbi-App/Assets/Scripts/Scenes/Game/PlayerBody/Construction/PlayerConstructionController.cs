using UnityEngine;
using GameController;
using System.Collections;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/PlayerConstructionController")]
    class PlayerConstructionController : InputModeMonoBehaviour
    {
#pragma warning disable 0649
        public GameObject mainCamera;
        public GameObject effectPrefab;
        public GameObject earnedXPTextPrefab;
        public Camera cam;
        private bool crafting = false;
        private GameObject newObject;
        private Vector3 rotation = new Vector3(0, 0, 0);
        private float distance = 10f;
        private Vector3 firstpoint;
        private Vector3 secondpoint;

        void Start()
        {
            Game.Instance.GetPlayer().GetCraftingController().SetCrafting(false, null);
        }

        void Update()
        {
            if (crafting)
            {
                if (desktopMode)
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
            float height = Game.Instance.GetWorld().GetMinHeightForObject(newObject);
            return new Vector3(newPosition.x, height, newPosition.z);
        }

        public void StartCrafting()
        {
            Game.Instance.GetPlayer().Freeze();
            CreateObjectToCraft();
            transform.localPosition = new Vector3(0,0,distance);
            newObject.transform.position = CheckFloor(transform.position);
            Game.Instance.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
            this.crafting = true;
        }

        public void StopCrafting()
        {
            Game.Instance.GetPlayer().Unfreeze();
            this.crafting = false;
            Game.Instance.GetPlayer().GetCraftingController().SetCrafting(false, null);
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
            effect.transform.localScale = effect.transform.localScale * GameObjectUtility.GetMaxSize(newObject);
            earnedText.transform.position = newObject.transform.position + (Vector3.up * 2f);
            GameObject.Destroy(effect, 3f);
            yield return Game.Instance.GetPlayer().GetCraftingController().Craft(newObject);
            
            CleanUp();
            Game.Instance.GetPlayer().Unfreeze();
        }


        private void CreateObjectToCraft()
        {
            checkInventory();
            newObject = GameObjectFactory.CreateObject(transform, Game.Instance.GetPlayer().GetCraftingController().GetSelectedType().prefab, -1, null, LayerMask.NameToLayer("Default"));
            if (!String.IsNullOrEmpty(Game.Instance.GetPlayer().GetCraftingController().GetUserText()))
                GameObjectUtility.TrySettingTextInChildren(newObject, Game.Instance.GetPlayer().GetCraftingController().GetUserText());
            newObject.transform.rotation = Quaternion.Euler(rotation);
            Game.Instance.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
        }

        void checkInventory()
        {
            if (!Game.Instance.GetPlayer().GetInventoryService().HasInventoryItem(Game.Instance.GetPlayer().GetCraftingController().GetSelectedType().prefab)) {
                Game.Instance.GetPlayer().GetCraftingController().SetSelectedType(Game.Instance.GetPlayer().GetInventoryService().GetNextAvailableItem().type);
            }
        }

        private void CleanUp()
        {
            GameObjectUtility.DestroyAllChildObjects(this.gameObject);
        }

    }
}