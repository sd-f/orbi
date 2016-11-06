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
            container.transform.SetParent(parent, false);

            GameObject newObject = GameObject.Instantiate(GetGamePrefab(prefab)) as GameObject;
            newObject.transform.SetParent(container.transform, false);
            newObject.name = "object_" + id;
            // max bounds
            GameObjectUtility.SetLayer(container, layer);
            GameObjectUtility.Freeze(container);
            return container;
        }



    }
}
