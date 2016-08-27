using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control;
using System;

public class GameScript : MonoBehaviour {

    private WorldService service;
    // Game objects
    private Terrain terrain;

    void Start () {
        Server.RUNNING_REQUESTS = 0;
        Game.GetInstance().SetServer(StartInitScript.server);
        Game.GetInstance().ResetServices();
        terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<Terrain>();
        // // TODO InvokeRepeating
        service = new WorldService(terrain);
        
        Invoke("WaitForLocation", 0);

        //InvokeRepeating("UpdateWorld", 2, 1);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && (SystemInfo.deviceType == DeviceType.Handheld))
            Application.Quit();
    }

    void WaitForLocation()
    {
        if (!Game.GetInstance().IsLocationReady())
        {
            Invoke("WaitForLocation", 0);
            Info.Show("Waiting for location...");
        } else
            Invoke("UpdateWorld", 1);
    }

    void UpdateWorld()
    {
        if (!Game.GetInstance().IsLocationReady())
            Invoke("WaitForLocation", 0);
        else { 
            if (!Server.RequestsRunning() && !Game.GetInstance().IsWorldUpdatedPaused())
                StartCoroutine(service.UpdateWorld(Game.GetInstance().player));
            Invoke("UpdateWorld", 2);
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
