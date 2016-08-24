using Assets.Control.services;
using Assets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Control
{
    class GoogleMapsService: AbstractService
    {
        public static bool satellite = false;

        private static string URL = "http://maps.googleapis.com/maps/api/staticmap";
        private string mapType = "satellite"; // satellite, roadmap
        private static int SIZE = TerrainService.TERRAIN_SIZE;
        private static int ZOOM = 18;
        private static string PARAMETERS = "zoom=" + ZOOM
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=1024x1024" 
            + "&center=";
        private static string URL_WITH_PARAMETERS = URL + "?" + PARAMETERS;
        public static long SCALE = (long)Math.Pow(2, ZOOM);

        

        public IEnumerator RequestMapData(Terrain terrain, GeoPosition geoPosition)
        {
            IndicateRequestStart();
            if (satellite)
                mapType = "satellite";
            else
                mapType = "roadmap";

            
            string url = URL_WITH_PARAMETERS
                + WWW.EscapeURL(geoPosition.latitude + "," + geoPosition.longitude)
                + "&maptype=" + mapType;
            //Debug.Log(url);
            WWW request = new WWW(url);
            yield return request;
            if (request.error == null)
            {
                TerrainData td = terrain.terrainData;

                Texture2D tex = new Texture2D(1024, 1024);
                //request.LoadImageIntoTexture(tex);
                tex.LoadImage(request.bytes);

                SplatPrototype[] splats = td.splatPrototypes;
                //plats[0] = new SplatPrototype();
                //plats[0].tileOffset = new Vector2(0, 0);
                //plats[0].tileSize = new Vector2(1024, 1024);
                splats[0].texture = tex;
                td.splatPrototypes = splats;
                /*
                float[,,] maps = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
                for (int y = 0; y < td.alphamapHeight; y++)
                {
                    for (int x = 0; x < td.alphamapWidth; x++)
                    {
                        //float alpha = (float)(t.terrainData.GetHeight(x + offset, y + offset)) / norm;
                        maps[x, y, 0] = tex.GetPixel(x, y).a / 255;
                    }
                }
                //Debug.Log(maps[t.terrainData.alphamapHeight, t.terrainData.alphamapWidth, 1]);
                td.SetAlphamaps(0, 0, maps);
                */
                td.RefreshPrototypes();
                terrain.Flush();
                
                IndicateRequestFinished();
            }
            else
            {
                IndicateRequestFinished();
                Error.Show(request.error);
            }

        }

    }

   
}
