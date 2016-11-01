using CanvasUtility;
using ClientModel;
using GameController.Services;
using ServerModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController
{
    public class TerrainService: AbstractHttpService
    {
        public static int TEXTURE_RASTER_SIZE = 2;

        //private static int HEIGHTMAP_SIZE_SERVER = 33;

        //private double heightMax = 0d;
        private double heightMin = 100000d;

        private int hmSize;         // 33
        //private int hmSizeX;         // 33
        //private int hmSizeY;         // 33
        private int amSizeY;         // 256
        private int amSizeX;         // 256
        public int terrainSize;     // 256
        public int terrainHeight;   // 100

        public TerrainData td;
        public Terrain terrain;
        public LayerMask terrainMask;

        private float[,] hm;
        private float[,,] alphaMaps;
        public static int L_WHITE = 1;
        public static int L_BLACK = 0;
        public static int L_GROUND = 2;
        public static int L_GRAS = 3;
        public static int L_STREET = 4;
        public static int SMOOTH_PATCH_SIZE = 3;
        public static float SMOOTH_MAX = SMOOTH_PATCH_SIZE * 2f;
        private static int NUMBER_OF_USER_LAYERS = 3;
        private static int USER_LAYER_START_INDEX = 2;
        private static int USER_LAYERS_END_INDEX = USER_LAYER_START_INDEX + NUMBER_OF_USER_LAYERS;


        void Start()
        {
            this.hmSize = td.heightmapHeight;
            this.amSizeY = td.alphamapHeight;
            this.amSizeX = td.alphamapWidth;
            //Game.Instance.GetClient().Log("AlphaMap Resolution " + this.amSize);
            this.terrainSize = (int)td.size.x;
            this.terrainHeight = (int)td.size.y;
            this.hm = new float[hmSize, hmSize];
            this.alphaMaps = td.GetAlphamaps(0, 0, amSizeX, amSizeY);
            ResetAlpha();
        }

        public bool IsInsideTerrain(int x, int z)
        {
            return (x < td.alphamapWidth) && (x > 0) && (z < td.alphamapHeight) && (z > 0);
        }

        public Vector2 GetAlphaMapCoordinates(Position worldPosition)
        {
            int mapX = (int)(((worldPosition.x + 256) / td.size.x) * td.alphamapWidth);
            int mapZ = (int)(((worldPosition.z + 256) / td.size.z) * td.alphamapHeight);
            return new Vector2(mapX, mapZ);
        }

        public void Paint(int x, int y, int layer)
        {
            PaintAround(x, y, layer);
            //Paint(x, y, layer, 1.0f);
            //
        }


        public void Paint(int x, int y, int layer, float amount)
        {
            if (IsInsideTerrain(x, y))
            {
                float rest = (1.0f - amount) / (NUMBER_OF_USER_LAYERS - 1.0f); // fix
                for (int l = USER_LAYER_START_INDEX; l < (USER_LAYERS_END_INDEX); l++)
                    if (l == layer)
                        alphaMaps[x, y, l] = amount;
                    else
                        if (amount != 1.0f) // tuning
                            alphaMaps[x, y, l] = alphaMaps[x, y, l] * rest;
                        else
                            alphaMaps[x, y, l] = 0.0f;
            }
        }


        private void PaintAround(int x, int y, int layer)
        {
            float amount = 1;
            float distance;
            for (int h = -SMOOTH_PATCH_SIZE; h<= SMOOTH_PATCH_SIZE; h++) 
                for (int v = -SMOOTH_PATCH_SIZE; v<= SMOOTH_PATCH_SIZE; v++) {
                    distance = (Math.Abs(h) + Math.Abs(v));
                    amount = 1.0f;
                    if (distance != 0)
                        amount = 1.0f - (0.5f * (distance / SMOOTH_MAX));
                    Paint(x + v, y + h, layer, amount);
                }

        }

        public void SetAlphaMaps(float[,,] maps)
        {
            terrain.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void ResetAlpha()
        {
            for (int y = 0; y < amSizeY; y++)
                for (int x = 0; x < amSizeX; x++)
                {
                    alphaMaps[x, y, L_WHITE] = 0f;
                    alphaMaps[x, y, L_BLACK] = 0f;
                    Paint(x, y, L_GRAS, 1.0f);
                }
                    
        }

        private void PaintForest(List<Vector2> poly)
        {
            for (int y = 0; y < amSizeY; y++)
                for (int x = 0; x < amSizeX; x++)
                    if (IsInPolygon(poly, new Vector2(x,y)))
                        Paint(x, y, L_GRAS, 1.0f);
                    
        }


        internal void CleanSplats()
        {
            ResetAlpha();
        }

        public IEnumerator ResetTerrain()
        {
            SetHeightMin(0.0f);
            SetHeightsToMin();
            ResetAlpha();
            Flush();
            yield return null;
        }

        public Terrain GetTerrain()
        {
            return this.terrain;
        }

        private void SetHeights()
        {
            td.SetHeightsDelayLOD(0, 0, hm);
            td.SetHeights(0, 0, hm);
        }

        public void SetHeightMin(double min)
        {
            this.heightMin = min;
        }

        public void SetHeightsToMin()
        {
            for (int x = 0; x < hmSize; x++)
                for (int y = 0; y < hmSize; y++)
                {
                    hm[x, y] = (float)heightMin;
                }
            SetHeights();
        }

        public void Flush()
        {
            //Normalize(alphaMaps);
            SetAlphaMaps(alphaMaps);
            terrain.Flush();
        }

        private void Normalize(float[,,] maps)
        {
            float maxValue = 1;
            float debugFixed = 0;
            for (int x = 0; x < amSizeX; x++)
                for (int y = 0; y < amSizeY; y++)
                {
                    maxValue = 0;
                    for (int l = 0; l < NUMBER_OF_USER_LAYERS; l++)
                        if (maxValue < maps[x, y, l])
                            maxValue = maps[x, y, l];
                    if (maxValue > 1)
                    {
                        debugFixed++;
                        for (int l = 0; l < NUMBER_OF_USER_LAYERS; l++)
                            maps[x, y, l] = maps[x, y, l] / maxValue;
                    }
                    /*
                    if (maxValue < 0)
                    {
                        maxValue = 1 / maxValue;
                        for (int l = 0; l < NUM_L; l++)
                            maps[x, y, l] = maps[x, y, l] * maxValue;
                    }
                    */
                        
                }

            //Debug.Log("Normalize Alphamap: " + debugFixed);
        }

        public void PaintTerrainFromMapity()
        {
            ResetAlpha();
            // Example #1
            // Loop over all the roads in Map-ity
            UnityEngine.GameObject streetLabelPrefab = Game.Instance.GetWorld().streetLabelPrefab;
            UnityEngine.GameObject mapContainer = Game.Instance.GetWorld().mapContainer;
            GameObjectUtility.DestroyAllChildObjects(mapContainer);
            Mapity.MapWay mapWay = null;
            //GetTerrainService().Paint(maps, 256, 256, TerrainService.L_GRAS);
            for (var mapWayEnumerator = Mapity.Singleton.mapWays.Values.GetEnumerator(); mapWayEnumerator.MoveNext();)
            {
                mapWay = mapWayEnumerator.Current as Mapity.MapWay;

                if (mapWay.tags != null)
                {
                    if (mapWay.tags.GetTag("name") != null)
                    {
                        for (int j = 0; j < (mapWay.wayMapNodes.Count - 1); j++)
                        {
                            // Get the node
                            Mapity.MapNode node_start = (Mapity.MapNode)mapWay.wayMapNodes[j];
                            Mapity.MapNode node_end = (Mapity.MapNode)mapWay.wayMapNodes[j + 1];
                            ClientModel.Position position_start = new ClientModel.Position(node_start.position.world);
                            ClientModel.Position position_end = new ClientModel.Position(node_end.position.world);
                            //Debug.Log(mapWay.tags.GetTag("name").ToString() + "start: " + position_start);
                            //Debug.Log(mapWay.tags.GetTag("name").ToString() + "end: " + position_end);
                            string name = mapWay.tags.GetTag("name").ToString();
                            Vector3 end = position_end.ToVector3();
                            Vector3 wayPoint = position_start.ToVector3();
                            float distance = Vector3.Distance(wayPoint, end);
                            //Debug.Log(name + " dt" + distance + " l " + name.Length);
                            Vector3 stepVector = (end - wayPoint).normalized;

                            for (int step = 1; step <= (int) (distance*4); step ++)
                            {
                                wayPoint = wayPoint + (stepVector * 0.25f);
                                if (((int)step) % (100 + name.Length) == 0)
                                {
                                    UnityEngine.GameObject streetLabel = UnityEngine.GameObject.Instantiate(streetLabelPrefab, mapContainer.transform) as UnityEngine.GameObject;
                                    streetLabel.GetComponentInChildren<TextMesh>().text = name;
                                    streetLabel.transform.localPosition = new Vector3(wayPoint.x, 0, wayPoint.z);
                                    streetLabel.transform.localRotation = Quaternion.LookRotation(stepVector);
                                    streetLabel.transform.Rotate(new Vector3(0f,90f,0f));

                                    UnityEngine.GameObject streetLabelReverse = UnityEngine.GameObject.Instantiate(streetLabelPrefab, mapContainer.transform) as UnityEngine.GameObject;
                                    streetLabelReverse.GetComponentInChildren<TextMesh>().text = name;
                                    streetLabelReverse.transform.localPosition = new Vector3(wayPoint.x, 0, wayPoint.z);
                                    streetLabelReverse.transform.localRotation = Quaternion.LookRotation(stepVector);
                                    streetLabelReverse.transform.Rotate(new Vector3(0f, -90f, 0f));
                                }
                                ClientModel.Position position = new ClientModel.Position(wayPoint);
                                Vector2 coordinates = GetAlphaMapCoordinates(position);
                                //Debug.Log(coordinates);
                                Paint((int)coordinates.y, (int)coordinates.x, TerrainService.L_STREET);
                            }
                            
                        }
                        //Debug.Log(mapWay.tags.GetTag("name").ToString());
                    }
                }
            }
            
            /* forest
            Mapity.MapRelation relation = null;
            

            for (IEnumerator relationsIterator = Mapity.Singleton.mapRelations.Values.GetEnumerator(); relationsIterator.MoveNext();)
            {
                relation = relationsIterator.Current as Mapity.MapRelation;
                if (relation.tags.GetTag("landuse") != null && relation.tags.GetTag("landuse").Equals("forest") )
                    //&& relation.tags.GetTag("type") != null && relation.tags.GetTag("type").Equals("polygon") ) // TODO type=multipolygon
                {
                    mapWay = null;
                    Debug.Log("Forest found");
                    //GetTerrainService().Paint(maps, 256, 256, TerrainService.L_GRAS);
                    
                    for (var mapWayEnumerator = relation.relationWays.GetEnumerator(); mapWayEnumerator.MoveNext();)
                    {
                        mapWay = mapWayEnumerator.Current as Mapity.MapWay;
                        
                        List<Vector2> poly = new List<Vector2>();
                        if (mapWay != null)
                        {
                            for (int j = 0; j < (mapWay.wayMapNodes.Count - 1); j++)
                            {
                                Mapity.MapNode node = (Mapity.MapNode)mapWay.wayMapNodes[j];
                                poly.Add(new ClientModel.Position(node.position.world).ToVector2());
                            }
                        }
                           
                            
                        if (poly.Count > 0)
                        {
                            PaintForest(poly);
                            return;
                        }
                            
                           
                    }
                }
            }
            */
            Flush();

            // Example #2
            // Loop over all the buildings in Map-ity

            /*Mapity.MapBuilding mapBuilding = null;
            
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

        public static bool IsInPolygon(List<Vector2> poly, Vector2 p)
        {
            Vector2 p1, p2;
            bool inside = false;
            if (poly.Count < 3)
                return inside;

            var oldPoint = new Vector2(
                poly[poly.Count - 1].x, poly[poly.Count - 1].y);
            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new Vector2(poly[i].y, poly[i].y);
                if (newPoint.x > oldPoint.x)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }
                if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
                    && (p.y - (long)p1.y) * (p2.x - p1.x)
                    < (p2.y - (long)p1.y) * (p.x - p1.x))
                {
                    inside = !inside;
                }
                oldPoint = newPoint;
            }
            return inside;
        }

        public float GetTerrainHeight(float x, float z)
        {
            Vector3 rayOrigin = new Vector3(x, 600, z);

            Ray ray = new Ray(rayOrigin, Vector3.down);
            float distanceToCheck = 600;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheck, terrainMask))
                return hit.point.y;
            return 0.0f;
        }

        public Vector3 ClampPosition(Vector3 vector3)
        {
            float boundX = (terrainSize / 2f);
            float boundY = 600;
            float boundZ = boundX;
            return new Vector3(Mathf.Clamp(vector3.x, -boundX, +boundX), Mathf.Clamp(vector3.y, 0.00001f, boundY), Mathf.Clamp(vector3.z, -boundY, +boundY));
        }
    }



}
