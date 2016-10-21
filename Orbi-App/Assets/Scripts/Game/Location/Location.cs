using CanvasUtility;
using GameController.Services;
using ServerModel;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameController
{

    [AddComponentMenu("App/Game/Location")]
    class Location : MonoBehaviour
    {
        // Schlossberg, Graz, Austria
        private GeoPosition position;
        private bool paused = false;
        private bool ready = false;
        private float compassValue = 0.0f;
        private float compassDelta = 0.0f;

        void Start()
        {
            position = Game.FALLBACK_START_POSITION;
            if (Game.GetClient().randomLocation)
                InvokeRepeating("RandomLocation", 10f, 10f);
        }

        void OnAwake()
        {
            Input.location.Start();
        }

        public void StartUpdatingLocation()
        {
            UpdateLocation();
            Game.GetWorld().SetCenterGeoPosition(position);
            ready = true;
            Invoke("UpdateLocation", 0.5f);
        }

        public bool IsReady()
        {
            return !paused && ready;
        }

        public GeoPosition GetGeoLocation()
        {
            return this.position;
        }

        void UpdateLocation()
        {
            if (!paused)
            {
                position.latitude = Input.location.lastData.latitude;
                position.longitude = Input.location.lastData.longitude;
            }
            Invoke("UpdateLocation", 0.5f);
        }

        void RandomLocation()
        {
           
            position.latitude += Random.Range(-0.001f, 0.001f);
            position.longitude += Random.Range(-0.001f, 0.001f);
            Game.GetClient().Log(position, this);
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
            if (Game.GetClient().serverType == ServerType.LOCAL || Game.GetClient().serverType == ServerType.DEV)
            {
                //Debug.Log(Game.FALLBACK_START_POSITION);
                Game.GetWorld().SetCenterGeoPosition(Game.FALLBACK_START_POSITION);
                Game.GetPlayer().GetModel().character.transform.geoPosition = Game.FALLBACK_START_POSITION;
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

        public float GetCompassValue()
        {
            return this.compassValue;
        }

        public void SetCompassValue(float value)
        {
            this.compassValue = value;
        }

        public float GetCompassDelta()
        {
            return this.compassDelta;
        }

        public void SetCompassDelta(float delta)
        {
            this.compassDelta = delta;
        }
    }

}
