using Assets.Model;
using System.Collections;
using UnityEngine;

namespace Assets.Control.services
{

    class PlayerService: AbstractService
    {

        public IEnumerator RequestPlayerHeight(Player player, UnityEngine.GameObject cameraGameObject, WorldAdapter adapter)
        {
            WWW request = Request("player/altitude", JsonUtility.ToJson(player));
            yield return request;
            if (request.error == null)
            {
                Player newPlayer = JsonUtility.FromJson<Player>(request.text);
                GeoPosition newPosition = new GeoPosition();
                newPosition.altitude = newPlayer.geoPosition.altitude;
                adapter.ToVirtual(newPosition);
                newPlayer.geoPosition.altitude = newPosition.altitude;
                cameraGameObject.transform.position = new Vector3(0, (float)newPlayer.geoPosition.altitude + 2.0f, 0);

                //Debug.Log("Update terrain took " + (DateTime.Now - startTime));
                IndicateRequestFinished();
            }
            else
            {
                IndicateRequestFinished();
                Error.Show(request.error);
            }
               
        }
    }
}
