using GameController.Services;
using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{

    [AddComponentMenu("App/Game/Game")]
    class Game : MonoBehaviour
    {
        public delegate void OnReadyEventHandler();
        public static event OnReadyEventHandler OnReady;


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

        public Game()
        {

        }

        public enum GameScene {
            GameScene,
            AuthorizationScene
        }

        private void SendOnReadyEvent()
        {
            // Send Event
            if (OnReady != null)
            {
                OnReady();
            }
            player.gameObject.SetActive(true);
        }


        void Awake()
        {
            //DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this as Game;
            }
            /*
           
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
            */
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
                    // UnityEngine.GameObject.Find("ButtonSwitchView").GetComponent<ViewSwitcher>().StopWebCam();
                    // Game.Instance.GetPlayer().SavePlayerTransform();
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
            SendOnReadyEvent();
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
