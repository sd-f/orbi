using GameController.Services;
using GameScene;
using ServerModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        private AuthService authService;
        private ServerService serverService;
        private GameScene currentScene = GameScene.StartScene;

        private bool typingMode = false;

        // settings
        private Settings settings = new Settings();

        public enum GameScene {
            SettingsScene,
            GameScene,
            StartScene,
            AuthorizationScene,
            LoadingScene,
            InventoryScene
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            serverService = new ServerService();
            authService = new AuthService();
            if (Game.GetClient().serverType == ServerType.LOCAL || Game.GetClient().serverType == ServerType.DEV)
                FALLBACK_START_POSITION = new GeoPosition(47.0678d, 15.5552d, 0.0d);
            else
                FALLBACK_START_POSITION = new GeoPosition(47.073158d, 15.438000d, 0.0d); // schlossberg
            // FALLBACK_START_POSITION = new GeoPosition(31.635890d, -8.012014d, 0.0d); // marakesh
            //FALLBACK_START_POSITION = new GeoPosition(47.0678d, 15.5552d, 0.0d); // lahö
        }

        public void LoadScene(GameScene scene)
        {
            if (currentScene == GameScene.GameScene)
            {
                //UnityEngine.GameObject.Find("ButtonSwitchView").GetComponent<ViewSwitcher>().StopWebCam();
                Game.GetPlayer().SavePlayerTransform();
            }
            currentScene = scene;
            SceneManager.LoadScene(scene.ToString());
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

        public void EnterTypingMode()
        {
            this.typingMode = true;
        }

        public void LeaveTypingMode()
        {
            this.typingMode = false;
        }

        public bool IsInTypingMode()
        {
            return typingMode;
        }

        public AuthService GetAuthService()
        {
            return authService;
        }

        public void Quit()
        {
            CancelInvoke();
            Application.Quit();
        }

    }

}
