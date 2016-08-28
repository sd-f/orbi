using Assets.Control.util;
using UnityEngine;

namespace GameController
{

    [AddComponentMenu("App/Game/Client")]
    public class Client : MonoBehaviour
    {

        // prod server, dev server or localhost
        public static ServerType SERVER_TYPE = ServerType.PROD;
        public static int VERSION = 2;

        private int runningRequests = 0;

        public void IncRunningRequests()
        {
            this.runningRequests++;
        }

        public void DecRunningRequests()
        {
            if (this.runningRequests > 0)
                this.runningRequests--;
        }

        public bool IsRequestRunning()
        {
            return (this.runningRequests > 0);
        }

    }

}
