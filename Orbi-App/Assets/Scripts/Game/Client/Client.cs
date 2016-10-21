﻿using GameController.Services;
using UnityEngine;

namespace GameController
{

    [AddComponentMenu("App/Game/Client")]
    class Client : MonoBehaviour
    {

        // prod server, dev server or localhost
        public ServerType serverType = ServerType.LOCAL;
        public static int VERSION = 20;
        //private AuthService authService = new AuthService();
        private int runningRequests = 0;
        public bool verbose = false;
        public bool randomLocation = false;

        public void Log(object objectToLog)
        {
            Log(objectToLog, this);
        }

        public void Log(object objectToLog, Object obj)
        {
            if (verbose)
                Debug.Log(objectToLog, obj);
        }

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
