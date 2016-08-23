using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control;
using System;

public class GameScript : MonoBehaviour {

    // scripts
    private static MapsTerrainScript MAPS_SCRIPT;

    // vars for update
    private GeoPosition currentPlayerPosition;

    // Use this for initialization
    void Start () {
        MAPS_SCRIPT = UnityEngine.GameObject.Find("MapsTerrain").GetComponent<MapsTerrainScript>();
        Invoke("UpdateWorld", 0.5f); // TODO InvokeRepeating
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateWorld()
    {
        MAPS_SCRIPT.UpdateWorld(Game.GetInstance().player);
    }
}
