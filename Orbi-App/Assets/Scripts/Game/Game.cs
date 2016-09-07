using GameController.Services;
using ServerModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{

    [AddComponentMenu("App/Game/Game")]
    class Game : MonoBehaviour
    {
        public static GeoPosition FALLBACK_START_POSITION; // Schlossberg, Graz, Austria

        // holding all scene persistent objects
        private static Game GAME;
        private static Client CLIENT;
        private static Player PLAYER;
        private static World WORLD;
        private static Location LOCATION;
        private Ui ui = new Ui();
        private ServerService serverService;

        // settings
        private Settings settings = new Settings();

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            serverService = new ServerService();
            FALLBACK_START_POSITION = new GeoPosition(47.073158d, 15.438000d, 0.0d); // schlossberg
            // FALLBACK_START_POSITION = new GeoPosition(31.635890d, -8.012014d, 0.0d); // marakesh
        }

        public ServerService GetServerService()
        {
            return this.serverService;
        }

        public Ui GetUi()
        {
            return this.ui;
        }

        public Settings GetSettings()
        {
            return this.settings;
        }

        public static Game GetGame()
        {
            return LoadAndGetGame().GetComponent<Game>();
        }

        public static Client GetClient()
        {
            return LoadAndGetClient();
        }

        public static Player GetPlayer()
        {
            return LoadAndGetPlayer();
        }

        public static World GetWorld()
        {
            return LoadAndGetWorld();
        }

        public static Location GetLocation()
        {
            return LoadAndGetLocation();
        }

        private static Game LoadAndGetGame()
        {
            if (GAME == null)
            {
                GAME = UnityEngine.GameObject.Find("Game").GetComponent<Game>();
            }
                
            return GAME;
        }

        private static Client LoadAndGetClient()
        {
            if (CLIENT == null)
                CLIENT = LoadAndGetGame().transform.Find("Client").GetComponent<Client>();
            return CLIENT;
        }

        private static Location LoadAndGetLocation()
        {
            if (LOCATION == null)
                LOCATION = LoadAndGetGame().transform.Find("Location").GetComponent<Location>();
            return LOCATION;
        }

        private static Player LoadAndGetPlayer()
        {
            if (PLAYER == null)
                PLAYER = LoadAndGetGame().transform.Find("Player").GetComponent<Player>();
            return PLAYER;
        }

        private static World LoadAndGetWorld()
        {
            if (WORLD == null)
                WORLD = LoadAndGetGame().transform.Find("World").GetComponent<World>();
            return WORLD;
        }

        public void Quit()
        {
            CancelInvoke();
            Application.Quit();
        }

    }

}
