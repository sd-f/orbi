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
        public IEnumerator RequestGameObjects(Player player, UnityEngine.GameObject parent, WorldAdapter adapter, TerrainService terrainService)
        {
            WWW request = Request("world/around", JsonUtility.ToJson(player));
            yield return request;
            if (request.error == null)
            {
                World newWorld = JsonUtility.FromJson<World>(request.text);
                // TODO update instead of deleta and create
                UnityEngine.GameObject[] oldCubes = UnityEngine.GameObject.FindGameObjectsWithTag("dynamicGameObject");
                foreach (UnityEngine.GameObject cube in oldCubes)
                {
                    UnityEngine.GameObject.Destroy(cube);
                }


                foreach (Model.GameObject gameObject in newWorld.gameObjects)
                {
                    UnityEngine.GameObject newCube = UnityEngine.GameObject.Instantiate(getPrefab(gameObject.prefab)) as UnityEngine.GameObject;
                    // = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    // Modify the clone to your heart's content
                    newCube.transform.parent = parent.transform;
                    newCube.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);
                    newCube.tag = "dynamicGameObject";
                    newCube.name = "cube_" + gameObject.id + "_" + gameObject.name;
                    newCube.transform.rotation = Quaternion.Euler(0.0001f, 0.00001f, 0.0f);
                    adapter.ToVirtual(gameObject.geoPosition, player);
                    Debug.Log(gameObject.geoPosition);
                    /*Vector3 pos = gameObject.geoPosition.ToPosition().ToVector3();
                    float height = terrainService.GetTerrain().SampleHeight(new Vector3(pos.x+128, 100, pos.z+128));
                    height = 100 - height;
                    Debug.Log(height);
                    gameObject.geoPosition.altitude = gameObject.geoPosition.altitude + height;*/
                    newCube.transform.position = gameObject.geoPosition.ToPosition().ToVector3();
                    
                }
            }
            else
                Error.Show(request.error);
            IndicateRequestFinished();

        }

        private UnityEngine.GameObject getPrefab(string prefab)
        {
            return Resources.Load<UnityEngine.GameObject>("Prefabs/" + prefab) as UnityEngine.GameObject;
        }
    }
}
