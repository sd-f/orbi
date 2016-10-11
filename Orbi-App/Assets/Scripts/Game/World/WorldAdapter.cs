using ClientModel;
using ServerModel;
using System;
using UnityEngine;

namespace GameController
{
    public static class WorldAdapter
    {
        static Projection projection;
        public static int ZOOM = 18;
        public static bool VERBOSE = false;

        static WorldAdapter()
        {
            projection = new Projection();
        }

        public static Position ToVirtualRelative(GeoPosition original_g)
        {
            GeoPosition pos_g = new GeoPosition();
            // get geo center
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            Position centerVirtual = FromGeoToVirtual(center);

            pos_g.latitude = original_g.latitude;
            pos_g.longitude = original_g.longitude;
            pos_g.altitude = original_g.altitude;


            // to virtual
            Position virtualPosition = FromGeoToVirtual(pos_g);

            if (VERBOSE)
                Game.GetClient().Log("r->v-1" + virtualPosition + "c" +  centerVirtual);

            // translate
            virtualPosition.z = virtualPosition.z - centerVirtual.z;
            virtualPosition.x = virtualPosition.x - centerVirtual.x;
            // y (altitude)
            virtualPosition.y = virtualPosition.y + Game.GetWorld().GetTerrainHeight(virtualPosition.x, virtualPosition.z);
            //virtualPosition.y = virtualPosition.y;

            if (VERBOSE)
                Game.GetClient().Log("r->v-2" + original_g + "(" + pos_g + ")" + "<" + virtualPosition);

            return virtualPosition;
        }

        public static GeoPosition ToRealRelative(Position original_v)
        {
            Position pos_v = new Position();

            // get virtual center
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            Position centerVirtual = FromGeoToVirtual(center);

            //Debug.Log(" -> center_v " + centerVirtual);

            // translate
            pos_v.z =  original_v.z + centerVirtual.z;
            pos_v.x =  original_v.x + centerVirtual.x;
            pos_v.y = original_v.y;

            // to real
            GeoPosition realPosition = FromVirtualToGeo(pos_v);

            if (VERBOSE)
                Game.GetClient().Log("v->r" + original_v + ">" + pos_v + ">" + realPosition);

            return realPosition;
        }



        public static GeoPosition FromVirtualToGeo(Position original)
        {
            // copy
            Position position = new Position();

            //Debug.Log(" -> pos_v " + original);

            // pos_v  -> pos_p  (2)
            position.z = original.z * 2d;
            position.x = original.x * 2d;
            position.y = original.y;

            //Debug.Log(" -> pos_p " + position);

            // pos_p -> pos_pn  (zoom)
            position.z = position.z / Projection.NUM_TILES;
            position.x = position.x / Projection.NUM_TILES;

            //Debug.Log(" -> pos_pn " + position);

            // pos_pn  -> pos_g (proj)
            GeoPosition geoPosition = projection.fromPointToLatLng(position);

            //Debug.Log(" -> pos_g " + geoPosition);

            // altitude (y)
            geoPosition.altitude = geoPosition.altitude - Game.GetWorld().GetTerrainHeight(geoPosition.longitude, geoPosition.latitude);

            return geoPosition;
        }

        public static Position FromGeoToVirtual(GeoPosition original)
        {
            //Debug.Log(" <- pos_g " + original);
            // pos_g  -> pos_pn (proj)
            Position virtualPosition = projection.fromLatLngToPoint(original);

            //Debug.Log(" <- pos_pn " + virtualPosition);

            // pos_pn -> pos_p  (zoom)
            virtualPosition.z = virtualPosition.z * Projection.NUM_TILES;
            virtualPosition.x = virtualPosition.x * Projection.NUM_TILES;

            //Debug.Log(" <- pos_p " + virtualPosition);

            // pos_p  -> pos_v  (2)
            virtualPosition.z = virtualPosition.z / 2d;
            virtualPosition.x = virtualPosition.x / 2d;

            //Debug.Log(" <- pos_v " + virtualPosition);


            return virtualPosition;
        }



    }
}
