using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control.services;

public class PlayerScript : MonoBehaviour {

    private PlayerService playerService = new PlayerService();
    private UnityEngine.GameObject mainCamera;

    void Start()
    {
        mainCamera = UnityEngine.GameObject.Find("MainCamera");
    }

    public void UpdatePlayerHeight(Player player)
    {
        StartCoroutine(playerService.RequestPlayerHeight(player, mainCamera));
    }
}
