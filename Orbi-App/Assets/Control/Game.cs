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

        public Game()
        {
            player.geoPosition = ServiceConstants.START_POSITION;
        }

        public void SetServer(ServerType serverType)
        {
            INSTANCE.server = serverType;
        }

        public string GetServerUrl()
        {
            return ServiceConstants.GetServerUrl(INSTANCE.server);
        }

        public static Game GetInstance()
        {
            return INSTANCE;
        }

    }
}
