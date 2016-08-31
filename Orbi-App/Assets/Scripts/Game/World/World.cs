using ServerModel;
using UnityEngine;

namespace GameController
{

    [AddComponentMenu("App/Game/World")]
    public class World : MonoBehaviour
    {
        public LayerMask terrainLayer;
        public LayerMask terrainObjectsLayer;
        public Terrain terrain;
        private GeoPosition centerGeoPosition;

        void Awake()
        {

        }

        public float GetMinHeightForObject(UnityEngine.GameObject objectToMove)
        {
            float newHeight = 0.0f;
            float height = 0.0f;
            UnityEngine.GameObject realObject;
            float x = 0.0f;
            float z = 0.0f;
            foreach (Transform child in objectToMove.transform)
            {
                realObject = child.gameObject;
                x = realObject.transform.position.x;
                z = realObject.transform.position.z;
                Vector3 box = objectToMove.GetComponentInChildren<Collider>().bounds.size;
                box = box / 2;

                // checking bounds (no loop)
                height = GetHeight(x, z);
                newHeight = GetHeight(x - box.x, z - box.z);
                if (newHeight > height)
                    height = newHeight;
                newHeight = GetHeight(x + box.x, z - box.z);
                if (newHeight > height)
                    height = newHeight;
                newHeight = GetHeight(x - box.x, z + box.z);
                if (newHeight > height)
                    height = newHeight;
                newHeight = GetHeight(x + box.x, z + box.z);
                if (newHeight > height)
                    height = newHeight;
                break; // should be only one object
            }

            return height + 0.0000001f;
        }

        public float GetHeight(float x, float z)
        {
            return GetHeight(x, z, terrainObjectsLayer);
        }

        public float GetTerrainHeight(float x, float z)
        {
            return GetHeight(x, z, terrainLayer);
        }

        private float GetHeight(float x, float z, LayerMask mask)
        {
            Vector3 rayOrigin = new Vector3(x, 110, z);

            Ray ray = new Ray(rayOrigin, Vector3.down);
            float distanceToCheck = 120f;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheck, mask))
                return hit.point.y;
            return 0.0f;
        }

    }

}
