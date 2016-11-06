using GameController.Services;
using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{

    [AddComponentMenu("App/Game/Game")]
    class Game : MonoBehaviour
    {
        public static ServerModel.GeoPosition FALLBACK_START_POSITION; // Schlossberg, Graz, Austria
        public static Game Instance { get; private set; }

        // holding all scene persistent objects
#pragma warning disable 0649
        public Client client;
        public Player player;
        
        public Location location;
        //public World world;

        public WorldController world;

        public AuthService authService;
        public ServerService serverService;

        public PlayerBodyController playerBodyController;

        public delegate void TypingModeChangedEventHandler();
        public static event TypingModeChangedEventHandler OnTypingModeChanged;

        private Ui ui = new Ui();
        private GameScene currentScene = GameScene.GameScene;

        private bool typingMode = false;
        private bool ready = false;

        // settings
        public Settings settings;

        public enum GameScene {
            GameScene,
            AuthorizationScene
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            if (Game.Instance.GetClient().serverType == ServerType.LOCAL || Game.Instance.GetClient().serverType == ServerType.DEV)
                FALLBACK_START_POSITION = new ServerModel.GeoPosition(47.0678d, 15.5552d, 0.0d);
            else
                FALLBACK_START_POSITION = new ServerModel.GeoPosition(47.073158d, 15.438000d, 0.0d); // schlossberg
            // FALLBACK_START_POSITION = new GeoPosition(31.635890d, -8.012014d, 0.0d); // marakesh
            //FALLBACK_START_POSITION = new GeoPosition(47.0678d, 15.5552d, 0.0d); // lahö
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this as Game;
            }
        }

        private void SendTypingModeChangedEvent()
        {
            // Send Event
            if (OnTypingModeChanged != null)
            {
                OnTypingModeChanged();
            }
        }

        public void LoadScene(GameScene scene)
        {
            if (SceneManager.GetActiveScene().name != scene.ToString()) { 
                if (currentScene == GameScene.GameScene)
                {
                    //UnityEngine.GameObject.Find("ButtonSwitchView").GetComponent<ViewSwitcher>().StopWebCam();
                    //Game.Instance.GetPlayer().SavePlayerTransform();
                }
                currentScene = scene;
                SceneManager.LoadScene(scene.ToString());
            }
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

        public Game GetGame()
        {
            return Instance;
        }

        public Client GetClient()
        {
            return client;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public WorldController GetWorld()
        {
            return world;
        }

        public Location GetLocation()
        {
            return location;
        }

        public void EnterTypingMode()
        {
            this.typingMode = true;
            SendTypingModeChangedEvent();
        }

        public void LeaveTypingMode()
        {
            this.typingMode = false;
            SendTypingModeChangedEvent();
        }

        public bool IsInTypingMode()
        {
            return typingMode;
        }

        public void SetReady(bool ready)
        {
            this.ready = ready;
        }

        public bool IsReady()
        {
            return ready;
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
