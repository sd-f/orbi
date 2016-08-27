using Assets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Control.services
{
    class TerrainService: AbstractService
    {
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

        WorldAdapter adapter;

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
            adapter = new WorldAdapter(this);
        }

        public IEnumerator RequestTerrain(Player player)
        {
            World generatedWorld = GenerateDummyWorldArround(player);
            generatedWorld.clientVersion = Game.GetInstance().player.clientVersion;
            WWW request = Request("world/terrain", JsonUtility.ToJson(generatedWorld));
            yield return request;
            if (request.error == null)
            {
                World terrainWorld = JsonUtility.FromJson<World>(request.text);
                try
                {
                    IndicateRequestFinished();
                    AdjustTerrainHeights(terrainWorld, player);
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

        public double getMinHeight()
        {
            return this.heightMin;
        }

        public void setTexture(Texture2D texture, int layerIndex)
        {
            SplatPrototype[] splats = td.splatPrototypes;
            splats[0].texture = texture;
            td.splatPrototypes = splats;
            td.RefreshPrototypes();
            t.Flush();
        }

        public void ResetTerrain()
        {
            SetHeightMin(0.0f);
            SetHeightsToMin();
            
            t.Flush();
            AddStaticAlpha();
            t.Flush();
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

        public World GenerateDummyWorldArround(Player player)
        {
            World dummyWorld = new World();
            
            int factor = terrainSize / (HEIGHTMAP_SIZE_SERVER - 1);
            // 0 .. 17 -> -8 .. +8
            for (int i = 0; i < HEIGHTMAP_SIZE_SERVER; i++)
                for (int j = 0; j < HEIGHTMAP_SIZE_SERVER; j++)
                {
                    Model.GameObject dummyGameObject = new Model.GameObject();
                    GeoPosition dummyGeoLocation = new GeoPosition();
                    dummyGeoLocation.latitude = (i - (HEIGHTMAP_SIZE_SERVER / 2)) * factor;
                    dummyGeoLocation.longitude = (j - (HEIGHTMAP_SIZE_SERVER / 2)) * factor;
                    adapter.ToReal(dummyGeoLocation, player);
                    dummyGeoLocation.altitude = 0.0d;
                    dummyGameObject.geoPosition = dummyGeoLocation;
                    dummyWorld.gameObjects.Add(dummyGameObject);
                }

            return dummyWorld;
        }

        private void SetHeightsMinMaxFromDummyWorld(World dummyWorld)
        {
            foreach (Model.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                if (dummyGameObject.geoPosition.altitude < heightMin)
                    heightMin = dummyGameObject.geoPosition.altitude;
                if (dummyGameObject.geoPosition.altitude > heightMax)
                    heightMax = dummyGameObject.geoPosition.altitude;
            }
        }

        public void AdjustTerrainHeights(World dummyWorld, Player player)
        {
            SetHeightsMinMaxFromDummyWorld(dummyWorld);
            SetHeightsToMin();
            //Texture2D texture = new Texture2D(heightMapSizeForRequest, heightMapSizeForRequest);
            int x, y, vX, vY;
            int factor = terrainSize / (HEIGHTMAP_SIZE_SERVER - 1);
            float height = 0.0f;
            double altitude = 0.0f;

            foreach (Model.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                altitude = dummyGameObject.geoPosition.altitude - heightMin;
                adapter.ToVirtual(dummyGameObject.geoPosition, player);
                
                x = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().z);
                y = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().x);
                vX = (x / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                vY = (y / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                height = (float)(altitude / terrainHeight);
                if (height > 1)
                    height = 1;
                if ((vX <= hmSize) && (vY <= hmSize) && (vX >= 0) && (vY >= 0))
                    hm[vX, vY] = height;
            }
            
            SetHeights();
        }

        private void AddStaticAlpha()
        {
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, amSize, amSize);
            for (int y = 0; y < amSize; y++)
                for (int x = 0; x < amSize; x++)
                {
                    maps[x, y, 0] = 1;
                    maps[x, y, 1] = 0.2f;
                }
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        public float GetTerrainHeight(float x, float z)
        {
            Vector3 rayOrigin = new Vector3(x, 110, z);

            Ray ray = new Ray(rayOrigin, Vector3.down);
            float distanceToCheck = 120f;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheck, terrainMask))
                return hit.point.y;
            return 0.0f;
        }
    }



}
