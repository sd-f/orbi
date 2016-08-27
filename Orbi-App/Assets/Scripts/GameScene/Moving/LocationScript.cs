using UnityEngine;
using System.Collections;
using Assets.Control;

public class LocationScript : MonoBehaviour {

    IEnumerator CheckGps()
    {
        if (Game.GetInstance().server.Equals(ServerType.LOCAL))
        {

            Warning.Show("Location running in static mode");
            Game.GetInstance().SetLocationReady(true);
            yield break;
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Error.Show("Please enable GPS");
            yield return new WaitForSeconds(3);
            Application.Quit();
            yield break;
        }
        

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            
                
           yield return new WaitForSeconds(1);
           maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Error.Show("Waiting for GPS timed out");
            yield return new WaitForSeconds(3);
            Error.Show("You will get static location");
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
        // Stop service if there is no need to query location updates continuously
        InvokeRepeating("UpdateLocation", 0, 1);
        //Debug.Log("location started...");
        
    }

    void Awake()
    {
        StartCoroutine(CheckGps());
    }

    void UpdateLocation()
    {
        Game.GetInstance().player.geoPosition.latitude = Input.location.lastData.latitude;
        Game.GetInstance().player.geoPosition.longitude = Input.location.lastData.longitude;
        Game.GetInstance().SetLocationReady(true);
        //Game.GetInstance().player.geoPosition.altitude = Input.location.lastData.altitude;
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }
}
