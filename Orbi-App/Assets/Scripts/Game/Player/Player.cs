using CanvasUtility;
using ClientModel;
using GameController.Services;
using GameScene;
using ServerModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
#pragma warning disable 0649
        public PlayerService playerService;
        public MessageService messageService;
        public InventoryService inventoryService;
        private bool loggedIn = false;
        private ServerModel.Player player = new ServerModel.Player();
        private bool frozen = true;
        
        private CraftingController craftingController = new CraftingController();
        private DestructionController destructionController = new DestructionController();


        public static float HEIGHT = 0.9f;

        void Start()
        {
            player.character.transform.geoPosition = Game.FALLBACK_START_POSITION;
        }

        void Awake()
        {
            if (!IsInvoking("CheckGPSPosition"))
                InvokeRepeating("CheckGPSPosition", 1f, 1f);
            if (!IsInvoking("CheckIfOutOfBounds"))
                InvokeRepeating("CheckIfOutOfBounds", 1f, 1f);
            if (!IsInvoking("CheckForMessages"))
                Invoke("CheckForMessages", 3f);
            if (!IsInvoking("CheckInventory"))
                Invoke("CheckInventory", 0f);
        }

        internal bool IsFrozen()
        {
            return this.frozen;
        }

        public void Freeze()
        {
            this.frozen = true;
        }

        public void Unfreeze()
        {
            this.frozen = false;
        }

        public CraftingController GetCraftingController()
        {
            return this.craftingController;
        }

        public DestructionController GetDestructionController()
        {
            return this.destructionController;
        }

        public InventoryService GetInventoryService()
        {
            return this.inventoryService;
        }

        public ServerModel.Player GetModel()
        {
            return this.player;
        }

        public void SetModel(ServerModel.Player model)
        {
            this.player = model;
        }

        internal void SavePlayerTransform()
        {
            if (GetPlayerBody() != null)
                GetPlayerBodyController().UpdateTransformInModel();
        }


        public void CheckGPSPosition()
        {
            if ((GetPlayerBody() != null) && !Game.Instance.GetSettings().IsDesktopInputEnabled() && !IsFrozen()) {
                this.player.character.transform.geoPosition = Game.Instance.GetLocation().GetGeoLocation();
                this.player.character.transform.rotation = new Rotation(GetPlayerBodyController().transform.rotation);
                Vector3 target = this.player.character.transform.geoPosition.ToPosition().ToVector3();
                WorldAdapter.VERBOSE = true;
                Game.Instance.GetClient().Log("CheckGPSPosition-pos " + target);
                WorldAdapter.VERBOSE = false;
                GetPlayerBodyController().SetTargetPosition(target);
            }
        }

        void CheckIfOutOfBounds()
        {
            if (GetPlayerBody() != null)
            {
                Vector3 playerPosition = GetPlayerBody().transform.position;
                // 50 meter radius
                if ((playerPosition.x > 120)
                    || (playerPosition.x < -120)
                    || (playerPosition.z > 120)
                    || (playerPosition.z < -120))
                {
                    Freeze();
                    SavePlayerTransform();
                    Game.Instance.GetWorld().SetCenterGeoPosition(Game.Instance.GetPlayer().GetModel().character.transform.position.ToGeoPosition());
                    Game.Instance.GetPlayer().GetModel().character.transform.position = new Position();
                    GetPlayerBodyController().ResetPosition();
                    //Game.Instance.LoadScene(Game.GameScene.LoadingScene);
                    // TODO
                }
                
            }
        }

        void CheckInventory()
        {
            StartCoroutine(LoadInventory());
        }

        IEnumerator LoadInventory()
        {
            yield return Game.Instance.GetPlayer().GetInventoryService().RequestInventory();
            if (!IsInvoking("CheckInventory"))
                Invoke("CheckInventory", 5f);
        }


        void CheckForMessages()
        {
            StartCoroutine(LoadMessages());
        }

        IEnumerator LoadMessages()
        {
            yield return Game.Instance.GetPlayer().GetMessageService().RequestMessages();
            if (!IsInvoking("CheckForMessages"))
                Invoke("CheckForMessages", 5f);
        }

        public PlayerBodyController GetPlayerBodyController()
        {
            return GetPlayerBody().GetComponent<PlayerBodyController>();
        }

        public UnityEngine.GameObject GetPlayerBody()
        {
            return UnityEngine.GameObject.Find("PlayerBody");
        }

        public PlayerService GetPlayerService()
        {
            return playerService;
        }

        public MessageService GetMessageService()
        {
            return messageService;
        }

        public void SetLoggedIn(Boolean loggedIn)
        {
            this.loggedIn = loggedIn;
        }

        public Boolean IsLoggedIn()
        {
            return this.loggedIn;
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}