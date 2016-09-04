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

        public void AdjustHeight()
        {
            if (UnityEngine.GameObject.Find("PlayerCamera") != null)
            {
                UnityEngine.GameObject.Find("PlayerCamera").SendMessage("AdjustHeight");
            }
        }

        public void CheckGPSPosition()
        {
            if (UnityEngine.GameObject.Find("PlayerCamera") != null)
            {
                if (!Game.GetLocation().GetGeoLocation().Equals(geoPosition))
                {
                    this.geoPosition = Game.GetLocation().GetGeoLocation();
                    GetPlayerCamera().MoveToPosition(this.geoPosition.ToPosition().ToVector3());
                    
                    //Debug.Log("Player gps update " + this.geoPosition.ToPosition());
                }
            }
        }

        void CheckIfOutOfBounds()
        {
            if (GetCamera() != null)
            {
                Vector3 playerPosition = GetCamera().transform.position;
                // 50 meter radius
                if ((playerPosition.x > 128)
                    || (playerPosition.x < -128)
                    || (playerPosition.z > 128)
                    || (playerPosition.z < -128))
                {
                    SceneManager.LoadScene("LoadingScene");
                }
                
            }
        }

        public PlayerCamera GetPlayerCamera()
        {
            return GetCamera().GetComponent<PlayerCamera>();
        }

        public UnityEngine.GameObject GetCamera()
        {
            return UnityEngine.GameObject.Find("PlayerCamera");
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