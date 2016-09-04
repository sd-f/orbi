using Assets.Control;
using Assets.Control.services;
using Assets.Control.util;
using Assets.Model;
using GameController;
using UnityEngine;

namespace Assets.Control
{
    class Game
    {
        private static Game INSTANCE = new Game();
        private bool worldUpdatePaused = false;


        public Model.Player player = new Model.Player();
        public Model.World world = new Model.World();
        public ServerType server = ServerType.LOCAL;

        private Assets.Control.services.WorldAdapter adapter;
        private services.TerrainService terrainService;
        private GameObjectsService gameObjectsService = new GameObjectsService();
        private PlayerService playerService = new PlayerService();
        private GoogleMapsService googleMapsService = new GoogleMapsService();

        private string craftPrefab = GameObjectTypes.DEFAULT;
        private bool heightsEnabled = false;
        private bool locationReady = false;

        public Game()
        {
            //player.geoPosition = Assets.Control.util.Server.START_POSITION;
        }

        public void Reset()
        {
            gameObjectsService = new GameObjectsService();
            playerService = new PlayerService();
            googleMapsService = new GoogleMapsService();
        }

        public static Game GetInstance()
        {
            return INSTANCE;
        }

        public void InitTerrain(Terrain terrain)
        {
            terrainService = new services.TerrainService(terrain);
            adapter = new Assets.Control.services.WorldAdapter(terrainService);
        }

        public void SetServer(ServerType serverType)
        {
            INSTANCE.server = serverType;
        }

        public void SetHeightsEnabled(bool enabled)
        {
            heightsEnabled = enabled;
        }

        public bool IsHeightsEnabled()
        {
            return heightsEnabled;
        }

        public void SetLocationReady(bool ready)
        {
            locationReady = ready;
        }

        public bool IsLocationReady()
        {
            return locationReady;
        }

        public string GetServerUrl()
        {
            return ServerConstants.GetServerUrl(INSTANCE.server);
        }

        public void SetCraftPrefab(string prefab)
        {
            this.craftPrefab = prefab;
        }

        public string GetCraftPrefab()
        {
            return this.craftPrefab;
        }

        public services.TerrainService GetTerrainService()
        {
            return this.terrainService;
        }

        public PlayerService GetPlayerService()
        {
            return this.playerService;
        }

        public GoogleMapsService GetGoogleMapsService()
        {
            return this.googleMapsService;
        }

        public GameObjectsService GetGameObjectsService()
        {
            return this.gameObjectsService;
        }

        public Assets.Control.services.WorldAdapter GetAdapter()
        {
            return this.adapter;
        }

        public bool IsWorldUpdatedPaused()
        {
            return this.worldUpdatePaused;
        }

        public void SetWorldUpdatePaused(bool worldUpdatePaused)
        {
            this.worldUpdatePaused = worldUpdatePaused;
        }

    }
}
