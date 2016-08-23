using Assets.Control.services;
using Assets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Control
{
    class GoogleMapsService
    {
        private static string URL = "http://maps.googleapis.com/maps/api/staticmap";
        private static string MAP_TYPE = "satellite"; // satellite, roadmap
        private static int SIZE = TerrainService.TERRAIN_SIZE;
        private static int ZOOM = 18;
        private static string PARAMETERS = "zoom=" + ZOOM
            + "&maptype=" + MAP_TYPE
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=1024x1024" 
            + "&center=";
        private static string URL_WITH_PARAMETERS = URL + "?" + PARAMETERS;
        public static long SCALE = (long)Math.Pow(2, ZOOM);

        

        public IEnumerator RequestMapData(Terrain terrain, GeoPosition geoPosition)
        {
            string url = URL_WITH_PARAMETERS  + WWW.EscapeURL(geoPosition.latitude + "," + geoPosition.longitude);
            WWW request = new WWW(url);
            yield return request;
            if (request.error == null)
            {
                TerrainData td = terrain.terrainData;

                Texture2D tex = new Texture2D(SIZE, SIZE);
                //request.LoadImageIntoTexture(tex);
                tex.LoadImage(request.bytes);
 
                /*
                float[,,] alphamaps = new float[td.alphamapWidth, td.alphamapHeight, 1];
                for (int i = 0; i < td.alphamapWidth; i++)
                    for (int j = 0; j < td.alphamapHeight; j++)
                        alphamaps[i, j, 0] = 1.0f; // just randomly blend for now

                td.SetAlphamaps(0, 0, alphamaps);
                */
                
                List<SplatPrototype> sp = new List<SplatPrototype>();
                SplatPrototype sp1 = new SplatPrototype();
                sp.Add(sp1);

                sp1.texture = tex;
                sp1.tileSize = new Vector2(SIZE, SIZE);
                sp1.tileOffset = new Vector2(0, 0);


                td.splatPrototypes = sp.ToArray();

                
                terrain.Flush();
                td.RefreshPrototypes();

            }
            else
                Error.Show(request.error);
            
        }

       

    }

   
}
