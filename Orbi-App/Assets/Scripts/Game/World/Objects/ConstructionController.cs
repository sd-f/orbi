using UnityEngine;
using GameController;
using System.Collections;
using System;
using ServerModel;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/ConstructionController")]
    class ConstructionController : GameMonoBehaviour
    {
#pragma warning disable 0649
        public UnityEngine.GameObject mainCamera;
        public UnityEngine.GameObject processingEffect;
        public UnityEngine.GameObject earnedXPTextPrefab;
        public Camera cam;
        private bool crafting = false;
        private UnityEngine.GameObject newObject;
        private Vector3 rotation = new Vector3(0, 0, 0);
        private float distance = 10f;
        private Vector3 firstpoint;
        private Vector3 secondpoint;
        private bool collission = false;
        private GameObjectType selectedType;
        private string userText = "";
        private UnityEngine.GameObject objectToCraft;
        private UnityEngine.GameObject processingEffectGameObject;

        public override void Start()
        {
            base.Start();
            SetCrafting(false, null);
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
            SetCrafting(true, newObject);
        }

        public void StopCrafting()
        {
            Game.Instance.GetPlayer().Unfreeze();
            SetCrafting(false, null);
            CleanUp();
        }

        public void Craft()
        {
            StartCoroutine(CraftingProcess());
        }

        IEnumerator CraftingProcess()
        {
            processingEffectGameObject = UnityEngine.GameObject.Instantiate(processingEffect) as UnityEngine.GameObject;
            UnityEngine.GameObject earnedText = UnityEngine.GameObject.Instantiate(earnedXPTextPrefab) as UnityEngine.GameObject;
            earnedText.GetComponent<XPEarnedText>().SetAmount(ServerModel.CharacterDevelopment.XP_CRAFT);
            earnedText.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            processingEffectGameObject.transform.position = newObject.transform.position;
            processingEffectGameObject.transform.localScale = processingEffectGameObject.transform.localScale * GameObjectUtility.GetMaxSize(newObject);
            earnedText.transform.position = newObject.transform.position + (Vector3.up * 2f);
            
            yield return Craft(newObject);
            
            CleanUp();
            Game.Instance.GetPlayer().Unfreeze();
        }


        private void CreateObjectToCraft()
        {
            CheckInventory();
            newObject = GameObjectFactory.CreateObject(transform, GetSelectedType().prefab, -1, null, LayerMask.NameToLayer("Default"));
            if (!String.IsNullOrEmpty(GetUserText()))
                GameObjectUtility.TrySettingTextInChildren(newObject, GetUserText());
            newObject.transform.rotation = Quaternion.Euler(rotation);
            SetCrafting(true, newObject);
        }

        public void CheckInventory()
        {
            GameObjectType type = GetSelectedType();
            if ((type == null) || ((type != null) && !Game.Instance.GetPlayer().GetInventoryService().HasInventoryItem(type.prefab))) {
                InventoryItem nextItem = Game.Instance.GetPlayer().GetInventoryService().GetNextAvailableItem();
                if (nextItem != null)
                    SetSelectedType(nextItem.type);
            }
        }

        private void CleanUp()
        {
            if (processingEffectGameObject != null)
            {
                UnityEngine.GameObject.Destroy(processingEffectGameObject, 1);
            }
            GameObjectUtility.DestroyAllChildObjects(this.gameObject);
        }

        public void SetCrafting(Boolean crafting, UnityEngine.GameObject objectToCraft)
        {
            this.objectToCraft = objectToCraft;
            this.crafting = crafting;
        }

        public Boolean IsCrafting()
        {
            return this.crafting;
        }

        public UnityEngine.GameObject GetObjectToCraft()
        {
            return this.objectToCraft;
        }

        public GameObjectType GetSelectedType()
        {
            return this.selectedType;
        }

        public void SetSelectedType(GameObjectType type)
        {
            this.selectedType = type;
        }



        public void SetColliding(bool collission)
        {
            this.collission = collission;
        }

        public bool IsColliding()
        {
            return this.collission;
        }

        public string GetUserText()
        {
            return this.userText;
        }

        public void SetUserText(string text)
        {
            if ((text != null) && (text.Length > 16))
                this.userText = text.Substring(0, 15);
            else
                this.userText = text;
        }



        public IEnumerator Craft(UnityEngine.GameObject gameObject)
        {
            ServerModel.GameObject newObject = new ServerModel.GameObject();
            newObject.id = -1;
            newObject.name = "new";
            newObject.type = GetSelectedType();
            if (GetSelectedType().supportsUserText)
                newObject.userText = GetUserText();
            newObject.transform.rotation = new Rotation(gameObject.transform.rotation.eulerAngles);
            newObject.transform.geoPosition = new ClientModel.Position(gameObject.transform.position).ToGeoPosition();
            newObject.constraints = (int)RigidbodyConstraints.FreezeAll;
            ServerModel.Player player = Game.Instance.GetPlayer().GetModel();
            player.gameObjectToCraft = newObject;
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestCraft(player);
            yield return Game.Instance.GetPlayer().GetInventoryService().RequestInventory();
            UnityEngine.GameObject.Destroy(processingEffectGameObject, 1f);
            yield return Game.Instance.GetWorld().UpdateObjects();
        }

    }
}