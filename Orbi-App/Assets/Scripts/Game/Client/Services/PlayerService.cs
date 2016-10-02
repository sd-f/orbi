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
                Game.GetPlayer().GetCraftingController().SetInventory(inventory);
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestCraft(ServerModel.Player player)
        {
            WWW request = Request("player/craft", JsonUtility.ToJson(player));
            yield return request;
            if (request.error == null)
            {
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestDestroy(ServerModel.Player player)
        {
            WWW request = Request("player/destroy", JsonUtility.ToJson(player));
            yield return request;
            if (request.error == null)
            {
                //
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }
    }
}
