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
    class GoogleMapsService: AbstractService
    {
        public static bool satellite = false;

        private static string URL = "http://maps.googleapis.com/maps/api/staticmap";
        private string mapType = "satellite"; // satellite, roadmap, terrain
        private static string PARAMETERS = "zoom=" + WorldAdapter.ZOOM
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=1024x1024"
            + "&center=";
        private static string URL_WITH_PARAMETERS = URL + "?" + PARAMETERS;
        private SortedList<int, SplatPrototype> splats;

        public IEnumerator RequestMapData(GeoPosition geoPosition, SplatPrototype prototype)
        {
            IndicateRequestStart();
            if (Game.GetGame().GetSettings().IsSatelliteOverlayEnabled())
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
                //tex.wrapMode = TextureWrapMode.Repeat;
                request.LoadImageIntoTexture(tex);
                //tex.LoadImage(request.bytes);
                prototype.texture = tex;
            }
            else
                Error.Show(request.error);
            IndicateRequestFinished();

        }

        public IEnumerator LoadTextures()
        {
            splats = new SortedList<int, SplatPrototype>();
            int offset_x;
            int offset_y;
            Position position;
            int splatIndex = 1;
            for (int x = 0; x <= 1; x++)
            {
                for (int y = 0; y <= 1; y++)
                {
                    offset_x = x * 256;
                    offset_y = y * 256;
                    
                    // 1 --> x: -128, z: -128
                    // 2 --> x: -128, z: 128
                    // 3 --> x: 128, z: -128
                    // 4 --> x: 128, z: 128

                    SplatPrototype prototype = new SplatPrototype();
                    prototype.tileOffset = new Vector2(offset_x, offset_y);
                    prototype.tileSize = new Vector2(256, 256);
                    
                    position = new Position(offset_x - 128f, 0f, offset_y - 128f);
                    Debug.Log("loading: " + offset_x + "," + offset_y + "@" + position + " - " + position.ToGeoPosition());
                    yield return RequestMapData(position.ToGeoPosition(), prototype);
                    splats.Add(splatIndex, prototype);
                    splatIndex++;
                }
            }
            Game.GetWorld().GetTerrainService().SetMapsSplats(splats);


        }

    }

   
}
