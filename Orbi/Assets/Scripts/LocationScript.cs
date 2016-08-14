using UnityEngine;
using System.Collections;

public class LocationScript : MonoBehaviour {

    public static double latitude = 47.0676;
    public static double longitude = 15.5552f;
    public static double elevation = 0.0f;

    IEnumerator Start()
    {
        //Debug.Log("location starting...");
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();
        //Debug.Log("location started");
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
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        
        InvokeRepeating("UpdateWorld", 0, 5);
    }

    // Update is called once per frame
    void Update () {
	
	}

    void UpdateWorld()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        elevation = Input.location.lastData.altitude;
        InitScript initScript = GameObject.FindGameObjectWithTag("cubes_container").GetComponent<InitScript>();
        initScript.UdpateWorld(); // todo elevation
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }
}
