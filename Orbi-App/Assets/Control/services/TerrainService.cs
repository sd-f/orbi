using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Control.services
{
    class TerrainService
    {
        private int heightMapSizeForRequest = 33;
        public static int TERRAIN_SIZE = 256;
        public static double MAX_HEIGHT = 0d;
        public static double MIN_HEIGHT = 100000d;
        public static int TERRAIN_HEIGHT = 300;

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
            Debug.Log("min " + MIN_HEIGHT + " max " + MAX_HEIGHT);
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
            double scale = (MAX_HEIGHT - MIN_HEIGHT) / TERRAIN_HEIGHT;
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

                texture.SetPixel(vX, vY, new Color((float)(dummyGameObject.geoPosition.altitude / TERRAIN_HEIGHT), 0.0f, 0.0f));

            }
            texture.Apply();
            UnityEngine.GameObject.Find("Plane").GetComponent<Renderer>().material.mainTexture = texture;
            //Texture2D newTexuture = ScaleTexture(texture, hmWidth, hmHeight);
            //Debug.Log("w" + hmWidth + "h" + hmHeight);
            // TODO smooth height
            for (x = 0; x < hmWidth; x++)
            {
                for (y = 0; y < hmHeight; y++)
                {
                    heights[x, y] = texture.GetPixel(x, y).r;
                }
            }
            terrain.terrainData.SetHeights(0, 0, heights);
            terrain.Flush();

        }



        private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                    result.SetPixel(j, i, newColor);
                }
            }
            result.Apply();
            return result;
        }

    }



}
