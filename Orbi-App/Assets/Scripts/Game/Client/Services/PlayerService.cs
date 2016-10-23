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
                Game.GetPlayer().GetCraftingController().RemoveInventoryItem(player.gameObjectToCraft.prefab);
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestPlayer()
        {
            WWW request = Request("player",null);
            yield return request;
            if (request.error == null)
            {
                ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(request.text);
                Game.GetPlayer().SetModel(player);
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestUpdateTransform()
        {
            ClientModel.Transform newTransform = Game.GetPlayer().GetModel().character.transform;
            //Game.GetClient().Log("update transform " + newTransform);
            WWW request = Request("player/update", JsonUtility.ToJson(newTransform));
            yield return request;
            if (request.error == null)
            {
                //ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(request.text);
                //Game.GetPlayer().SetModel(player);
                IndicateRequestFinished();
            }
            else
                HandleError(request);

        }

        public IEnumerator RequestStatsUpdate()
        {
            WWW request = Request("player", null);
            yield return request;
            if (request.error == null)
            {
                ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(request.text);
                Game.GetPlayer().GetModel().character.level = player.character.level;
                Game.GetPlayer().GetModel().character.xp = player.character.xp;
                Game.GetPlayer().GetModel().character.xr = player.character.xr;
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
