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

        private static int HEIGHTMAP_SIZE_SERVER = 33;

        private double heightMax = 0d;
        private double heightMin = 100000d;

        private int hmSize;         // 33
        private int amSize;         // 256
        public int terrainSize;     // 256
        public int terrainHeight;   // 100

        public TerrainData td;
        private Terrain t;
        private LayerMask terrainMask;

        private float[,] hm;

        public static int L_GROUND = 0;
        public static int L_GRAS = 1;
        public static int L_STREET = 2;
        public static int smoothArea = 3;
        private static int NUM_L = 3;


        public TerrainService(Terrain terrain)
        {
            this.t = terrain;
            this.td = t.terrainData;
            terrainMask = 1 << LayerMask.NameToLayer("Terrain");
            this.hmSize = td.heightmapResolution;
            this.amSize = td.alphamapResolution;
            //Game.GetClient().Log("AlphaMap Resolution " + this.amSize);
            this.terrainSize = (int)td.size.x;
            this.terrainHeight = (int)td.size.y;
            this.hm = new float[hmSize, hmSize];
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

        public void Paint(float[,,] maps, int x, int y, int layer)
        {
            Paint(maps, x, y, layer, 1.0f);
            PaintAround(maps, x, y, layer);
        }

        public void Paint(float[,,] maps, int x, int y, int layer, float amount)
        {
            if (IsInsideTerrain(x, y))
            {
                float rest = (1 - amount) / (NUM_L - 1);
                for (int l = 0; l < NUM_L; l++)
                    if (l == layer)
                        maps[x, y, l] = amount;
                    else
                        maps[x, y, l] = maps[x, y, l] * rest;
            }
        }

        private void PaintAround(float[,,] maps, int x, int y, int layer)
        {
            float amount;
            for (int h = -smoothArea; h<= smoothArea; h++) 
                for (int v = -smoothArea; v<= smoothArea; v++)
                    if ((h!=0) && (v!=0))
                    {
                        amount = (smoothArea*2) / (Math.Abs(h) + Math.Abs(v));
                        PaintSmooth(maps, x + v, y + h, layer, amount);
                    }
                    
        }

        private void PaintSmooth(float[,,] maps, int x, int y, int layer, float amount)
        {
            if (IsInsideTerrain(x, y))
                if (maps[x, y, layer] != 1.0f)
                    Paint(maps, x, y, layer, amount);
        }

        public float[,,] GetAlphaMaps()
        {
           return td.GetAlphamaps(0, 0, amSize, amSize);
        }

        public void SetAlphaMaps(float[,,] maps)
        {
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void ResetAlpha()
        {
            float[,,] maps = GetAlphaMaps();
            for (int y = 0; y < amSize; y++)
                for (int x = 0; x < amSize; x++)
                    Paint(maps, x, y, L_GROUND, 1.0f);
            SetAlphaMaps(maps);
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
            t.Flush();
            yield return null;
        }

        public Terrain GetTerrain()
        {
            return this.t;
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

        public void PaintTerrainFromMapity()
        {
            // Example #1
            // Loop over all the roads in Map-ity
            UnityEngine.GameObject streetLabelPrefab = Game.GetWorld().streetLabelPrefab;
            UnityEngine.GameObject mapContainer = Game.GetWorld().mapContainer;
            GameObjectUtility.DestroyAllChildObjects(mapContainer);
            Mapity.MapWay mapWay = null;
            float[,,] maps = GetAlphaMaps();
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
                            Debug.Log(name + " dt" + distance + " l " + name.Length);
                            Vector3 stepVector = (end - wayPoint).normalized;

                            for (int step = 1; step <= (int) (distance*4); step ++)
                            {
                                wayPoint = wayPoint + (stepVector * 0.25f);
                                if (((int)step) % (100 + name.Length) == 0)
                                {
                                    UnityEngine.GameObject streetLabel = UnityEngine.GameObject.Instantiate(streetLabelPrefab, mapContainer.transform) as UnityEngine.GameObject;
                                    streetLabel.GetComponent<TextMesh>().text = name;
                                    streetLabel.transform.localPosition = new Vector3(wayPoint.x, 3, wayPoint.z);
                                    streetLabel.transform.localRotation = Quaternion.LookRotation(stepVector);
                                    streetLabel.transform.Rotate(new Vector3(0f,90f,0f));

                                    UnityEngine.GameObject streetLabelReverse = UnityEngine.GameObject.Instantiate(streetLabelPrefab, mapContainer.transform) as UnityEngine.GameObject;
                                    streetLabelReverse.GetComponent<TextMesh>().text = name;
                                    streetLabelReverse.transform.localPosition = new Vector3(wayPoint.x, 3, wayPoint.z);
                                    streetLabelReverse.transform.localRotation = Quaternion.LookRotation(stepVector);
                                    streetLabelReverse.transform.Rotate(new Vector3(0f, -90f, 0f));
                                }
                                ClientModel.Position position = new ClientModel.Position(wayPoint);
                                Vector2 coordinates = GetAlphaMapCoordinates(position);
                                //Debug.Log(coordinates);
                                Paint(maps, (int)coordinates.y, (int)coordinates.x, TerrainService.L_STREET);
                            }
                            
                        }
                        //Debug.Log(mapWay.tags.GetTag("name").ToString());
                    }
                }
            }
            SetAlphaMaps(maps);
            GetTerrain().Flush();

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
    }



}
