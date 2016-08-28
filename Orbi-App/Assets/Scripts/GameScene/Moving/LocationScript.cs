using UnityEngine;
using System.Collections;
using Assets.Control;
using CanvasUtility;

public class LocationScript : MonoBehaviour {

    IEnumerator CheckGps()
    {
       // TODO Game.GetInstance().player.geoPosition = Game.START_POSITION;
        yield return new WaitForSeconds(1);
        if (Game.GetInstance().server.Equals(GameController.ServerType.LOCAL))
        {
            Warning.Show("Location running in static mode");
            Game.GetInstance().SetLocationReady(true);
            yield break;
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Error.Show("Please enable GPS");
            Warning.Show("You will get static location");
            Game.GetInstance().SetLocationReady(true);
            yield break;
        }
        

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Warning.Show("Waiting for location service to start...");
            yield return new WaitForSeconds(1);
           maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
           
            Error.Show("Waiting for GPS timed out");
            yield return new WaitForSeconds(3);
            Game.GetInstance().SetLocationReady(true);
            Warning.Show("You will get static location");
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
        // fix pc position always 0,0
        if ((Input.location.lastData.latitude) != 0 && (Input.location.lastData.longitude != 0))
        {
            Game.GetInstance().player.geoPosition.latitude = Input.location.lastData.latitude;
            Game.GetInstance().player.geoPosition.longitude = Input.location.lastData.longitude;
        }
        Game.GetInstance().SetLocationReady(true);
        //Game.GetInstance().player.geoPosition.altitude = Input.location.lastData.altitude;
    }

    void OnDestroy()
    {
        Input.location.Stop();
        CancelInvoke();
    }
}
