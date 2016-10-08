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
        private PlayerService playerService = new PlayerService();
        private MessageService messageService = new MessageService();
        private Vector3 positionBeforeOutOfBounds = new Vector3(0, 0, 0);
        private Quaternion rotationBeforeOutOfBounds = Quaternion.Euler(new Vector3(0,0,0)); 
        private bool loggedIn = false;
        private ServerModel.Player player = new ServerModel.Player();
        private bool frozen = true;
        
        private CraftingController craftingController = new CraftingController();
        private DestructionController destructionController = new DestructionController();

        private List<CharacterMessage> messages = new List<CharacterMessage>();

        public static float HEIGHT = 0.8f;

        void Start()
        {
            player.character.transform.geoPosition = Game.FALLBACK_START_POSITION;
        }

        void Awake()
        {
            Invoke("CheckIfOutOfBounds", 0.5f);
            Invoke("CheckGPSPosition", 1f);
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

        public ServerModel.Player GetModel()
        {
            return this.player;
        }

        public void SetModel(ServerModel.Player model)
        {
            this.player = model;
        }

        internal void RestoreRotation()
        {
            if (GetPlayerBody() != null)
                GetPlayerBody().transform.rotation = rotationBeforeOutOfBounds;
        }

        public Vector3 GetPositionBeforeOutOfBounds()
        {
            return this.positionBeforeOutOfBounds;
        }

        public void CheckGPSPosition()
        {
            if (GetPlayerBody() != null)
                //Info.Show("debug check gps");
                if (!frozen)
                {
                    this.player.character.transform.geoPosition = Game.GetLocation().GetGeoLocation();
                    this.player.character.transform.rotation = new Rotation(GetPlayerBodyController().transform.rotation);
                    GetPlayerBodyController().SetTargetPosition(this.player.character.transform.geoPosition.ToPosition().ToVector3());
                }
            Invoke("CheckGPSPosition", 2f);
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
                    this.rotationBeforeOutOfBounds = GetPlayerBody().transform.rotation;
                    this.positionBeforeOutOfBounds = GetPlayerBody().transform.position;
                    Game.GetWorld().SetCenterGeoPosition(new Position(Game.GetPlayer().GetPositionBeforeOutOfBounds()).ToGeoPosition());
                    Game.GetPlayer().GetModel().character.transform.position = new Position();
                    Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
                }
                
            }
            Invoke("CheckIfOutOfBounds", 0.5f);
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

        public List<CharacterMessage> GetMessages()
        {
            return this.messages;
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}