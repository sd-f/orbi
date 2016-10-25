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
    class MapTextureService: AbstractHttpService
    {
        public static bool satellite = false;

        
        private string mapType = "satellite"; // satellite, roadmap, terrain

        private static string GOOGLE_URL = "http://maps.googleapis.com/maps/api/staticmap";
        private static string GOOGLE_PARAMS = "zoom=" + WorldAdapter.ZOOM
            + "&scale=2"
            + "&format=PNG"
            //+ "&sensor=false"
            + "&size=512x512"
            + "&center=";
        private static string GOOGLE_URL_WITH_PARAMETERS = GOOGLE_URL + "?" + GOOGLE_PARAMS;

        private static string OSM_URL = "http://a.tile.openstreetmap.org/19/";
        private SortedList<int, SplatPrototype> splats;

        public IEnumerator RequestMapDataGoogle(GeoPosition geoPosition, SplatPrototype prototype)
        {
            IndicateRequestStart();
            if (Game.GetGame().GetSettings().IsSatelliteOverlayEnabled())
                mapType = "satellite";
            else
                mapType = "roadmap";


            string url = GOOGLE_URL_WITH_PARAMETERS
                + WWW.EscapeURL(geoPosition.latitude + "," + geoPosition.longitude)
                + "&maptype=" + mapType;
            //Debug.Log(url);
            Game.GetClient().Log("google map request = " + url);
            WWW request = new WWW(url);
            yield return request;
            IndicateRequestFinished();
            if (request.error == null)
            {
                Texture2D tex = new Texture2D(1024, 1024);
                //tex.wrapMode = TextureWrapMode.Repeat;
                request.LoadImageIntoTexture(tex);
                //tex.LoadImage(request.bytes);
                prototype.texture = tex;
            }
            else
                HandleError(request);
        }

        public IEnumerator RequestMapDataOsm(long latitude, long longitude, SplatPrototype prototype)
        {
            IndicateRequestStart();

            string url = OSM_URL
                + latitude + "/" + longitude + ".png";
            //Debug.Log(url);
            Game.GetClient().Log("osm map request = " + url);
            WWW request = new WWW(url);
            yield return request;
            IndicateRequestFinished();
            if (request.error == null)
            {
                Texture2D tex = new Texture2D(256, 256); // TODO change if sd-f osm server is running
                //tex.wrapMode = TextureWrapMode.Repeat;
                request.LoadImageIntoTexture(tex);
                //tex.LoadImage(request.bytes);
                prototype.texture = tex;
            }
            else
                HandleError(request);
            
        }

        public IEnumerator LoadTexturesTiled()
        {
            //Projection projection = new Projection();
            splats = new SortedList<int, SplatPrototype>();
            int offset_x;
            int offset_z;
            Position position;
            int splatIndex = 1;
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
                    //GeoPosition pos_g = position.ToGeoPosition();
                    //Debug.Log("back: " + pos_g.ToPosition());
                    //Debug.Log(projection.WorldToTilePos(pos_g.longitude, pos_g.latitude)); // for open street maps
                    //Debug.Log("loading: " + offset_x + "," + offset_z + "@" + position + " - " + pos_g);
                    //gout = gout + "new google.maps.LatLng("+ pos_g.latitude+ ","+ pos_g.longitude+ "),\n";
                    //UnityEngine.GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = position.ToVector3();
                    //cube.transform.name = "c" + splatIndex;
                    //cube.transform.SetParent(Game.GetGame().transform);

                    yield return RequestMapDataGoogle(position.ToGeoPosition(), prototype);
                    splats.Add(splatIndex, prototype);
                    splatIndex++;
                }
            }
            //Debug.Log(gout);
            //Game.GetWorld().GetTerrainService().SetMapsSplats(splats);
        }

        public IEnumerator LoadTexture()
        {
            Position position;
            SplatPrototype prototype = new SplatPrototype();
            prototype.tileOffset = new Vector2(0, 0);
            prototype.tileSize = new Vector2(1024, 1024);
            position = new Position(0d,0d,0d);
            GeoPosition geoPosition = position.ToGeoPosition();
            PointF tilePosition = WorldAdapter.PROJECTION.WorldToTilePos(geoPosition.longitude, geoPosition.latitude, 19);
            yield return RequestMapDataOsm((long)tilePosition.x, (long)tilePosition.y, prototype);
            //Debug.Log(gout);
            //Game.GetWorld().GetTerrainService().SetGroundTexture(prototype.texture);


        }

    }


}
