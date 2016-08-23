using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control.services;

public class GameObjectsScript : MonoBehaviour {

    private GameObjectsService gameObjectService = new GameObjectsService();
    private UnityEngine.GameObject parent;

    // Use this for initialization
    void Start () {
        parent = this.gameObject;
	}

    public void UpdateGameObjects(Player player)
    {
        StartCoroutine(gameObjectService.RequestGameObjects(player, parent));
    }

}
