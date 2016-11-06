using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class PlayerService: AbstractHttpService
    {
        public IEnumerator RequestCraft(ServerModel.Player player)
        {
            yield return Request("player/craft", JsonUtility.ToJson(player), OnCrafted, player.gameObjectToCraft.type.prefab);
        }

        private void OnCrafted(string data, object prefab)
        {
            Game.Instance.GetPlayer().GetInventoryService().RemoveInventoryItem((string)prefab);
        }

        public IEnumerator RequestPlayer()
        {
            yield return Request("player",null,OnPlayerRecieved);
        }

        private void OnPlayerRecieved(string data)
        {
            ServerModel.Player player = JsonUtility.FromJson<ServerModel.Player>(data);
            Game.Instance.GetPlayer().SetModel(player);
        }

        public IEnumerator RequestUpdateTransform()
        {
            ClientModel.Transform newTransform = Game.Instance.GetPlayer().GetModel().character.transform;
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
            Game.Instance.GetPlayer().GetModel().character.level = player.character.level;
            Game.Instance.GetPlayer().GetModel().character.xp = player.character.xp;
            Game.Instance.GetPlayer().GetModel().character.xr = player.character.xr;
            Game.Instance.GetPlayer().GetModel().character.nextLevelXp = player.character.nextLevelXp;
            Game.Instance.GetPlayer().GetModel().character.lastLevelXp = player.character.lastLevelXp;
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
