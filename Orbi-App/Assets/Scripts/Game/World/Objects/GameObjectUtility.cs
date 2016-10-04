using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        internal static long GetId(GameObject obj)
        {
            if (obj.name.Contains("container_"))
            {
                string id = obj.name.Replace("container_", "");
                return (long) Convert.ToDouble(id);
            }
            return 0;
        }
    }
}
