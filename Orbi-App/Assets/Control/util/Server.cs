using Assets.Model;

namespace Assets.Control
{
    class Server
    {
        public static int RUNNING_REQUESTS = 0;

        public static string SERVER_URL = "https://softwaredesign.foundation/orbi/api";
        public static string SERVER_DEV_URL = "https://softwaredesign.foundation/orbi-dev/api";
        public static string SERVER_LOCALHOST_URL = "http://localhost:8080/api";
        public static GeoPosition START_POSITION = new GeoPosition(47.0678d, 15.5552d,511.0d);

        public static bool RequestsRunning()
        {
            return (RUNNING_REQUESTS > 0);
        }

        public static string GetServerUrl(ServerType type)
        {
            switch (type)
            {
                case ServerType.PROD:
                    return SERVER_URL;
                case ServerType.DEV:
                    return SERVER_DEV_URL;
                case ServerType.LOCAL:
                    return SERVER_LOCALHOST_URL;
                default:
                    return SERVER_URL;
            }
        }
    }

    public enum ServerType{
        PROD = 0,
        DEV = 1,
        LOCAL = 2
    }

}
