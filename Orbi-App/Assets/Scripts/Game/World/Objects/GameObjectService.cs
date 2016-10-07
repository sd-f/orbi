using CanvasUtility;
using GameController.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController
{
    class GameObjectService: AbstractHttpService
    {

        GameObject gameObjectsContainer;
        GameObject charactersContainer;

        public GameObjectService()
        {
            gameObjectsContainer = GameObject.Find("Objects");
            charactersContainer = GameObject.Find("Characters");
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

        void matchExisting(List<ServerModel.GameObject> objects, GameObject[] oldObjects)
        {
            long id;
            // match all existing objects
            foreach (ServerModel.GameObject gameObject in objects)
                foreach (GameObject oldObject in oldObjects)
                {
                    id = GameObjectUtility.GetId(oldObject.gameObject);
                    if (id.Equals(gameObject.id))
                        gameObject.gameObject = oldObject;
                }
        }

        void updateAndCreate(List<ServerModel.GameObject> objects)
        {
            foreach (ServerModel.GameObject gameObject in objects)
            {
                if (gameObject.gameObject == null)
                {
                    GameObject newObject = GameObjectFactory.CreateObject(gameObjectsContainer.transform, gameObject.prefab, gameObject.id, "DynamicGameObject");
                    GameObjectUtility.Transform(newObject, gameObject.transform);
                }
                else
                {
                    GameObjectUtility.Transform(gameObject.gameObject, gameObject.transform);
                }
            }
        }

        void deleteMissing(List<ServerModel.GameObject> objects, GameObject[] oldObjects)
        {
            // delete removed objects
            bool found = false;
            long id;

            // delete all not existing
            foreach (UnityEngine.GameObject oldObject in oldObjects)
            {
                found = false;
                id = GameObjectUtility.GetId(oldObject.gameObject);
                foreach (ServerModel.GameObject gameObject in objects)
                    if (id.Equals(gameObject.id))
                        found = true;
                if (!found)
                    UnityEngine.GameObject.Destroy(oldObject);
            }
        }

        void matchExistingCharacters(List<ServerModel.Character> characters, GameObject[] oldCharacters)
        {
            long id;
            // match all existing objects
            foreach (ServerModel.Character gameObject in characters)
                foreach (GameObject oldObject in oldCharacters)
                {
                    id = GameObjectUtility.GetId(oldObject.gameObject, "uma_");
                    if (id.Equals(gameObject.id))
                        gameObject.gameObject = oldObject;
                }
        }

        void updateAndCreateCharacters(List<ServerModel.Character> characters)
        {
            foreach (ServerModel.Character character in characters)
            {
                if (character.gameObject == null)
                {
                    
                    GameObject newObject = Game.GetWorld().GetUMACreator().GenerateUMA(charactersContainer, "uma_" + character.id);
                    GameObjectUtility.SetLayer(newObject, LayerMask.NameToLayer("Objects"));
                    newObject.tag = "DynamicCharacter";
                    GameObjectUtility.Transform(newObject, character.transform);
                }
                else
                {
                    GameObjectUtility.Transform(character.gameObject, character.transform);
                }
            }
        }

        void deleteMissingCharacters(List<ServerModel.Character> characters, GameObject[] oldCharacters)
        {
            // delete removed objects
            bool found = false;
            long id;

            // delete all not existing
            foreach (UnityEngine.GameObject oldObject in oldCharacters)
            {
                found = false;
                id = GameObjectUtility.GetId(oldObject.gameObject, "uma_");
                foreach (ServerModel.Character character in characters)
                    if (id.Equals(character.id))
                        found = true;
                if (!found)
                    UnityEngine.GameObject.Destroy(oldObject);
            }
        }

        void UpdateGameObjects(ServerModel.World world)
        {
            GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("DynamicGameObject");
            List<ServerModel.GameObject> newObjects = world.gameObjects;

            // match all existing objects and set transient gameobject
            matchExisting(newObjects, oldObjects);

            // create all missing and update transform for existing
            updateAndCreate(newObjects);

            // delete expired objects
            deleteMissing(newObjects, oldObjects);
        }

        void UpdateCharacters(ServerModel.World world)
        {
            GameObject[] oldCharacters = GameObject.FindGameObjectsWithTag("DynamicCharacter");
            List<ServerModel.Character> newCharacters = world.characters;

            // match all existing objects and set transient gameobject
            matchExistingCharacters(newCharacters, oldCharacters);

            // create all missing and update transform for existing
            updateAndCreateCharacters(newCharacters);

            // delete expired objects
            deleteMissingCharacters(newCharacters, oldCharacters);
        }

        public void RefreshWorld(ServerModel.Player player, ServerModel.World world)
        {
            // game objects
            UpdateGameObjects(world);
            // update characters maybe reduce update interval
            UpdateCharacters(world);
            
        }

    }
}
