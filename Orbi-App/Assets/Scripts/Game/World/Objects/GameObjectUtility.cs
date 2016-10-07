using System;
using UnityEngine;

namespace GameController
{
    public static class GameObjectUtility
    {

        public static void DestroyAllChildObjects(GameObject parent)
        {
            foreach (Transform child in parent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Rigidbody GetRigidBody(GameObject obj)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                return obj.GetComponent<Rigidbody>();
            }
            return GetRigidBodyInChildren(obj);
        }

        public static Rigidbody GetRigidBodyInChildren(GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.gameObject.GetComponent<Rigidbody>() != null)
                {
                    return child.gameObject.GetComponent<Rigidbody>();
                }
                return GetRigidBodyInChildren(child.gameObject);
            }
            return null;
        }

        public static void SetLayer(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayer(child.gameObject, layer);
            }
        }

        internal static void Freeze(GameObject obj)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            foreach (Transform child in obj.transform)
            {
                Freeze(child.gameObject);
            }
        }


        internal static GameObject GetObjectContainer(GameObject obj)
        {
            return GetObjectContainer(obj, "container_");
        }

        internal static GameObject GetObjectContainer(GameObject obj, string prefix)
        {
            if (obj.name.Contains(prefix))
                return obj;
            if (obj.transform.parent != null)
                if (obj.transform.parent.gameObject != null)
                    return GetObjectContainer(obj.transform.parent.gameObject, prefix);
            return null;
        }

        internal static void Transform(GameObject newObject, ClientModel.Transform transform)
        {
            newObject.transform.position = transform.geoPosition.ToPosition().ToVector3();
            newObject.transform.localRotation = Quaternion.Euler(0, (float)transform.rotation.y, 0);
        }



        internal static long GetId(GameObject obj)
        {
            return GetId(obj, "container_");
        }

        internal static long GetId(GameObject obj, string prefix)
        {
            if (obj.name.Contains(prefix))
            {
                string id = obj.name.Replace(prefix, "");
                return (long) Convert.ToDouble(id);
            }
            return 0;
        }
    }
}
