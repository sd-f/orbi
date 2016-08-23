using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Control.services
{
    class WorldAdapter
    {

        public static int TileSize = 256;
        public static double OriginX, OriginY;
        public static double PixelsPerLonDegree;
        public static double PixelsPerLonRadian;
        private static double CORRECTED_SCALE = GoogleMapsService.SCALE / 2.5d;

        static WorldAdapter()
        {
            OriginX = TileSize / 2;
            OriginY = TileSize / 2;
            PixelsPerLonDegree = TileSize / 360.0d;
            PixelsPerLonRadian = TileSize / (2.0d * Math.PI);
        }

        public static void ToVirtual(GeoPosition geoPosition)
        {
            geoPosition.longitude = geoPosition.longitude * CORRECTED_SCALE;
            geoPosition.latitude = geoPosition.latitude * CORRECTED_SCALE;
            double siny = Math.Sin(DegreesToRadians(geoPosition.latitude));
            geoPosition.longitude = (geoPosition.longitude * PixelsPerLonDegree);
            geoPosition.latitude = (.5 * Math.Log((1 + siny) / (1 - siny)) * -PixelsPerLonRadian);

            
            geoPosition.altitude = (geoPosition.altitude - TerrainService.MIN_HEIGHT)/2.0f;
        }

        public static void ToVirtual(GeoPosition position, Player player)
        {
            position.latitude = position.latitude - player.geoPosition.latitude;
            position.longitude = position.longitude - player.geoPosition.longitude;
            ToVirtual(position);
        }


        public static void ToReal(GeoPosition geoPosition)
        {
            geoPosition.longitude = (geoPosition.longitude) / PixelsPerLonDegree;
            geoPosition.longitude = geoPosition.longitude / CORRECTED_SCALE;
            double latRadians = (geoPosition.latitude) / -PixelsPerLonRadian;
            geoPosition.latitude = (RadiansToDegrees(Math.Atan(Math.Sinh(latRadians))));
            geoPosition.latitude = geoPosition.latitude / CORRECTED_SCALE;
            geoPosition.altitude = (geoPosition.altitude + TerrainService.MIN_HEIGHT)*2.0d;
        }

        public static void ToReal(GeoPosition position, Player player)
        {
            ToReal(position);
            position.latitude = position.latitude + player.geoPosition.latitude;
            position.longitude = position.longitude + player.geoPosition.longitude;
        }

        public static double DegreesToRadians(double deg)
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
