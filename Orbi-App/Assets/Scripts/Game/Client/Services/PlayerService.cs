using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class PlayerService: AbstractHttpService
    {
        public IEnumerator RequestInventory()
        {
            WWW request = Request("player/inventory", null);
            yield return request;
            if (request.error == null)
            {
                ServerModel.Inventory inventory = JsonUtility.FromJson<ServerModel.Inventory>(request.text);
                Game.GetPlayer().SetInventory(inventory);
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }
    }
}
