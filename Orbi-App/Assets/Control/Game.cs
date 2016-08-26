using Assets.Control;
using Assets.Control.services;
using Assets.Control.util;
using Assets.Model;
using UnityEngine;

namespace Assets.Control
{
    class Game
    {
        private static Game INSTANCE = new Game();

        public Player player = new Player();
        public World world = new World();
        public ServerType server = ServerType.LOCAL;

        private WorldAdapter adapter;
        private TerrainService terrainService;
        private GameObjectsService gameObjectsService = new GameObjectsService();
        private PlayerService playerService = new PlayerService();
        private GoogleMapsService googleMapsService = new GoogleMapsService();

        private string craftPrefab = GameObjectTypes.DEFAULT;
        private bool heightsEnabled = false;
        private bool locationReady = false;

        public Game()
        {
            player.geoPosition = Server.START_POSITION;
        }

        public void ResetServices()
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
            terrainService = new TerrainService(terrain);
            adapter = new WorldAdapter(terrainService);
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
            return Server.GetServerUrl(INSTANCE.server);
        }

        public void SetCraftPrefab(string prefab)
        {
            this.craftPrefab = prefab;
        }

        public string GetCraftPrefab()
        {
            return this.craftPrefab;
        }

        public TerrainService GetTerrainService()
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

        public WorldAdapter GetAdapter()
        {
            return this.adapter;
        }

    }
}
