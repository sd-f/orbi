using CanvasUtility;
using GameController.Services;
using System;
using System.Collections;
using UnityEngine;

namespace GameController
{
    class GameObjectService: AbstractService
    {

        GameObject gameObjectsContainer;

        public GameObjectService()
        {
            gameObjectsContainer = GameObject.Find("Objects");
        }

        public IEnumerator RequestGameObjects()
        {
            WWW request = Request("world/around", JsonUtility.ToJson(Game.GetPlayer().GetModel()));
            yield return request;
            if (request.error == null)
            {

                //Debug.Log("objects loaded...");
                ServerModel.World newWorld = JsonUtility.FromJson<ServerModel.World>(request.text);
                RefreshWorld(Game.GetPlayer().GetModel(), newWorld);
                IndicateRequestFinished();
            }
            else
                HandleError(request);
        }

        public void RefreshWorld(ServerModel.Player player, ServerModel.World world)
        {
            // TODO update instead of deleta and create
            GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("DynamicGameObject");
            // delete removed objects
            bool found = false;
            long id;
            // match all existing objects
            foreach (ServerModel.GameObject gameObject in world.gameObjects)
            {
                foreach (GameObject oldObject in oldObjects)
                {
                    id = Convert.ToInt64(oldObject.gameObject.name.Replace("container_", ""));
                    if (id.Equals(gameObject.id))
                    {
                        gameObject.gameObject = oldObject;
                    }
                }
                
            }
            foreach (ServerModel.GameObject gameObject in world.gameObjects)
            {
                if (gameObject.gameObject == null)
                {
                    GameObject newObject = GameObjectFactory.CreateObject(gameObjectsContainer.transform, gameObject.prefab, gameObject.id, gameObject.name, true, "DynamicGameObject");
                    newObject.transform.position = gameObject.geoPosition.ToPosition().ToVector3();
                    GameObjectFactory.GetObject(newObject).transform.localRotation = Quaternion.Euler(0, (float)gameObject.rotation.y, 0);
                } else
                {
                    gameObject.gameObject.transform.position = gameObject.geoPosition.ToPosition().ToVector3();
                    GameObjectFactory.GetObject(gameObject.gameObject).transform.localRotation = Quaternion.Euler(0, (float)gameObject.rotation.y, 0);
                }
            }

            // delete all not existing
            foreach (UnityEngine.GameObject oldObject in oldObjects)
            {
                found = false;
                id = Convert.ToInt64(oldObject.gameObject.name.Replace("container_", ""));
                foreach (ServerModel.GameObject gameObject in world.gameObjects)
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

        public IEnumerator RequestDestroy(CraftContainerScript craftContainerScript)
        {
            WWW request = Request("world/objects/destroy", JsonUtility.ToJson(Game.GetPlayer().GetModel()));
            yield return request;
            if (request.error == null)
            {
                IndicateRequestFinished();
                ServerModel.World world = JsonUtility.FromJson<ServerModel.World>(request.text);
                RefreshWorld(Game.GetPlayer().GetModel(), world);

                //Debug.Log("Update terrain took " + (DateTime.Now - startTime));
                Info.Show("Destroyed!");
            }
            else
                HandleError(request);
            craftContainerScript.ClearContainer();
        }
    }
}
