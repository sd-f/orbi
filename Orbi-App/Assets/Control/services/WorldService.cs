using UnityEngine;
using System.Collections;
using Assets.Model;
using System;
using Assets.Control.services;

namespace Assets.Control
{
    class WorldService : AbstractService
    {

        public WorldService(Terrain terrain)
        {
            Game.GetInstance().InitTerrain(terrain);
        }

        public IEnumerator UpdateWorld(Player player)
        {
            
            yield return Game.GetInstance().GetGoogleMapsService().RequestMapData(player.geoPosition);
            if (Game.GetInstance().IsHeightsEnabled())
            {
                yield return Game.GetInstance().GetTerrainService().RequestTerrain(player);
            } else
                Game.GetInstance().GetTerrainService().ResetTerrain();
            Game.GetInstance().GetPlayerService().SetPlayerOnTerrain();
            yield return Game.GetInstance().GetGameObjectsService().RequestGameObjects();
            //Info.Show("updating player height");
            // works only if terrain is loaded
        }
    }

    
}
