using System;
using ClientModel;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

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


        public static GameObject GetObject(GameObject container)
        {
            foreach (UnityEngine.Transform child in container.transform)
            {
                if (child.gameObject.name.Contains("object_") && container.name.Replace("container_", "").Equals(child.gameObject.name.Replace("object_", "")))
                    return child.gameObject;
                if (child.gameObject.name.Contains("uma_") && container.name.Replace("uma_container_", "").Equals(child.gameObject.name.Replace("uma_", "")))
                    return child.gameObject;
            }
            return null;
        }

        public static void NormalizeScale(GameObject container)
        {
            GameObject realObject = GetObject(container);
            float maxSize = Mathf.Clamp(GetMaxSize(realObject), 0.001f, 20f);
            float normalSize = 1.0f / maxSize;
            container.transform.localScale = new Vector3(normalSize, normalSize, normalSize);
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

        public static float GetMaxSize(GameObject obj)
        {
            float size = 1f;
            Collider collider = GetCollider(obj);
            if (collider != null)
                return GetMaxDimension(collider.bounds.size);
            return size;
        }

        public static Collider GetCollider(GameObject obj)
        {
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
                return collider;
            foreach (UnityEngine.Transform child in obj.transform)
                return GetCollider(child.gameObject);
            return null;
        }

        private static float GetMaxDimension(Vector3 vector)
        {
            float dim = 0f;
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

        internal static void UnFreeze(GameObject obj, RigidbodyConstraints constraints)
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().constraints = constraints;
            if (obj.GetComponent<NavMeshAgent>() != null)
            {
                obj.GetComponent<NavMeshAgent>().updatePosition = true;
                obj.GetComponent<NavMeshAgent>().enabled = true;
            }
            foreach (UnityEngine.Transform child in obj.transform)
                UnFreeze(child.gameObject, constraints);
        }

        internal static void DisableAI(GameObject obj)
        {
            if (obj.GetComponent<AICharacterControl>() != null)
            {
                obj.GetComponent<AICharacterControl>().enabled = false;
            }
            foreach (UnityEngine.Transform child in obj.transform)
                DisableAI(child.gameObject);
        }

        internal static void Freeze(GameObject obj)
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (obj.GetComponent<NavMeshAgent>() != null)
            {
                obj.GetComponent<NavMeshAgent>().updatePosition = false;
                obj.GetComponent<NavMeshAgent>().enabled = false;
            }
            foreach (UnityEngine.Transform child in obj.transform)
                Freeze(child.gameObject);
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

        internal static void Transform(GameObject gameObject, ClientModel.Transform transform, bool onNavMesh)
        {
            Translate(gameObject, transform.geoPosition, onNavMesh);
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

        internal static void Translate(GameObject gameObject, ServerModel.GeoPosition geoPosition, bool onNavMesh)
        {
            Vector3 b = geoPosition.ToPosition().ToVector3();
            if (onNavMesh)
            {
                b = Game.Instance.GetWorld().GetTerrainService().ClampPosition(b);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(b, out hit, 20f, 1))
                {
                    b = hit.position;
                    //Debug.Log(gameObject.name + " hit " + b);
                }
                //Debug.Log(gameObject.name + " " + b);
            }
            
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
