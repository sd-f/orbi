using Assets.Model;
using System.Collections;
using UnityEngine;
using System;

namespace Assets.Control.services
{

    class PlayerService: AbstractService
    {

        UnityEngine.GameObject camera;

        public PlayerService()
        {
            camera = UnityEngine.GameObject.Find("MainCamera");
        }

        public IEnumerator RequestPlayerHeight()
        {
            WWW request = Request("player/altitude", JsonUtility.ToJson(Game.GetInstance().player));
            yield return request;
            if (request.error == null)
            {
                IndicateRequestFinished();
                Player newPlayer = JsonUtility.FromJson<Player>(request.text);
                GeoPosition newPosition = new GeoPosition();
                newPosition.altitude = newPlayer.geoPosition.altitude;
                Game.GetInstance().GetAdapter().ToVirtual(newPosition);
                newPlayer.geoPosition.altitude = newPosition.altitude;
                camera.transform.position = new Vector3(0, (float)newPlayer.geoPosition.altitude + 2.0f, 0);
            }
            else
                HandleError(request);
               
        }

        public IEnumerator RequestCraft(CraftContainerScript script)
        {
            WWW request = Request("player/craft", JsonUtility.ToJson(Game.GetInstance().player));
            yield return request;
            if (request.error == null)
            {
                IndicateRequestFinished();
                World world = JsonUtility.FromJson<World>(request.text);
                Game.GetInstance().GetGameObjectsService().RefreshWorld(Game.GetInstance().player, world);

                //Debug.Log("Update terrain took " + (DateTime.Now - startTime));
                Info.Show("Yeah!");


            }
            else
                HandleError(request);
            script.ClearContainer();

        }

        public void SetPlayerOnTerrain()
        {
            camera.transform.position = new Vector3(0, Game.GetInstance().GetTerrainService().GetTerrainHeight(0, 0) + 3.0f, 0);
        }

    }
}
