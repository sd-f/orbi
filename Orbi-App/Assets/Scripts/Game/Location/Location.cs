using Assets.Control.util;
using CanvasUtility;
using GameController.Services;
using ServerModel;
using System.Collections;
using UnityEngine;

namespace GameController
{

    [AddComponentMenu("App/Game/Location")]
    class Location : MonoBehaviour
    {
        // Schlossberg, Graz, Austria
        private GeoPosition position;
        private bool paused = false;
        private bool ready = false;

        void Start()
        {
            position = Game.FALLBACK_START_POSITION;
        }

        public void StartUpdatingLocation()
        {
            UpdateLocation();
            Game.GetWorld().SetCenterGeoPosition(position);
            ready = true;
            InvokeRepeating("UpdateLocation", 0.1f, 0.1f);
        }

        public bool IsReady()
        {
            return !paused && ready;
        }

        public GeoPosition GetGeoLocation()
        {
            return this.position;
        }

        public void UpdateLocation(double latitude, double longitude)
        {
            if (!paused)
            {
                position.latitude = latitude;
                position.longitude = longitude;
            }
        }

        void UpdateLocation()
        {
            position.latitude = Input.location.lastData.latitude;
            position.longitude = Input.location.lastData.longitude;
        }

        public void Pause()
        {
            this.paused = true;
        }

        public void Resume()
        {
            this.paused = false;
        }

        void OnDestroy()
        {
            Input.location.Stop();
            CancelInvoke();
        }

        public IEnumerator Boot()
        {
            if (Game.GetClient().serverType == ServerType.LOCAL)
            {
                //Debug.Log(Game.FALLBACK_START_POSITION);
                Game.GetWorld().SetCenterGeoPosition(Game.FALLBACK_START_POSITION);
                Game.GetPlayer().GetModel().geoPosition = Game.FALLBACK_START_POSITION;
                this.ready = true;
                Resume();
                Warning.Show("Location running in LOCAL-mode");
                yield break;
            }

            Input.location.Start();
            

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                //Info.Show("Waiting for location service to start...");
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Error.Show("Waiting for GPS timed out");
                yield return new WaitForSeconds(3);
                Application.Quit();
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Error.Show("Unable to determine device location, GPS error");
                yield return new WaitForSeconds(3);
                Application.Quit();
                yield break;
            }
            Input.compass.enabled = true;
            if (!SystemInfo.supportsGyroscope)
            {
                Warning.Show("No Gyroscope available");
            } else
            {
                Input.gyro.enabled = true;
            }
            Info.Show("Location initialized");
            StartUpdatingLocation();
        }
    }

}
