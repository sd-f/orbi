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

        public static UnityEngine.GameObject getPrefab(string prefab)
        {
            return Resources.Load<UnityEngine.GameObject>("Prefabs/" + prefab) as UnityEngine.GameObject;
        }

        public static UnityEngine.GameObject CreateObject(Transform parent, string prefab, long id, string name, bool dynamic)
        {
            UnityEngine.GameObject container = new GameObject();
            if (dynamic)
                container.tag = "dynamicGameObject";
            container.name = "container_" + id + "_" + name;
            container.transform.parent = parent;
            UnityEngine.GameObject newObject = UnityEngine.GameObject.Instantiate(GameObjectTypes.getPrefab(prefab)) as UnityEngine.GameObject;
            newObject.transform.parent = container.transform;
            
            newObject.name = "object_" + id + "_" + name;
            Vector3 boundsSize = newObject.GetComponentInChildren<Collider>().bounds.size;
            newObject.transform.localPosition = new Vector3(0, (boundsSize.y / 2f), 0);
            Debug.Log(newObject.name + " " + boundsSize.y);
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


    }
}
