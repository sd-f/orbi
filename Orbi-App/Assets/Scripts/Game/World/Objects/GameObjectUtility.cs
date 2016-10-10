using System;
using ClientModel;
using UnityEngine;

namespace GameController
{
    public static class GameObjectUtility
    {

        public static void DestroyAllChildObjects(GameObject parent)
        {
            foreach (UnityEngine.Transform child in parent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void TrySettingTextInChildren(GameObject parent, string text)
        {
            TextMesh textMesh = parent.GetComponent<TextMesh>();
            if ((parent.tag == "UserText") && (textMesh != null))
                textMesh.text = text;
            foreach (UnityEngine.Transform child in parent.transform)
                TrySettingTextInChildren(child.gameObject, text);
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
            foreach (UnityEngine.Transform child in obj.transform)
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
            foreach (UnityEngine.Transform child in obj.transform)
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
            foreach (UnityEngine.Transform child in obj.transform)
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

        internal static void Transform(GameObject gameObject, ClientModel.Transform transform)
        {
            Translate(gameObject, transform.geoPosition);
            Rotate(gameObject, transform.rotation);
            
        }

        internal static void Rotate(GameObject gameObject, ServerModel.Rotation rotation)
        {
            
            Quaternion target = Quaternion.Euler(0, (float)rotation.y, 0);
            if (Quaternion.Angle(gameObject.transform.localRotation, target) > 0.0001)
            {
                gameObject.transform.localRotation = target;
            }
                
        }

        internal static void Translate(GameObject gameObject, ServerModel.GeoPosition geoPosition)
        {
            Vector3 b = geoPosition.ToPosition().ToVector3();
            if (!V3Equal(gameObject.transform.position,b))
            {
                gameObject.transform.position = b;
            }
               
        }

        public static bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
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
