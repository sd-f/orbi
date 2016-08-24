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
        private string mapType = "satellite"; // satellite, roadmap, terrain
        private static int ZOOM = 18;
        private static string PARAMETERS = "zoom=" + ZOOM
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=1024x1024" 
            + "&center=";
        private static string URL_WITH_PARAMETERS = URL + "?" + PARAMETERS;
        public static long SCALE = (long)Math.Pow(2, ZOOM);

        

        public IEnumerator RequestMapData(TerrainService terrainService, GeoPosition geoPosition)
        {
            IndicateRequestStart();
            if (satellite)
                mapType = "satellite";
            else
                mapType = "terrain";

            
            string url = URL_WITH_PARAMETERS
                + WWW.EscapeURL(geoPosition.latitude + "," + geoPosition.longitude)
                + "&maptype=" + mapType;
            //Debug.Log(url);
            WWW request = new WWW(url);
            yield return request;
            if (request.error == null)
            {
                Texture2D tex = new Texture2D(1024, 1024);
                request.LoadImageIntoTexture(tex);
                //tex.LoadImage(request.bytes);
                terrainService.setTexture(tex, 0);
            }
            else
                Error.Show(request.error);
            IndicateRequestFinished();

        }

    }

   
}
