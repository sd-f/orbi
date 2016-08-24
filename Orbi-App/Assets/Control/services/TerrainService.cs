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
            WWW request = Request("world/terrain", JsonUtility.ToJson(generatedWorld));
            yield return request;
            if (request.error == null)
            {
                World terrainWorld = JsonUtility.FromJson<World>(request.text);
                try
                {
                    AdjustTerrainHeights(terrainWorld, player);
                    AddStaticAlpha();
                    t.Flush();
                }
                catch (Exception ex) {
                    Error.Show(ex.Message);
                }    
            }
            else
                Error.Show(request.error);
            
            IndicateRequestFinished();
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
            //Debug.Log("min" + heightMin + "max" + heightMax);
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
                
                //UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = dummyGameObject.geoPosition.ToPosition().ToVector3();
                //cube.transform.localScale = new Vector3(2, 2, 2);
                x = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().z);
                y = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().x);
                vX = (x / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                vY = (y / factor) + ((HEIGHTMAP_SIZE_SERVER - 1) / 2);
                height = (float)(altitude / terrainHeight);
                //texture.SetPixel(vX, vY, new Color(height, 0.0f, 0.0f));
                hm[vX, vY] = height;
            }
            
            SetHeights();
        }

        private void AddStaticAlpha()
        {
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
            for (int y = 0; y < t.terrainData.alphamapHeight; y++)
                for (int x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    maps[x, y, 0] = 1;
                    maps[x, y, 1] = 0.2f;
                }
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void AddAlpha()
        {
            //Debug.Log(t.terrainData.alphamapWidth);
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
            int offset = -(terrainSize / 2);
            float norm = (float) ((heightMax - heightMin));
            float factor = 1f;
            for (int y = 0; y < t.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    float alpha = (float) (t.SampleHeight(new Vector3(y + offset, 0.0f, x + offset))) / norm;
                    alpha = alpha * factor;
                    //maps[x, y, 0] = 1 - alpha;
                    //maps[x, y, 0] = alpha;
                    //maps[x, y, 1] = 1 - alpha;
                    maps[x, y, 0] = 1;
                    maps[x, y, 1] = 0.1f;
                }
            }
            //Debug.Log(maps[t.terrainData.alphamapHeight, t.terrainData.alphamapWidth, 1]);
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void ResetAlpha()
        {
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
            for (int y = 0; y < t.terrainData.alphamapHeight; y++)
                for (int x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    maps[x, y, 0] = 1;
                    maps[x, y, 1] = 0;
                }
            t.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void AddSteepnessAlpha()
        {
            float[,,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 2];

            // For each point on the alphamap...
            for (var y = 0; y < t.terrainData.alphamapHeight; y++)
            {
                for (var x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    // Get the normalized terrain coordinate that
                    // corresponds to the the point.
                    var normX = x * 1.0 / (t.terrainData.alphamapWidth - 1);
                    var normY = y * 1.0 / (t.terrainData.alphamapHeight - 1);

                    // Get the steepness value at the normalized coordinate.
                    var angle = t.terrainData.GetSteepness((float)normX, (float)normY);

                    // Steepness is given as an angle, 0..90 degrees. Divide
                    // by 90 to get an alpha blending value in the range 0..1.
                    var frac = angle / 90.0;
                    map[x, y, 0] = (float) frac;
                    map[x, y, 1] = (float) (1 - frac);
                }
            }

            t.terrainData.SetAlphamaps(0, 0, map);
        }

        public float GetTerrainHeight(float x, float z)
        {

            // objectPos is position of your object
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
