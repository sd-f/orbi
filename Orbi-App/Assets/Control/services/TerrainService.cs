using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Control.services
{
    class TerrainService
    {
        private int heightMapSizeForRequest = 33;
        public static int TERRAIN_SIZE = 256;
        public static double MAX_HEIGHT = 0d;
        public static double MIN_HEIGHT = 100000d;
        public static int TERRAIN_HEIGHT = 100;

        public World GenerateDummyWorldArround(Player player)
        {
            World dummyWorld = new World();

            int factor = TERRAIN_SIZE / (heightMapSizeForRequest - 1);
            // 0 .. 17 -> -8 .. +8
            for (int i = 0; i < heightMapSizeForRequest; i++)
                for (int j = 0; j < heightMapSizeForRequest; j++)
                {
                    Model.GameObject dummyGameObject = new Model.GameObject();
                    GeoPosition dummyGeoLocation = new GeoPosition();
                    dummyGeoLocation.latitude = (i - (heightMapSizeForRequest / 2)) * factor;
                    dummyGeoLocation.longitude = (j - (heightMapSizeForRequest / 2)) * factor;
                    WorldAdapter.ToReal(dummyGeoLocation, player);
                    dummyGeoLocation.altitude = 0.0d;
                    dummyGameObject.geoPosition = dummyGeoLocation;
                    dummyWorld.gameObjects.Add(dummyGameObject);
                }

            return dummyWorld;
        }



        public void AdjustTerrainHeights(Terrain terrain, World dummyWorld, Player player)
        {
            foreach (Model.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                if (dummyGameObject.geoPosition.altitude < MIN_HEIGHT)
                    MIN_HEIGHT = dummyGameObject.geoPosition.altitude;
                if (dummyGameObject.geoPosition.altitude > MAX_HEIGHT)
                    MAX_HEIGHT = dummyGameObject.geoPosition.altitude;
            }
            //Debug.Log("min " + MIN_HEIGHT + " max " + MAX_HEIGHT);
            int hmWidth = terrain.terrainData.heightmapWidth;
            int hmHeight = terrain.terrainData.heightmapHeight;
            float[,] heights = terrain.terrainData.GetHeights(0, 0, hmWidth, hmHeight);
            for (int i = 0; i < hmWidth; i++)
                for (int j = 0; j < hmHeight; j++)
                {
                    heights[i, j] = (float)MIN_HEIGHT;
                }
            Texture2D texture = new Texture2D(heightMapSizeForRequest, heightMapSizeForRequest);
            int x, y, vX, vY;
            int factor = TERRAIN_SIZE / (heightMapSizeForRequest - 1);
            float height = 0.0f;
            float[,] source = new float[heightMapSizeForRequest, heightMapSizeForRequest];
            foreach (Model.GameObject dummyGameObject in dummyWorld.gameObjects)
            {
                WorldAdapter.ToVirtual(dummyGameObject.geoPosition, player);

                //UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = dummyGameObject.geoPosition.ToPosition().ToVector3();
                //cube.transform.localScale = new Vector3(2, 2, 2);
                x = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().z);
                y = (int)Math.Round(dummyGameObject.geoPosition.ToPosition().x);
                vX = (x / factor) + ((heightMapSizeForRequest - 1) / 2);
                vY = (y / factor) + ((heightMapSizeForRequest - 1) / 2);
                height = (float)(dummyGameObject.geoPosition.altitude / TERRAIN_HEIGHT);
                texture.SetPixel(vX, vY, new Color(height, 0.0f, 0.0f));
                source[vX, vY] = height;
            }
            texture.Apply();
            UnityEngine.GameObject.Find("Plane").GetComponent<Renderer>().material.mainTexture = texture;
            // 33x33
            // -> 257

            float[,] target = new float[hmWidth, hmHeight];
           // target = scale(source, heightMapSizeForRequest, heightMapSizeForRequest, hmWidth, hmHeight);

            //texture.Resize(hmWidth, hmHeight);
            //texture.Apply();
            //Texture2D heightTexture = ScaleTexture(texture, hmWidth, hmHeight);
            //Debug.Log("w" + hmWidth + "h" + hmHeight);
            // TODO smooth height
            for (x = 0; x < hmWidth; x++)
            {
                for (y = 0; y < hmHeight; y++)
                {
                    heights[x, y] = texture.GetPixel(x, y).r;
                }
            }
            terrain.terrainData.SetHeightsDelayLOD(0, 0, heights);
            terrain.terrainData.SetHeights(0, 0, heights);
            AddAlpha(terrain);
            terrain.terrainData.RefreshPrototypes();
            terrain.Flush();

            

        }

        void AddAlpha(Terrain t)
        {
            //Debug.Log(t.terrainData.alphamapWidth);
            float[,,] maps = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
            int offset = -(TERRAIN_SIZE / 2);
            float norm = (float) ((MAX_HEIGHT - MIN_HEIGHT));
            float factor = 1f;
            for (int y = 0; y < t.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    float alpha = (float) (t.SampleHeight(new Vector3(y + offset, 0.0f, x + offset))) / norm;
                    alpha = alpha * factor;
                    //maps[x, y, 0] = 1 - alpha;
                    maps[x, y, 0] = alpha;
                    maps[x, y, 1] = 1 - alpha;
                }
            }
            //Debug.Log(maps[t.terrainData.alphamapHeight, t.terrainData.alphamapWidth, 1]);
            t.terrainData.SetAlphamaps(0, 0, maps);
        }


    }



}
