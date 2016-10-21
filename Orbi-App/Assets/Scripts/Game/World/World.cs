using System;
using ServerModel;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameScene;

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
        public Texture2D defaultGroundTexture;

        private bool skipRefresh = false;
        private TerrainService terrainService;
        private GeoPosition centerGeoPosition;
        private MapTextureService textureService;
        private GameObjectService gameObjectService;
        private UMACreator umaCreator;
        private ServerModel.Statistics stats = new Statistics();


        void Start()
        {
            Invoke("RefreshObjects", 2f);
        }

        void OnEnable()
        {
            // Register Map-ity's Loaded Event
            Mapity.MapityLoaded += OnMapityLoaded;
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        void OnDisable()
        {
            // Un-Register Map-ity's Loaded Event
            Mapity.MapityLoaded -= OnMapityLoaded;
        }

        void Awake()
        {
            this.backGroundLayerMask = backgroundLayersTerrain;
            this.terrainService = new TerrainService(terrain);
            this.textureService = new MapTextureService();
            this.gameObjectService = new GameObjectService();
            this.umaCreator = this.GetComponent<UMACreator>();
            
        }

        internal void SetStatistics(Statistics stats)
        {
            this.stats = stats;
        }

        internal Statistics GetStatistics()
        {
            return this.stats;
        }

        public IEnumerator UpdateObjects()
        {
            if (Game.GetLocation().IsReady())
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

        public UMACreator GetUMACreator()
        {
            return this.umaCreator;
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

                Mapity.Singleton.Load();
               // yield return textureService.LoadTexture();
                if (Game.GetGame().GetSettings().IsHeightsEnabled())
                {
                    //Debug.Log("heights");
                    //yield return terrainService.RequestTerrain();
                }
                else
                {
                    yield return terrainService.ResetTerrain();
                }
            }
        }

        public void SetCenterGeoPosition(GeoPosition centerGeoPosition)
        {
            Mapity.Singleton.SetLongitude(centerGeoPosition.longitude.ToString());
            Mapity.Singleton.SetLattitude(centerGeoPosition.latitude.ToString());
            this.centerGeoPosition = new GeoPosition(centerGeoPosition);
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
                //Debug.DrawLine(rayOrigin, hit.point, Color.cyan);
                return hit.point.y;
            }
            
            return 0.0f;
        }

        public void ForceRefreshOnNextLoading()
        {
            this.skipRefresh = false;
        }

        public void SkipRefreshOnNextLoading()
        {
            this.skipRefresh = true;
        }

        void OnDestroy()
        {
            // cleanup dynamic splats
            CancelInvoke();
            GetTerrainService().CleanSplats();
        }


        void OnMapityLoaded()
        {
            // Example #1
            // Loop over all the roads in Map-ity

            Mapity.MapWay mapWay = null;
            float[,,] maps = GetTerrainService().GetAlphaMaps();
            //GetTerrainService().Paint(maps, 256, 256, TerrainService.L_GRAS);
            for (var mapWayEnumerator = Mapity.Singleton.mapWays.Values.GetEnumerator(); mapWayEnumerator.MoveNext();)
            {
                mapWay = mapWayEnumerator.Current as Mapity.MapWay;

                if (mapWay.tags != null)
                {
                    if (mapWay.tags.GetTag("name") != null)
                    {
                        for (int j = 0; j < (mapWay.wayMapNodes.Count-1); j++)
                        {
                            // Get the node
                            Mapity.MapNode node_start = (Mapity.MapNode)mapWay.wayMapNodes[j];
                            Mapity.MapNode node_end = (Mapity.MapNode)mapWay.wayMapNodes[j+1];
                            ClientModel.Position position_start = new ClientModel.Position(node_start.position.world);
                            ClientModel.Position position_end = new ClientModel.Position(node_end.position.world);
                            //Debug.Log(mapWay.tags.GetTag("name").ToString() + "start: " + position_start);
                            //Debug.Log(mapWay.tags.GetTag("name").ToString() + "end: " + position_end);
                            
                            Vector3 end = position_end.ToVector3();
                            Vector3 wayPoint = position_start.ToVector3();
                            float distance = Vector3.Distance(wayPoint, end) * 10;
                            int run = 0;
                            //Debug.Log("dt" + distance);
                            while (Vector3.SqrMagnitude(wayPoint - end) > 0.5)
                            {
                                run++;
                                wayPoint = Vector3.Lerp(wayPoint, end, 1/distance);
                                /*
                                UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                                cube.transform.position = wayPoint;
                                cube.transform.parent = UnityEngine.GameObject.Find("Objects").transform;
                                cube.name = mapWay.tags.GetTag("name").ToString();
                                */
                                ClientModel.Position position = new ClientModel.Position(wayPoint);
                                Vector2 coordinates = GetTerrainService().GetAlphaMapCoordinates(position);
                                    //Debug.Log(coordinates);
                                GetTerrainService().Paint(maps, (int)coordinates.y, (int)coordinates.x, TerrainService.L_STREET);
                            }
                            //Debug.Log(mapWay.tags.GetTag("name").ToString() + "start: " + position_start + " runs: " + run);

                            /*
                            for (int cx = (int)position_start.x; cx < (int)position_end.x; cx = cx + moveX)
                            {
                                for (int cz = (int)position_start.z; cz < (int)position_end.z; cz = cz + moveY)
                                {
                                    if (GetTerrainService().IsInsideTerrain(cx,cz))
                                    {
                                        if ((cx % 20 == 0) && (cz % 20 == 0))
                                        {
                                            UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                                            cube.transform.position = new Vector3(cx, 0, cz);
                                            cube.transform.parent = UnityEngine.GameObject.Find("Objects").transform;
                                            cube.name = mapWay.tags.GetTag("name").ToString();
                                        }
                                        
                                        Vector2 coordinates = GetTerrainService().GetAlphaMapCoordinates(new ClientModel.Position(cx, 0, cz));

                                        GetTerrainService().Paint(maps, (int)coordinates.x, (int)coordinates.y, TerrainService.L_ROCK);
                                    }
                                }
                            }
                            */
                            //Debug.Log(node.position.world.ToString());
                        }
                        //Debug.Log(mapWay.tags.GetTag("name").ToString());
                    }
                }
            }
            GetTerrainService().SetAlphaMaps(maps);
            GetTerrainService().GetTerrain().Flush();

            // Example #2
            // Loop over all the buildings in Map-ity

            Mapity.MapBuilding mapBuilding = null;
            /*
            for (var mapBuildingEnumerator = Mapity.Singleton.mapBuildings.Values.GetEnumerator(); mapBuildingEnumerator.MoveNext();)
            {
                mapBuilding = mapBuildingEnumerator.Current as Mapity.MapBuilding;

                // Log building id
                Debug.Log("Building: " + mapBuilding.id.ToString());

                // Loop over the ways which define this building(usually 1)
                for (int i = 0; i < mapBuilding.buildingWays.Count; i++)
                {
                    // Get the way
                    Mapity.MapWay buildingWay = (Mapity.MapWay)mapBuilding.buildingWays[i];

                    // NULL check
                    if (buildingWay != null)
                    {
                        if (buildingWay.tags != null)
                        {
                            if (buildingWay.tags.GetTag("name") != null)
                            {
                               // Debug.Log(buildingWay.tags.GetTag("name").ToString());
                            }
                        }

                        // Loop over the nodes
                        for (int j = 0; j < buildingWay.wayMapNodes.Count; j++)
                        {
                            // Get the node
                            Mapity.MapNode node = (Mapity.MapNode)buildingWay.wayMapNodes[j];

                            //Debug.Log(node.position.world.ToString());
                        }
                    }
                }
            }
            */

        }

    }

}
