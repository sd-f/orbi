using UnityEngine;
using System.Collections;
using Assets.Model;
using System;
using Assets.Control.services;

namespace Assets.Control
{
    class WorldService : AbstractService
    {
        private TerrainService terrainService;
        private GameObjectsService gameObjectsService = new GameObjectsService();
        private PlayerService playerService = new PlayerService();
        private GoogleMapsService googleMapsService = new GoogleMapsService();
        private WorldAdapter worldAdapter;

        // gameobjects
        UnityEngine.GameObject cameraGameObject;
        UnityEngine.GameObject objectsContainer;

        public WorldService(Terrain terrain, UnityEngine.GameObject planeTerrain, UnityEngine.GameObject cameraGameObject, UnityEngine.GameObject objectsContainer)
        {
            terrainService = new TerrainService(terrain, planeTerrain);
            worldAdapter = new WorldAdapter(terrainService);
            this.cameraGameObject = cameraGameObject;
            this.objectsContainer = objectsContainer;
        }

        public IEnumerator UpdateWorld(Player player)
        {
            
            yield return googleMapsService.RequestMapData(terrainService, player.geoPosition);
            if (Game.GetInstance().IsHeightsEnabled())
            {
                yield return terrainService.RequestTerrain(player);
            } else
                terrainService.ResetTerrain();

            yield return playerService.RequestPlayerHeight(player, cameraGameObject, worldAdapter);
            yield return gameObjectsService.RequestGameObjects(player, objectsContainer, worldAdapter);
            //Info.Show("updating player height");
            // works only if terrain is loaded
        }
    }

    
}
