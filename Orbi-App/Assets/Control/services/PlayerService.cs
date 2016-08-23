using Assets.Model;
using System.Collections;
using UnityEngine;

namespace Assets.Control.services
{

    class PlayerService: AbstractService
    {

        public IEnumerator RequestPlayerHeight(Player player, UnityEngine.GameObject camera)
        {
            WWW request = Request("player/altitude", JsonUtility.ToJson(player));
            yield return request;
            if (request.error == null)
            {
                Player newPlayer = JsonUtility.FromJson<Player>(request.text);
                GeoPosition newPosition = new GeoPosition();
                newPosition.altitude = newPlayer.geoPosition.altitude;
                WorldAdapter.ToVirtual(newPosition);
                newPlayer.geoPosition.altitude = newPosition.altitude;
                camera.transform.position = new Vector3(0, (float)newPlayer.geoPosition.altitude + 2.0f, 0);

                //Debug.Log("Update terrain took " + (DateTime.Now - startTime));
            }
            else
                Error.Show(request.error);
        }
    }
}
