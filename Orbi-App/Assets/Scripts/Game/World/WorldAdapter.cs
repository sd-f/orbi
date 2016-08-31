using Assets.Model;
using System;
using UnityEngine;

namespace GameController
{
    class WorldAdapter
    {

        private static int TILE_SIZE = 256;
        private double pixelsPerLonDegree;
        private double pixelsPerLonRadian;
        public static int ZOOM = 18;
        public static long SCALE = (long)Math.Pow(2, ZOOM);
        private double scale = SCALE / 2.5d;

        public WorldAdapter()
        {
            pixelsPerLonDegree = TILE_SIZE / 360.0d;
            pixelsPerLonRadian = TILE_SIZE / (2.0d * Math.PI);
        }

        public void ToVirtual(GeoPosition geoPosition)
        {
            geoPosition.longitude = geoPosition.longitude * scale;
            geoPosition.latitude = geoPosition.latitude * scale;

            double siny = Math.Sin(DegreesToRadians(geoPosition.latitude));
            geoPosition.longitude = (geoPosition.longitude * pixelsPerLonDegree);
            geoPosition.latitude = -((.5 * Math.Log((1 + siny) / (1 - siny)) * -pixelsPerLonRadian));

            Vector3 pos = geoPosition.ToPosition().ToVector3();
            geoPosition.altitude = geoPosition.altitude + Game.GetWorld().GetTerrainHeight(pos.x,pos.z);
        }

        public void ToVirtual(GeoPosition position, GeoPosition center)
        {
            position.latitude = position.latitude - center.latitude;
            position.longitude = position.longitude - center.longitude;
            ToVirtual(position);
        }


        public void ToReal(GeoPosition geoPosition)
        {
            Vector3 pos = geoPosition.ToPosition().ToVector3();
            geoPosition.altitude = geoPosition.altitude - Game.GetWorld().GetTerrainHeight(pos.x, pos.z);

            geoPosition.longitude = (geoPosition.longitude) / pixelsPerLonDegree;
            geoPosition.longitude = geoPosition.longitude / scale;

            double latRadians = (geoPosition.latitude) / -pixelsPerLonRadian;
            geoPosition.latitude = -(RadiansToDegrees(Math.Atan(Math.Sinh(latRadians))));
            geoPosition.latitude = geoPosition.latitude / scale;
        }

        public void ToReal(GeoPosition position, GeoPosition center)
        {
            ToReal(position);
            position.latitude = position.latitude + center.latitude;
            position.longitude = position.longitude + center.longitude;

        }

        public double DegreesToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        public static double RadiansToDegrees(double rads)
        {
            return rads * 180.0 / Math.PI;
        }

        public static double Bound(double value, double min, double max)
        {
            value = Math.Min(value, max);
            return Math.Max(value, min);
        }

        

    }
}
