using Assets.Control.util;
using Assets.Model;
using CanvasUtility;
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
                IndicateRequestFinished();
                World newWorld = JsonUtility.FromJson<World>(request.text);
                RefreshWorld(Game.GetInstance().player, newWorld);
            }
            else
                HandleError(request);
            

        }

        public void RefreshWorld(Player player, World world)
        {
            // TODO update instead of deleta and create
            UnityEngine.GameObject[] oldObjects = UnityEngine.GameObject.FindGameObjectsWithTag("DynamicGameObject");
            // delete removed objects
            bool found = false;
            long id;
            // match all existing objects



            foreach (Model.GameObject gameObject in world.gameObjects)
            {
                foreach (UnityEngine.GameObject oldObject in oldObjects)
                {
                    id = Convert.ToInt64(oldObject.gameObject.name.Replace("container_", ""));
                    if (id.Equals(gameObject.id))
                    {
                        gameObject.gameObject = oldObject;
                    }
                }
                
            }
            foreach (Model.GameObject gameObject in world.gameObjects)
            {
                if (gameObject.gameObject == null)
                {
                    UnityEngine.GameObject newObject = GameObjectTypes.CreateObject(gameObjectsContainer.transform, gameObject.prefab, gameObject.id, gameObject.name, true, "DynamicGameObject");
                    Game.GetInstance().GetAdapter().ToVirtual(gameObject.geoPosition, player);
                    newObject.transform.position = gameObject.geoPosition.ToPosition().ToVector3();
                    GameObjectTypes.GetObject(newObject).transform.localRotation = Quaternion.Euler(0, (float)gameObject.rotation.y, 0);
                } else
                {
                    Game.GetInstance().GetAdapter().ToVirtual(gameObject.geoPosition, player);
                    gameObject.gameObject.transform.position = gameObject.geoPosition.ToPosition().ToVector3();
                    GameObjectTypes.GetObject(gameObject.gameObject).transform.localRotation = Quaternion.Euler(0, (float)gameObject.rotation.y, 0);
                }
            }

            // delete all not existing
            foreach (UnityEngine.GameObject oldObject in oldObjects)
            {
                found = false;
                id = Convert.ToInt64(oldObject.gameObject.name.Replace("container_", ""));
                foreach (Model.GameObject gameObject in world.gameObjects)
                {
                    if (id.Equals(gameObject.id))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    UnityEngine.GameObject.Destroy(oldObject);
                }
            }
        }

        public IEnumerator RequestDestroy()
        {
            WWW request = Request("world/objects/destroy", JsonUtility.ToJson(Game.GetInstance().player));
            yield return request;
            if (request.error == null)
            {
                IndicateRequestFinished();
                World world = JsonUtility.FromJson<World>(request.text);
                Game.GetInstance().GetGameObjectsService().RefreshWorld(Game.GetInstance().player, world);

                //Debug.Log("Update terrain took " + (DateTime.Now - startTime));
                Info.Show("Destroyed!");
            }
            else
                HandleError(request);
            //craftContainerScript.ClearContainer();
        }
    }
}
