using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control;
using System;

public class GameScript : MonoBehaviour {

    private WorldService service;
    // Game objects
    private Terrain terrain;
    private UnityEngine.GameObject cameraGameObject;
    private UnityEngine.GameObject planeTerrain;
    private UnityEngine.GameObject objectsContainer;

    void Awake () {
        Server.RUNNING_REQUESTS = 0;
        Game.GetInstance().SetServer(StartInitScript.server);
        cameraGameObject = UnityEngine.GameObject.Find("MainCamera");
        objectsContainer = UnityEngine.GameObject.Find("Objects");
        planeTerrain = UnityEngine.GameObject.Find("PlaneTerrain");
        terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<Terrain>();
        // // TODO InvokeRepeating
        service = new WorldService(terrain, planeTerrain, cameraGameObject, objectsContainer);
        
        Invoke("WaitForLocation", 0);

        //InvokeRepeating("UpdateWorld", 2, 1);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
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
            if (!Server.RequestsRunning())
                StartCoroutine(service.UpdateWorld(Game.GetInstance().player));
            Invoke("UpdateWorld", 2);
        }
    }
}
