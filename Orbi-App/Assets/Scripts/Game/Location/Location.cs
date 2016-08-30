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
        private GeoPosition position = Game.FALLBACK_START_POSITION;
        private float heading = 0.0f;

        public void StartUpdatingLocation()
        {
            InvokeRepeating("UpdateLocation", 0, 0.1f);
        }

        public GeoPosition GetGeoLocation()
        {
            return this.position;
        }

        public float GetHeading()
        {
            return this.heading;
        }

        void UpdateLocation()
        {
            position.latitude = Input.location.lastData.latitude;
            position.longitude = Input.location.lastData.longitude;
            this.heading = Input.compass.magneticHeading;
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
                Warning.Show("Location running in LOCAL-mode");
                yield break;
            }

            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                Info.Show("Waiting for location service to start...");
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
            Info.Show("Location initialized");
            StartUpdatingLocation();
        }
    }

}
