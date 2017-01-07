using CanvasUtility;
using GameController.Services;
using GameScene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GameController
{
    public class GameObjectService: AbstractHttpService
    {

        public GameObject gameObjectsContainer;
        public GameObject charactersContainer;
        public GameObject objectCreatedEffect;
        public GameObject characterCreatedEffect;
        private List<ServerModel.Character> oldCharacters = new List<ServerModel.Character>();
        private List<ServerModel.GameObject> oldObjects = new List<ServerModel.GameObject>();
        private bool initialized = false;

        public IEnumerator RequestStatistics()
        {
            yield return Request("world/statistics", null, OnStatisticsLoaded);
        }

        private void OnStatisticsLoaded(string data)
        {
            ServerModel.Statistics stats = JsonUtility.FromJson<ServerModel.Statistics>(data);
            Game.Instance.GetWorld().SetStatistics(stats);
            // TODO bad hack
            GameObject statsGameObject = GameObject.Find("StatisticsPanel");
            if (statsGameObject != null)
            {
                //StartScene.Canvas statsCanvas = statsGameObject.GetComponent<StartScene.Canvas>();
                /*if (statsCanvas != null)
                    statsCanvas.UpdateStats();*/
                    // TODO
            }
        }

        public IEnumerator RequestGameObjects()
        {
            //Game.Instance.GetClient().Log("Update Objects on " + Game.Instance.GetPlayer().GetModel().character.transform.geoPosition);
            yield return Request("world/around", JsonUtility.ToJson(Game.Instance.GetPlayer().GetModel()), OnObjectsLoaded);
        }

        private void OnObjectsLoaded(string data)
        {
            ServerModel.World newWorld = JsonUtility.FromJson<ServerModel.World>(data);
            StartCoroutine(RefreshWorld(Game.Instance.GetPlayer().GetModel(), newWorld));
            
        }

        void UpdateCharacter(ServerModel.Character oldCharacter, ServerModel.Character newCharacter)
        {
            
            oldCharacter.transform = newCharacter.transform;
            foreach (Transform child in oldCharacter.gameObject.transform)
                if (child.name.Equals("uma_" + oldCharacter.id))
                {
                    child.GetComponent<ThirdPersonCharacter>().SetTarget(oldCharacter.transform.geoPosition.ToPosition().ToVector3());
                    child.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(new Vector3(0f, (float)oldCharacter.transform.rotation.y, 0f)));
                }   
                    
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
            GameObjectUtility.Transform(newObjectContainer, newCharacter.transform, true);

            GameObject newObject = Game.Instance.GetWorld().GetUMACreator().GenerateUMA(newObjectContainer, "uma_" + newCharacter.id);
            newObject.GetComponent<CharacterProperties>().SetCharacter(newCharacter);
            GameObjectUtility.SetLayer(newObject, LayerMask.NameToLayer("Objects"));
            
            UnityEngine.GameObject effect = UnityEngine.GameObject.Instantiate(characterCreatedEffect) as UnityEngine.GameObject;
            effect.transform.position = newObject.transform.position;
            effect.transform.localScale = effect.transform.localScale * GameObjectUtility.GetMaxSize(newObject);
            Destroy(effect, 2f);
            //newObject.gameObject.GetComponent<UMAMovement>().SetTransform(newObjectTarget.transform);
        }

        internal void ClearAll()
        {
            oldCharacters = new List<ServerModel.Character>();
            oldObjects = new List<ServerModel.GameObject>();
            GameObjectUtility.DestroyAllChildObjects(gameObjectsContainer);
            GameObjectUtility.DestroyAllChildObjects(charactersContainer);
        }

        private IEnumerator UpdateObject(ServerModel.GameObject oldObject, ServerModel.GameObject newObject)
        {
            UpdateAi(oldObject.gameObject, newObject);
            oldObject.transform = newObject.transform;
            GameObjectUtility.Transform(oldObject.gameObject, newObject.transform, (newObject.type.ai));
            yield return null;
        }


        private IEnumerator CreateObject(ServerModel.GameObject newObject)
        {
            //yield return new WaitForEndOfFrame();
            UnityEngine.GameObject newGameObject = GameObjectFactory.CreateObject(gameObjectsContainer.transform, newObject.type.prefab, newObject.id, "DynamicGameObject");
            ObjectProperties props = GameObjectUtility.GetCollider(newGameObject).gameObject.AddComponent<ObjectProperties>();
            props.SetObject(newObject);
            if (!String.IsNullOrEmpty(newObject.userText))
                GameObjectUtility.TrySettingTextInChildren(newGameObject, newObject.userText);
            newObject.gameObject = newGameObject;
            GameObjectUtility.Transform(newGameObject, newObject.transform, (newObject.type.ai));
            GameObjectUtility.SetConstraints(newGameObject, GameObjectUtility.IntToRigidbodyConstraint(newObject.constraints));
            oldObjects.Add(newObject);
            if (initialized)
            {
                UnityEngine.GameObject effect = UnityEngine.GameObject.Instantiate(objectCreatedEffect) as UnityEngine.GameObject;
                effect.transform.position = newGameObject.transform.position;
                effect.transform.localScale = effect.transform.localScale * GameObjectUtility.GetMaxSize(newGameObject) * 2f;
                Destroy(effect, 3.5f);
            }
            yield return null;

        }



        private void UpdateAi(UnityEngine.GameObject gameObject, ServerModel.GameObject newObject)
        {
            if (newObject.type.ai)
            {
                ThirdPersonCharacter controller = gameObject.GetComponentInChildren<ThirdPersonCharacter>();
                if (controller != null)
                {
                    controller.SetTarget(newObject.aiProperties.target.geoPosition.ToPosition().ToVector3());
                }
                GameObjectUtility.UnFreeze(gameObject, GameObjectUtility.IntToRigidbodyConstraint(newObject.constraints));
            }
        }

        private IEnumerator UpdateGameObjects(ServerModel.World world)
        {
            List<ServerModel.GameObject> newObjects = world.gameObjects;
            foreach (ServerModel.GameObject newObject in newObjects)
            {
                ServerModel.GameObject oldObject = oldObjects.Find(o => o.id == newObject.id);
                
                if (oldObject != null)
                    yield return UpdateObject(oldObject, newObject);
                else
                    yield return CreateObject(newObject);
                    
            }
            foreach (ServerModel.GameObject oldObject in oldObjects)
                if (newObjects.Find(o => o.id == oldObject.id) == null)
                {
                    GameObject gameObject = oldObject.gameObject;
                    oldObject.gameObject = null;
                    GetComponent<DestructionController>().RemoveObject(gameObject);
                    

                }
            oldObjects.RemoveAll(r => r.gameObject == null);
            yield return null;

        }

        private IEnumerator UpdateCharacters(ServerModel.World world)
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
            yield return null;
        }

        public void RemoveObject(GameObject objectToRemove)
        {
            oldObjects.RemoveAll(r => GameObject.ReferenceEquals(r.gameObject,objectToRemove));
        }

        private IEnumerator RefreshWorld(ServerModel.Player player, ServerModel.World world)
        {
            // game objects
            yield return UpdateGameObjects(world);
            // update characters maybe reduce update interval
            yield return UpdateCharacters(world);
            initialized = true;
        }

    }
}
