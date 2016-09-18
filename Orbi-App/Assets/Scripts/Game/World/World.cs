using System;
using ServerModel;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameController
{

    [AddComponentMenu("App/Game/World")]
    public class World : MonoBehaviour
    {
        
        public LayerMask terrainLayer;
        public LayerMask terrainObjectsLayer;
        public Terrain terrain;
        public LayerMask backgroundLayersTerrain;
        public LayerMask backgroundLayersCamera;
        public LayerMask backGroundLayerMask;

        private bool skipRefresh = false;
        private TerrainService terrainService;
        private GeoPosition centerGeoPosition;
        private MapTextureService textureService;
        private GameObjectService gameObjectService;

        void Awake()
        {
            this.backGroundLayerMask = backgroundLayersTerrain;
            this.terrainService = new TerrainService(terrain);
            this.textureService = new MapTextureService();
            this.gameObjectService = new GameObjectService();
            InvokeRepeating("RefreshObjects", 1f, 2f);
        }

        public IEnumerator UpdateObjects()
        {
            yield return gameObjectService.RequestGameObjects();
        }

        public void RefreshObjects()
        {
            if (Game.GetLocation().IsReady())
            {
                StartCoroutine(UpdateObjects());
            }
                
        }

        public TerrainService GetTerrainService()
        {
            return this.terrainService;
        }

        public IEnumerator UpdateWorld()
        {
            if (skipRefresh)
            {
                skipRefresh = false;
            } else
            {
                yield return textureService.LoadTextures();
                if (Game.GetGame().GetSettings().IsHeightsEnabled())
                {
                    //Debug.Log("heights");
                    yield return terrainService.RequestTerrain();
                }
                else
                {
                    yield return terrainService.ResetTerrain();
                }
            }
        }

        public void SetCenterGeoPosition(GeoPosition centerGeoPosition)
        {
            this.centerGeoPosition = centerGeoPosition;
        }

        public GeoPosition GetCenterGeoPostion()
        {
            return this.centerGeoPosition;
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

        public float GetHeight(double x, double z)
        {
            return GetHeight((float)x, (float)z);
        }

        public float GetHeight(float x, float z)
        {
            return GetHeight(x, z, terrainObjectsLayer);
        }

        public float GetTerrainHeight(double x, double z)
        {
            return GetTerrainHeight((float)x, (float)z);
        }

        public float GetTerrainHeight(float x, float z)
        {
            return GetHeight(x, z, terrainLayer);
        }

        private float GetHeight(float x, float z, LayerMask mask)
        {
            Vector3 rayOrigin = new Vector3(x, 300, z);
            
            Ray ray = new Ray(rayOrigin, Vector3.down);
            float distanceToCheck = 400f;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheck, mask))
            {
                return hit.point.y;
            }
                
            return 0.0f;
        }

        public void SkipRefreshOnNextLoading()
        {
            this.skipRefresh = true;
        }

        void OnDestroy()
        {
            // cleanup dynamic splats
            GetTerrainService().SetMapsSplats(new SortedList<int, SplatPrototype>());
        }


    }

}
