using GameController.Services;
using ServerModel;
using System;
using UnityEngine;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
        private GeoPosition geoPosition = Game.FALLBACK_START_POSITION;
        private PlayerService playerService = new PlayerService();
        private AuthService authService = new AuthService();
        private Boolean loggedIn = false;

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

    }
}