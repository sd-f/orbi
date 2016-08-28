using GameController.Services;
using ServerModel;
using UnityEngine;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
        private static GeoPosition START_POSITION = new GeoPosition(47.073158d, 15.438000d, 2.0d); // schlossberg
        private GeoPosition geoPosition;
        private PlayerService playerService = new PlayerService();
        private AuthService authService = new AuthService();

        public Player()
        {
            this.geoPosition = START_POSITION;
        }

        public PlayerService GetPlayerService()
        {
            return playerService;
        }

        public AuthService GetAuthService()
        {
            return authService;
        }

        public ServerModel.Player ToServerModel()
        {
            ServerModel.Player playerModel = new ServerModel.Player();
            playerModel.geoPosition = geoPosition;
            return playerModel;
        }

    }
}