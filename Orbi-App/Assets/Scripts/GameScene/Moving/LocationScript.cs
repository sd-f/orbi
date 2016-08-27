using UnityEngine;
using System.Collections;
using Assets.Control;

public class LocationScript : MonoBehaviour {

    IEnumerator CheckGps()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            Game.GetInstance().SetLocationReady(true);
            Warning.Show("Location disabled for desktop devices");
            yield break;
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Error.Show("Please enable GPS");
            yield break;
        }
            

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 120;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Error.Show("Wating for GPS timed out");
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
        // Stop service if there is no need to query location updates continuously
        InvokeRepeating("UpdateLocation", 0, 1);
        //Debug.Log("location started...");
        Game.GetInstance().SetLocationReady(true);
    }

    void Awake()
    {
        StartCoroutine(CheckGps());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateLocation()
    {
        Game.GetInstance().player.geoPosition.latitude = Input.location.lastData.latitude;
        Game.GetInstance().player.geoPosition.longitude = Input.location.lastData.longitude;
        //Game.GetInstance().player.geoPosition.altitude = Input.location.lastData.altitude;
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }
}
