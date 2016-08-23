using UnityEngine;
using System.Collections;
using Assets.Model;
using System;
using Assets.Control.services;

namespace Assets.Control
{
    class WorldService : AbstractService
    {
        private TerrainService terrainService = new TerrainService();

        public IEnumerator RequestTerrain(Terrain terrain, PlayerScript playerScript, GameObjectsScript gameObjectsScript, Player player)
        {
            World generatedWorld = terrainService.GenerateDummyWorldArround(player);
            WWW request = Request("world/terrain", JsonUtility.ToJson(generatedWorld));
            yield return request;
            if (request.error == null)
            {
                World terrainWorld = JsonUtility.FromJson<World>(request.text);
                terrainService.AdjustTerrainHeights(terrain, terrainWorld, player);
                // works only if terrain is loaded
                playerScript.UpdatePlayerHeight(player);
                gameObjectsScript.UpdateGameObjects(player);
            }
            else
                Error.Show(request.error);
        }
    }
}
