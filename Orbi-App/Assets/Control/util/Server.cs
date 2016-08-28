using Assets.Model;

namespace Assets.Control.util
{
    class Server
    {
        public static int RUNNING_REQUESTS = 0;

        
        

        public static bool RequestsRunning()
        {
            return (RUNNING_REQUESTS > 0);
        }

        
    }

    
}
