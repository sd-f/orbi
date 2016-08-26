using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Control.util
{
    class GameObjectTypes
    {
        public static string DEFAULT = "ScifiCrate/ScifiCrate_1";

        public static GameObject getPrefab(string prefab)
        {
            return Resources.Load<GameObject>("Game/Prefabs/" + prefab) as GameObject;
        }

        public static UnityEngine.GameObject CreateObject(Transform parent, string prefab, long id, string name, bool dynamic, string tag)
        {
            GameObject container = new GameObject();
            container.tag = tag;
            container.name = "container_" + id;
            container.transform.parent = parent;
            GameObject newObject = UnityEngine.GameObject.Instantiate(GameObjectTypes.getPrefab(prefab)) as GameObject;
            newObject.transform.parent = container.transform;
            newObject.name = "object_" + id;
            Vector3 boundsSize = newObject.GetComponentInChildren<Collider>().bounds.size;
            newObject.transform.localPosition = new Vector3(0, (boundsSize.y / 2f), 0);
            
            SetLayer(container, LayerMask.NameToLayer("Objects"));
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
