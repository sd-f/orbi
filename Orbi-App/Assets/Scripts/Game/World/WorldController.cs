﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameScene;

namespace GameController
{

    [AddComponentMenu("App/Game/WorldCopy")]
    public class WorldController : GameMonoBehaviour
    {

        public LayerMask terrainLayer;
        public LayerMask terrainObjectsLayer;
        public Terrain terrain;
        public LayerMask backgroundLayersTerrain;
        public LayerMask backgroundLayersCamera;
        public LayerMask backGroundLayerMask;

        public MapTextureService textureService;
        public GameObjectService gameObjectService;
        public TerrainService terrainService;

        public UnityEngine.GameObject streetLabelPrefab;
        public UnityEngine.GameObject mapContainer;

        private ServerModel.GeoPosition centerGeoPosition;
        private UMACreator umaCreator;
        private ServerModel.Statistics stats = new ServerModel.Statistics();

        public override void OnReady()
        {
            base.OnReady();
            Invoke("RefreshObjects", 2f);
        }


        public override void OnEnable()
        {
            base.OnEnable();
            Mapity.MapityLoaded += OnMapityLoaded;
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        public void OnDisable()
        {
            Mapity.MapityLoaded -= OnMapityLoaded;
        }

        public override void Awake()
        {
            base.Awake();
            this.backGroundLayerMask = backgroundLayersTerrain;
            this.umaCreator = this.GetComponent<UMACreator>();

        }

        public void SetStatistics(ServerModel.Statistics stats)
        {
            this.stats = stats;
        }

        public ServerModel.Statistics GetStatistics()
        {
            return this.stats;
        }

        public IEnumerator UpdateObjects()
        {
            if (Game.Instance.GetLocation().IsReady())
                yield return gameObjectService.RequestGameObjects();
            if (!IsInvoking("RefreshObjects"))
            {
                Invoke("RefreshObjects", 2f);
            }
        }

        public void RefreshObjects()
        {
            StartCoroutine(UpdateObjects());
        }

        internal GameObjectService GetGameObjectService()
        {
            return this.gameObjectService;
        }

        public GameScene.UMACreator GetUMACreator()
        {
            return this.umaCreator;
        }

        public TerrainService GetTerrainService()
        {
            return this.terrainService;
        }

        public IEnumerator UpdateWorld()
        {

            Mapity.Singleton.Unload();
            yield return Mapity.Singleton.LoadMap();

        }

        public void SetCenterGeoPosition(ServerModel.GeoPosition centerGeoPosition)
        {
            if (!centerGeoPosition.Equals(this.centerGeoPosition))
            {
                Mapity.Singleton.SetLongitude(centerGeoPosition.longitude.ToString());
                Mapity.Singleton.SetLattitude(centerGeoPosition.latitude.ToString());

                this.centerGeoPosition = new ServerModel.GeoPosition(centerGeoPosition);

                GetGameObjectService().ClearAll();
                StartCoroutine(UpdateWorld());
            }
        }

        public ServerModel.GeoPosition GetCenterGeoPostion()
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
                return height;
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
                //Debug.DrawLine(rayOrigin, hit.point, Color.cyan);
                return hit.point.y;
            }

            return 0.0f;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            // cleanup dynamic splats
            CancelInvoke();
            GetTerrainService().CleanSplats();
        }

        void OnMapityLoaded()
        {
            //Debug.Log("OnMapityLoaded");
            StartCoroutine( GetTerrainService().PaintTerrainFromMapity() );
        }


    }
}
