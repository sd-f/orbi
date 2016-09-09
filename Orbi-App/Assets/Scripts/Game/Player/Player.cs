using CanvasUtility;
using ClientModel;
using GameController.Services;
using GameScene;
using ServerModel;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
        private GeoPosition geoPosition = Game.FALLBACK_START_POSITION;
        private PlayerService playerService = new PlayerService();
        private Vector3 positionBeforeOutOfBounds = new Vector3(0, 0, 0);
        private Quaternion rotationBeforeOutOfBounds = Quaternion.Euler(new Vector3(0,0,0)); 
        private AuthService authService = new AuthService();
        private Boolean loggedIn = false;
        public static float HEIGHT = 3.0f;

        void Start()
        {
        }

        void Awake()
        {
            InvokeRepeating("CheckIfOutOfBounds", 0, 0.5f);
            InvokeRepeating("CheckGPSPosition", 0, 3f);
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
                if (!Game.GetLocation().GetGeoLocation().Equals(geoPosition))
                {
                    this.geoPosition = Game.GetLocation().GetGeoLocation();
                    GetPlayerBody().transform.position = (this.geoPosition.ToPosition().ToVector3());
                    
                    //Debug.Log("Player gps update " + this.geoPosition.ToPosition());
                }
            }
        }

        void CheckIfOutOfBounds()
        {
            if (GetPlayerBody() != null)
            {
                Vector3 playerPosition = GetPlayerBody().transform.position;
                // 50 meter radius
                if ((playerPosition.x > 128)
                    || (playerPosition.x < -128)
                    || (playerPosition.z > 128)
                    || (playerPosition.z < -128))
                {
                    this.rotationBeforeOutOfBounds = GetPlayerBody().transform.rotation;
                    this.positionBeforeOutOfBounds = GetPlayerBody().transform.position;
                    SceneManager.LoadScene("LoadingScene");
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

        public AuthService GetAuthService()
        {
            return authService;
        }

        public void SetLoggedIn(Boolean loggedIn)
        {
            this.loggedIn = loggedIn;
        }

        public Boolean IsLoggedIn()
        {
            return this.loggedIn;
        }

        public ServerModel.Player ToServerModel()
        {
            ServerModel.Player playerModel = new ServerModel.Player();
            playerModel.geoPosition = geoPosition;
            return playerModel;
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}