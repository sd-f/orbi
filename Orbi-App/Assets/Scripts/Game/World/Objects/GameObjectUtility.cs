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

        public static void NormalizeScale(GameObject obj)
        {
            float size = 1.0f / GetSize(obj);
            obj.transform.localScale = new Vector3(size, size, size);
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

        public static float GetSize(GameObject obj)
        {
            float size = 1.0f;
            Collider collider = GetCollider(obj);
            if (collider != null)
                return GetDimension(collider.bounds.size);
            return size;
        }

        public static Collider GetCollider(GameObject obj)
        {
            Collider collider;
            foreach (UnityEngine.Transform child in obj.transform)
            {
                collider = child.GetComponent<Collider>();
                if (collider != null)
                    return collider;
                return GetCollider(child.gameObject);
            }
            return null;
        }

        private static float GetDimension(Vector3 vector)
        {
            float dim = 1f;
            if (vector.x > dim)
                dim = vector.x;
            if (vector.y > dim)
                dim = vector.y;
            if (vector.z > dim)
                dim = vector.z;
            return dim;
        }

        internal static RigidbodyConstraints IntToRigidbodyConstraint(int constraints)
        {
            foreach(RigidbodyConstraints constraint in Enum.GetValues(typeof(RigidbodyConstraints)))
                if ((int)constraint == constraints)
                    return constraint;
            return RigidbodyConstraints.FreezeAll;
        }

        internal static void SetConstraints(GameObject obj, RigidbodyConstraints constraints)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().constraints = constraints;
            }
            foreach (UnityEngine.Transform child in obj.transform)
            {
                SetConstraints(child.gameObject, constraints);
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

        public static GameObject FindChildWithName(GameObject obj, string name)
        {
            foreach (UnityEngine.Transform child in obj.transform)
                if (child.name.Equals(name))
                    return child.gameObject;
            return null;
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
