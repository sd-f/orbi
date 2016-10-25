using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class PlayerService: AbstractHttpService
    {
        public IEnumerator RequestInventory()
        {
            yield return Request("player/inventory", null, OnInventoryRecieved);
        }

        private void OnInventoryRecieved(string data)
        {
            ServerModel.Inventory inventory = JsonUtility.FromJson<ServerModel.Inventory>(data);
            Game.GetPlayer().GetCraftingController().SetInventory(inventory);
            IndicateRequestFinished();
        }

        public IEnumerator RequestCraft(ServerModel.Player player)
        {
            yield return Request("player/craft", JsonUtility.ToJson(player), OnCrafted, player.gameObjectToCraft.prefab);
        }

        private void OnCrafted(string data, object prefab)
        {
            Game.GetPlayer().GetCraftingController().RemoveInventoryItem((string)prefab);
        }

        public IEnumerator RequestPlayer()
        {
            yield return Request("player",null,OnPlayerRecieved);
        }

        private void OnPlayerRecieved(string data)
        {
            ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(data);
            Game.GetPlayer().SetModel(player);
        }

        public IEnumerator RequestUpdateTransform()
        {
            ClientModel.Transform newTransform = Game.GetPlayer().GetModel().character.transform;
            yield return Request("player/update", JsonUtility.ToJson(newTransform), OnPlayerTransformUpdated);
        }

        private void OnPlayerTransformUpdated(string data)
        {
            // silent
        }

        public IEnumerator RequestStatsUpdate()
        {
            yield return Request("player", null, OnStatsUpdatesRecieved);
        }

        private void OnStatsUpdatesRecieved(string data)
        {
            ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(data);
            Game.GetPlayer().GetModel().character.level = player.character.level;
            Game.GetPlayer().GetModel().character.xp = player.character.xp;
            Game.GetPlayer().GetModel().character.xr = player.character.xr;
        }

        public IEnumerator RequestDestroy(ServerModel.Player player)
        {
            yield return Request("player/destroy", JsonUtility.ToJson(player), OnDestroyed);
        }

        private void OnDestroyed(string obj)
        {
            // silent
        }
    }
}
