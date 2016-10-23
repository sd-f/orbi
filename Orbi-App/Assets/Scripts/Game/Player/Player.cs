﻿using CanvasUtility;
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
        private bool loggedIn = false;
        private ServerModel.Player player = new ServerModel.Player();
        private bool frozen = true;
        
        private CraftingController craftingController = new CraftingController();
        private DestructionController destructionController = new DestructionController();

        private List<CharacterMessage> messages = new List<CharacterMessage>();

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

        internal void SavePlayerTransform()
        {
            if (GetPlayerBody() != null)
                GetPlayerBodyController().UpdateTransformInModel();
        }


        public void CheckGPSPosition()
        {
            if ((GetPlayerBody() != null) && !Game.GetGame().GetSettings().IsDesktopInputEnabled() && !IsFrozen()) {
                this.player.character.transform.geoPosition = Game.GetLocation().GetGeoLocation();
                this.player.character.transform.rotation = new Rotation(GetPlayerBodyController().transform.rotation);
                Vector3 target = this.player.character.transform.geoPosition.ToPosition().ToVector3();
                WorldAdapter.VERBOSE = true;
                //Game.GetClient().Log("CheckGPSPosition-pos " + target);
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
                    Game.GetWorld().SetCenterGeoPosition(Game.GetPlayer().GetModel().character.transform.position.ToGeoPosition());
                    Game.GetPlayer().GetModel().character.transform.position = new Position();
                    GetPlayerBodyController().ResetPosition();
                    Game.GetGame().LoadScene(Game.GameScene.LoadingScene);
                }
                
            }
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