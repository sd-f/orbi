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
        private bool deltaUpdated = false;

        // for debug in editor only
        public double latitude = 0.0f;
        public double longitude = 0.0f;

        private int failedPositionRequests;

        void Start()
        {
            if (Game.Instance.GetClient().randomLocation)
                InvokeRepeating("RandomLocation", 10f, 10f);
        }

        void OnAwake()
        {
            Input.location.Start();
        }

        public void StartUpdatingLocation()
        {
            UpdateLocation();
            Game.Instance.GetWorld().SetCenterGeoPosition(position);
            ready = true;
            Invoke("UpdateLocation", 0.5f);
            Invoke("RestartDevices", 3f);
        }

        void RestartDevices()
        {
            Sensor.Activate(Sensor.Type.Accelerometer);
            Sensor.Activate(Sensor.Type.MagneticField);
            Sensor.Activate(Sensor.Type.Gyroscope);
            SensorHelper.ActivateRotation();
            Input.location.Start();
        }

        public bool IsDeltaUpdated()
        {
            return this.deltaUpdated;
        }

        public void SetDeltaUpdated(bool updated)
        {
            this.deltaUpdated = updated;
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
            this.latitude = position.latitude;
            this.longitude = position.longitude;
            if (!paused)
            {        
                CheckLocation();
            }
            
        }

        void RandomLocation()
        {
           
            position.latitude += Random.Range(-0.001f, 0.001f);
            position.longitude += Random.Range(-0.001f, 0.001f);
            Game.Instance.GetPlayer().GetModel().character.transform.rotation = new Rotation(0f, Random.Range(0f, 360f), 0f);
            Game.Instance.GetClient().Log("Random Location: " + position, this);
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

        public void CheckLocation()
        {
            
            if (Input.location.status == LocationServiceStatus.Running)
            {
                position.latitude = Input.location.lastData.latitude;
                position.longitude = Input.location.lastData.longitude;
            } else
            {
                failedPositionRequests++;
                if (failedPositionRequests > 60)
                {
                    Error.Show("No GPS Postion");
                    failedPositionRequests = 0;
                }
            }            

            Invoke("UpdateLocation", 0.5f);
        }

        public IEnumerator Boot()
        {
            position = Game.FALLBACK_START_POSITION;
            if (Game.Instance.GetClient().serverType == ServerType.LOCAL || Game.Instance.GetClient().serverType == ServerType.DEV)
            {
                //Debug.Log(Game.Instance.FALLBACK_START_POSITION);
                Game.Instance.GetWorld().SetCenterGeoPosition(Game.FALLBACK_START_POSITION);
                Game.Instance.GetPlayer().GetModel().character.transform.geoPosition = Game.FALLBACK_START_POSITION;
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
                Info.Show("Waiting for location service to start...");
                yield return new WaitForSeconds(3);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Error.Show("Waiting for GPS timed out - Turn on GPS");
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
