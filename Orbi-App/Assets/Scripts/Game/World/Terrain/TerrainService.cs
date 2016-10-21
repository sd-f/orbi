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
