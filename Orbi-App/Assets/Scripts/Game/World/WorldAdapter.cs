using ClientModel;
using ServerModel;
using System;
using UnityEngine;

namespace GameController
{
    public static class WorldAdapter
    {

        private static int TILE_SIZE = 256;
        private static double PIXEL_PER_LON_DEGREE;
        private static double PIXEL_PER_LON_RADIAN;
        public static int ZOOM = 18;
        public static long MAPS_SCALE = (long)Math.Pow(2, ZOOM);
        private static double SCALE = MAPS_SCALE / 2.5d;
        private static double SCALE_Y = MAPS_SCALE / 3.6d;

        static WorldAdapter()
        {
            PIXEL_PER_LON_DEGREE = TILE_SIZE / 360.0d;
            PIXEL_PER_LON_RADIAN = TILE_SIZE / (2.0d * Math.PI);
        }

        public static void ToVirtual(Position virtualPosition)
        {
            virtualPosition.x = virtualPosition.x * SCALE;
            virtualPosition.z = virtualPosition.z * SCALE_Y;

            double siny = Math.Sin(DegreesToRadians(virtualPosition.z));
            virtualPosition.x = (virtualPosition.x * PIXEL_PER_LON_DEGREE);


            virtualPosition.z = -((.5 * Math.Log((1 + siny) / (1 - siny)) * -PIXEL_PER_LON_RADIAN));

            virtualPosition.y = virtualPosition.y + Game.GetWorld().GetTerrainHeight(virtualPosition.x, virtualPosition.z);
        }

        public static Position ToVirtualRelative(GeoPosition realPosition)
        {
            Position virtualPosition = new Position(realPosition.longitude, realPosition.altitude, realPosition.latitude);

            // translate
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            virtualPosition.z = virtualPosition.z - center.latitude;
            virtualPosition.x = virtualPosition.x - center.longitude;

            // scale
            ToVirtual(virtualPosition);
            return virtualPosition;
        }


        public static void ToReal(GeoPosition virtualPosition)
        {
            virtualPosition.altitude = virtualPosition.altitude - Game.GetWorld().GetTerrainHeight(virtualPosition.longitude, virtualPosition.latitude);

            virtualPosition.longitude = (virtualPosition.longitude) / PIXEL_PER_LON_DEGREE;

            double latRadians = (virtualPosition.latitude) / -PIXEL_PER_LON_RADIAN;
            virtualPosition.latitude = -(RadiansToDegrees(Math.Atan(Math.Sinh(latRadians))));

            virtualPosition.longitude = virtualPosition.longitude / SCALE;
            virtualPosition.latitude = virtualPosition.latitude / SCALE_Y;
        }

        public static GeoPosition ToRealRelative(Position virtualPosition)
        {
            GeoPosition realPosition = new GeoPosition(virtualPosition.z, virtualPosition.x, virtualPosition.y);

            // scale
            ToReal(realPosition);

            // translate
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            realPosition.latitude = realPosition.latitude + center.latitude;
            realPosition.longitude = realPosition.longitude + center.longitude;
            return realPosition;

        }

        private static double DegreesToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double rads)
        {
            return rads * 180.0 / Math.PI;
        }

        private static double Bound(double value, double min, double max)
        {
            value = Math.Min(value, max);
            return Math.Max(value, min);
        }

        

    }
}
