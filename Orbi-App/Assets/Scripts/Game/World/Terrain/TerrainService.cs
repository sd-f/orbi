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

        private TerrainData td;
        private Terrain t;
        private LayerMask terrainMask;

        private float[,] hm;


        public TerrainService(Terrain terrain)
        {
            this.t = terrain;
            this.td = t.terrainData;
            terrainMask = 1 << LayerMask.NameToLayer("Terrain");
            this.hmSize = td.heightmapResolution;
            this.amSize = td.alphamapResolution;
            this.terrainSize = (int)td.size.x;
            this.terrainHeight = (int)td.size.y;
            this.hm = new float[hmSize, hmSize];
        }

        public IEnumerator RequestTerrain()
        {
            ServerModel.World generatedWorld = GenerateDummyWorldArround();
            WWW request = Request("world/terrain", JsonUtility.ToJson(generatedWorld));
            yield return request;
            if (request.error == null)
            {
                ServerModel.World terrainWorld = JsonUtility.FromJson<ServerModel.World>(request.text);
                try
                {
                    IndicateRequestFinished();
                    //AdjustTerrainHeights(terrainWorld);
                    AddStaticAlpha();
                    
                    t.Flush();
                }
                catch (Exception ex) {
                    Error.Show(ex.Message);
                }    
            }
            else
                HandleError(request);
            
            
        }

        

        public ServerModel.World GenerateDummyWorldArround()
        {
            ServerModel.World dummyWorld = new ServerModel.World();
            
            int factor = terrainSize / (HEIGHTMAP_SIZE_SERVER - 1);
            // 0 .. 17 -> -8 .. +8
            for (int i = 0; i < HEIGHTMAP_SIZE_SERVER; i++)
                for (int j = 0; j < HEIGHTMAP_SIZE_SERVER; j++)
                {
                    ServerModel.GameObject dummyGameObject = new ServerModel.GameObject();
                    Position dummyGeoLocation = new Position();
                    dummyGeoLocation.z = (i - (HEIGHTMAP_SIZE_SERVER / 2)) * factor;
                    dummyGeoLocation.x = (j - (HEIGHTMAP_SIZE_SERVER / 2)) * factor;
                    dummyGeoLocation.y = 0.0d;
                    dummyGameObject.geoPosition = dummyGeoLocation.ToGeoPosition();
                    //Debug.Log(dummyGameObject.geoPosition);
                    dummyWorld.gameObjects.Add(dummyGameObject);
                }

            return dummyWorld;
        }

        private void SetHeightsMinMaxFromDummyWorld(ServerModel.World dummyWorld)
        {
            heightMin = 100000d;
            heightMax = 0d;
            foreach (ServerModel.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                if (dummyGameObject.geoPosition.altitude < heightMin)
                    heightMin = dummyGameObject.geoPosition.altitude;
                if (dummyGameObject.geoPosition.altitude > heightMax)
                    heightMax = dummyGameObject.geoPosition.altitude;
            }
        }

        public void AdjustTerrainHeights(ServerModel.World dummyWorld)
        {
            SetHeightsMinMaxFromDummyWorld(dummyWorld);
            SetHeightsToMin();
            //Texture2D texture = new Texture2D(heightMapSizeForRequest, heightMapSizeForRequest);
            int x, y, vX, vY;
            int factor = terrainSize / (HEIGHTMAP_SIZE_SERVER - 1);
            float height = 0.0f;
            double altitude = 0.0f;

            foreach (ServerModel.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                altitude = dummyGameObject.geoPosition.altitude - heightMin;
                Position pos = dummyGameObject.geoPosition.ToPosition();

                x = (int)Math.Round(pos.z);
                y = (int)Math.Round(pos.x);
                vX = (x / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                vY = (y / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                height = (float)(altitude / terrainHeight);
                //Debug.Log("(" + vX + "," + vY + ") " + altitude + " " + pos);
                if (height > 1)
                    height = 1;
                if ((vX <= hmSize) && (vY <= hmSize) && (vX >= 0) && (vY >= 0))
                {
                    hm[vX, vY] = height;
                    //Debug.Log("(" + vX + "," + vY + ") " + height);
                }
                    
            }
            
            SetHeights();
        }

        public double getMinHeight()
        {
            return this.heightMin;
        }


        public void SetMapsSplats(SortedList<int, SplatPrototype> prototypes)
        {
            SplatPrototype[] splats = td.splatPrototypes;
            List<SplatPrototype> newSplats = new List<SplatPrototype>();
            newSplats.Add(splats[0]); // grass
            foreach (KeyValuePair<int, SplatPrototype> pair in prototypes)
            {
                newSplats.Add(pair.Value);
            }
            
            td.splatPrototypes = newSplats.ToArray();
            td.RefreshPrototypes();
            t.Flush();
        }

        public IEnumerator ResetTerrain()
        {
            SetHeightMin(0.0f);
            SetHeightsToMin();
            AddStaticAlpha();
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

        private void AddStaticAlpha2()
        {
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, amSize, amSize);
            for (int y = 0; y < amSize; y++)
            {
                for (int x = 0; x < amSize; x++)
                {
                    maps[x, y, 1] = 0.9f;
                    maps[x, y, 0] = 0.1f;
                }
            }
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void AddStaticAlpha()
        {
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, amSize, amSize);
            int vermeindlicherLayer = 0;
            int index_helper_y = 0;
            int index_helper_x = 0;
            int bla = 0;
            for (int y = 0; y < amSize; y++) {
                if (y % 256 == 0)
                {
                    index_helper_y++;
                }
                for (int x = 0; x < amSize; x++)
                {
                    index_helper_x = index_helper_y + (x / 256);
                    if (x % 256 == 0)
                    {
                        bla = 2;

                    }

                    vermeindlicherLayer = index_helper_x + index_helper_y - 1;

                    // für vermeindlichen auf 0.8 sonst für alle anderen 0.0
                    for (int zusetzenderLayer = 1; zusetzenderLayer <= 4; zusetzenderLayer++)
                    {
                        if (zusetzenderLayer == (vermeindlicherLayer))
                            maps[y,x, zusetzenderLayer] = 0.9f;
                        else
                            maps[y,x, zusetzenderLayer] = 0f;
                    }
                   
                    maps[x, y, 0] = 0.1f;
                }
            }
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        public float GetTerrainHeight(float x, float z)
        {
            Vector3 rayOrigin = new Vector3(x, 300, z);

            Ray ray = new Ray(rayOrigin, Vector3.down);
            float distanceToCheck = 400;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheck, terrainMask))
                return hit.point.y;
            return 0.0f;
        }
    }



}
