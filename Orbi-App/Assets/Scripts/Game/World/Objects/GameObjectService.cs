﻿using CanvasUtility;
using GameController.Services;
using GameScene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GameController
{
    class GameObjectService: AbstractHttpService
    {

        GameObject gameObjectsContainer;
        GameObject charactersContainer;

        private List<ServerModel.Character> oldCharacters = new List<ServerModel.Character>();
        private List<ServerModel.GameObject> oldObjects = new List<ServerModel.GameObject>();

        public GameObjectService()
        {
            gameObjectsContainer = GameObject.Find("Objects");
            charactersContainer = GameObject.Find("Characters");
        }

        public IEnumerator RequestStatistics()
        {
            WWW request = Request("world/statistics", null);
            yield return request;
            if (request.error == null)
            {
                ServerModel.Statistics stats = JsonUtility.FromJson<ServerModel.Statistics>(request.text);
                Game.GetWorld().SetStatistics(stats);
                // TODO bad hack
                GameObject statsGameObject = GameObject.Find("StatisticsPanel");
                if (statsGameObject != null)
                {
                    StartScene.Canvas statsCanvas = statsGameObject.GetComponent<StartScene.Canvas>();
                    if (statsCanvas != null)
                        statsCanvas.UpdateStats();
                }
                IndicateRequestFinished();
            }
            else
                HandleError(request);
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

        

        void UpdateCharacter(ServerModel.Character oldCharacter, ServerModel.Character newCharacter)
        {
            oldCharacter.transform = newCharacter.transform;
            foreach (Transform child in oldCharacter.gameObject.transform)
                if (child.name.Equals("uma_" + oldCharacter.id))
                    child.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(new Vector3(0f, (float)oldCharacter.transform.rotation.y, 0f)));
            foreach (Transform child in oldCharacter.gameObject.transform)
                if (child.name.Equals("uma_target_" + oldCharacter.id))
                    GameObjectUtility.Transform(child.gameObject, oldCharacter.transform);
        }

        void CreateCharacter(ServerModel.Character newCharacter)
        {
            GameObject newObjectContainer = new GameObject();
            newCharacter.gameObject = newObjectContainer;
            oldCharacters.Add(newCharacter);
            newObjectContainer.name = "uma_container_" + newCharacter.id;
            newObjectContainer.transform.SetParent(charactersContainer.transform);
            newObjectContainer.tag = "DynamicCharacter";
            GameObjectUtility.SetLayer(newObjectContainer, LayerMask.NameToLayer("Objects"));
            GameObjectUtility.Transform(newObjectContainer, newCharacter.transform);


            GameObject newObject = Game.GetWorld().GetUMACreator().GenerateUMA(newObjectContainer, "uma_" + newCharacter.id);
            newObject.GetComponent<CharacterProperties>().SetCharacter(newCharacter);
            GameObjectUtility.SetLayer(newObject, LayerMask.NameToLayer("Objects"));

            GameObject newObjectTarget = new GameObject();
            GameObjectUtility.SetLayer(newObjectTarget, LayerMask.NameToLayer("Objects"));
            newObjectTarget.name = "uma_target_" + newCharacter.id;
            newObjectTarget.transform.SetParent(newObjectContainer.transform);

            newObject.GetComponent<AICharacterControl>().SetTarget(newObjectTarget.transform);

            //newObject.gameObject.GetComponent<UMAMovement>().SetTransform(newObjectTarget.transform);
        }

        void UpdateObject(ServerModel.GameObject oldObject, ServerModel.GameObject newObject)
        {
            oldObject.transform = newObject.transform;
            GameObjectUtility.Transform(oldObject.gameObject, newObject.transform);
        }

        void CreateObject(ServerModel.GameObject newObject)
        {
            GameObject newGameObject = GameObjectFactory.CreateObject(gameObjectsContainer.transform, newObject.prefab, newObject.id, "DynamicGameObject");
            if (!String.IsNullOrEmpty(newObject.userText))
                GameObjectUtility.TrySettingTextInChildren(newGameObject, newObject.userText);
            newObject.gameObject = newGameObject;
            oldObjects.Add(newObject);
            GameObjectUtility.Transform(newGameObject, newObject.transform);
        }

        void UpdateGameObjects(ServerModel.World world)
        {
            List<ServerModel.GameObject> newObjects = world.gameObjects;
            foreach (ServerModel.GameObject newObject in newObjects)
            {
                ServerModel.GameObject oldObject = oldObjects.Find(o => o.id == newObject.id);
                if (oldObject != null)
                    UpdateObject(oldObject, newObject);
                else
                    CreateObject(newObject);
            }
            foreach (ServerModel.GameObject oldObject in oldObjects)
                if (newObjects.Find(o => o.id == oldObject.id) == null)
                {
                    UnityEngine.GameObject.Destroy(oldObject.gameObject);
                    oldObject.gameObject = null;
                }
            oldObjects.RemoveAll(r => r.gameObject == null);

        }

        void UpdateCharacters(ServerModel.World world)
        {
            List<ServerModel.Character> newCharacters = world.characters;
            foreach (ServerModel.Character newCharacter in newCharacters)
            {
                ServerModel.Character oldCharacter = oldCharacters.Find(o => o.id == newCharacter.id);
                if (oldCharacter != null)
                    UpdateCharacter(oldCharacter, newCharacter);
                else
                    CreateCharacter(newCharacter);
            }
            foreach (ServerModel.Character oldCharacter in oldCharacters)
                if (newCharacters.Find(o => o.id == oldCharacter.id) == null)
                {
                    UnityEngine.GameObject.Destroy(oldCharacter.gameObject);
                    oldCharacter.gameObject = null;
                }
            oldCharacters.RemoveAll(r => r.gameObject == null);
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
