using UnityEngine;
using System.Collections;
using Assets.Control;
using Assets.Model;
using System;

public class MapsTerrainScript : MonoBehaviour {

    private GoogleMapsService googleMapsService = new GoogleMapsService();
    private WorldService worldService = new WorldService();
    private Terrain googleMapsTerrain;
    private static PlayerScript PLAYER_SCRIPT;
    private static GameObjectsScript GAMEOBJECTS_SCRIPT;

    void Start () {
        googleMapsTerrain = GetComponent<Terrain>();
        PLAYER_SCRIPT = UnityEngine.GameObject.Find("Player").GetComponent<PlayerScript>();
        GAMEOBJECTS_SCRIPT = UnityEngine.GameObject.Find("GameObjects").GetComponent<GameObjectsScript>();
    }
	
	public void UpdateWorld(Player player) {
        StartCoroutine(worldService.RequestTerrain(googleMapsTerrain, PLAYER_SCRIPT, GAMEOBJECTS_SCRIPT, player));
        StartCoroutine(googleMapsService.RequestMapData(googleMapsTerrain, player.geoPosition));
	}

}
