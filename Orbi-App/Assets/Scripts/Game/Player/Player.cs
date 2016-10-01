using GameController.Services;
using GameScene;
using System;
using System.Collections;
using UnityEngine;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
        private PlayerService playerService = new PlayerService();
        private Vector3 positionBeforeOutOfBounds = new Vector3(0, 0, 0);
        private Quaternion rotationBeforeOutOfBounds = Quaternion.Euler(new Vector3(0,0,0)); 
        private bool loggedIn = false;
        private ServerModel.Player player = new ServerModel.Player();
        private bool frozen = true;
        
        private CraftingController craftingController = new CraftingController();
        private DestructionController destructionController = new DestructionController();

        public static float HEIGHT = 3.0f;

        void Start()
        {
            player.geoPosition = Game.FALLBACK_START_POSITION;
        }

        void Awake()
        {
            InvokeRepeating("CheckIfOutOfBounds", 0, 0.5f);
            InvokeRepeating("CheckGPSPosition", 0, 1f);
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

        internal void RestoreRotation()
        {
            if (GetPlayerBody() != null)
            {
                GetPlayerBody().transform.rotation = rotationBeforeOutOfBounds;
            }
        }

        public Vector3 GetPositionBeforeOutOfBounds()
        {
            return this.positionBeforeOutOfBounds;
        }

        public void CheckGPSPosition()
        {
            if (GetPlayerBody() != null)
            {
                //Info.Show("debug check gps");
                if (!frozen)
                {
                    this.player.geoPosition = Game.GetLocation().GetGeoLocation();
                    GetPlayerBodyController().SetTargetPosition(this.player.geoPosition.ToPosition().ToVector3());
                }
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
                    this.rotationBeforeOutOfBounds = GetPlayerBody().transform.rotation;
                    this.positionBeforeOutOfBounds = GetPlayerBody().transform.position;
                    
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

        public void SetLoggedIn(Boolean loggedIn)
        {
            this.loggedIn = loggedIn;
        }

        public Boolean IsLoggedIn()
        {
            return this.loggedIn;
        }

        public IEnumerator LoadInventory()
        {
            yield return Game.GetPlayer().GetPlayerService().RequestInventory();
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}