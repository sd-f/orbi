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
    class MapTextureService: AbstractService
    {
        public static bool satellite = false;

        private static string URL = "http://maps.googleapis.com/maps/api/staticmap";
        private string mapType = "satellite"; // satellite, roadmap, terrain
        private static string PARAMETERS = "zoom=" + WorldAdapter.ZOOM
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=512x512"
            + "&center=";
        private static string URL_WITH_PARAMETERS = URL + "?" + PARAMETERS;
        private SortedList<int, SplatPrototype> splats;

        public IEnumerator RequestMapData(GeoPosition geoPosition, SplatPrototype prototype)
        {
            IndicateRequestStart();
            if (Game.GetGame().GetSettings().IsSatelliteOverlayEnabled())
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
            Projection projection = new Projection();
            splats = new SortedList<int, SplatPrototype>();
            int offset_x;
            int offset_z;
            Position position;
            int splatIndex = 1;
            String gout = "";
            for (int x = 0; x <= 1; x++) // W, E -> x
            {
                for (int z = 0; z <= 1; z++) // S, N -> z 
                {
                    offset_x = x * 256;
                    offset_z = z * 256;

                    // 1 --> x: -128, z: -128 SW
                    // 3 --> x:  128, z: -128 SE
                    // 2 --> x: -128, z:  128 NW
                    // 4 --> x:  128, z:  128 NE

                    SplatPrototype prototype = new SplatPrototype();
                    prototype.tileOffset = new Vector2(0, 0);
                    prototype.tileSize = new Vector2(256, 256);

                    position = new Position(offset_z - 128f, 0f, offset_x - 128f);
                    GeoPosition pos_g = position.ToGeoPosition();
                    //Debug.Log(projection.WorldToTilePos(pos_g.longitude, pos_g.latitude)); // for open street maps
                    //Debug.Log("loading: " + offset_x + "," + offset_z + "@" + position + " - " + pos_g);
                    //gout = gout + "new google.maps.LatLng("+ pos_g.latitude+ ","+ pos_g.longitude+ "),\n";
                    //UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = position.ToVector3();
                    //cube.transform.name = "c" + splatIndex;
                    //cube.transform.SetParent(Game.GetGame().transform);

                    yield return RequestMapData(position.ToGeoPosition(), prototype);
                    splats.Add(splatIndex, prototype);
                    splatIndex++;
                }
            }
            //Debug.Log(gout);
            Game.GetWorld().GetTerrainService().SetMapsSplats(splats);


        }

    }


}
