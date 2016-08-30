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
