using Assets.Control;
using Assets.Model;

namespace Assets.Control
{
    class Game
    {
        private static Game INSTANCE = new Game();

        public Player player = new Player();
        public World world = new World();
        public ServerType server = ServerType.LOCAL;
        private bool heightsEnabled = false;
        private bool locationReady = false;

        public Game()
        {
            player.geoPosition = Server.START_POSITION;
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

        public static Game GetInstance()
        {
            return INSTANCE;
        }

    }
}
