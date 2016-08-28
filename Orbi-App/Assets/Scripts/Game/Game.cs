using GameController.Services;
using UnityEngine;

namespace GameController
{

    [AddComponentMenu("App/Game/Game")]
    class Game : MonoBehaviour
    {

        // holding all scene persistent objects
        private static Game GAME;
        private static Client CLIENT;
        private static Player PLAYER;
        private static World WORLD;
        private Ui ui = new Ui();
        private ServerService serverService = new ServerService();

        // settings
        private Settings settings = new Settings();

        void Start()
        {
            DontDestroyOnLoad(gameObject);
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

        private static Game LoadAndGetGame()
        {
            if (GAME == null)
                GAME = GameObject.Find("Game").GetComponent<Game>();
            return GAME;
        }

        private static Client LoadAndGetClient()
        {
            if (CLIENT == null)
                CLIENT = LoadAndGetGame().transform.Find("Client").GetComponent<Client>();
            return CLIENT;
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

    }

}
