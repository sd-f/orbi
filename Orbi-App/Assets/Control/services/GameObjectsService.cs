using Assets.Control.util;
using Assets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Control.services
{
    class GameObjectsService: AbstractService
    {

        UnityEngine.GameObject gameObjectsContainer;

        public GameObjectsService()
        {
            gameObjectsContainer = UnityEngine.GameObject.Find("Objects");
        }

        public IEnumerator RequestGameObjects()
        {
            WWW request = Request("world/around", JsonUtility.ToJson(Game.GetInstance().player));
            yield return request;
            if (request.error == null)
            {
                World newWorld = JsonUtility.FromJson<World>(request.text);
                RefreshWorld(Game.GetInstance().player, newWorld);
            }
            else
                Error.Show(request.error);
            IndicateRequestFinished();

        }

        public void RefreshWorld(Player player, World world)
        {
            // TODO update instead of deleta and create
            UnityEngine.GameObject[] oldCubes = UnityEngine.GameObject.FindGameObjectsWithTag("dynamicGameObject");
            foreach (UnityEngine.GameObject cube in oldCubes)
            {
                UnityEngine.GameObject.Destroy(cube);
            }


            foreach (Model.GameObject gameObject in world.gameObjects)
            {
                UnityEngine.GameObject newObject = GameObjectTypes.CreateObject(gameObjectsContainer.transform, gameObject.prefab, gameObject.id, gameObject.name, true);
                Game.GetInstance().GetAdapter().ToVirtual(gameObject.geoPosition, player);
                newObject.transform.position = gameObject.geoPosition.ToPosition().ToVector3();

            }
        }
    }
}
