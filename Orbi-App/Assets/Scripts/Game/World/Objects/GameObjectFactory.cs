using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameController
{
    class GameObjectFactory
    {
        public static string DEFAULT = "Cubes/Bricks";

        public static GameObject GetPrefab(string prefab)
        {
            return Resources.Load<GameObject>(prefab) as GameObject;
        }

        public static GameObject GetGamePrefab(string prefab)
        {
            return Resources.Load<GameObject>("Game/Prefabs/" + prefab) as GameObject;
        }

        public static GameObject CreateObject(Transform parent, string prefab, long id, string tag)
        {
            return CreateObject(parent, prefab, id, tag, LayerMask.NameToLayer("Objects"));
        }

        public static GameObject CreateObject(Transform parent, string prefab, long id, string tag, int layer)
        {
            GameObject container = new GameObject();
            if (tag != null)
                container.tag = tag;
            container.name = "container_" + id;
            container.transform.parent = parent;

            GameObject newObject = GameObject.Instantiate(GetGamePrefab(prefab)) as GameObject;
            newObject.transform.parent = container.transform;
            newObject.name = "object_" + id;
            // max bounds
            Vector3 boundsSize = new Vector3(0, 0, 0);
            SetLayer(container, layer);
            return container;
        }



        public static void SetLayer(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayer(child.gameObject, layer);
            }
        }

        
        public static GameObject GetObject(GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.gameObject.name.Contains("object_") && obj.name.Replace("container_", "").Equals(child.gameObject.name.Replace("object_", "")))
                {
                    return child.gameObject;
                }
            }
            return null;
        }

    }
}
