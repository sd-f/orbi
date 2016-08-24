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

        private static int TILE_SIZE = 256;
        private double originX, originY;
        private double pixelsPerLonDegree;
        private double pixelsPerLonRadian;
        private double scale = GoogleMapsService.SCALE / 2.5d;
        private TerrainService terrainService;

        public WorldAdapter(TerrainService terrainService)
        {
            this.terrainService = terrainService;
            originX = TILE_SIZE / 2;
            originY = TILE_SIZE / 2;
            pixelsPerLonDegree = TILE_SIZE / 360.0d;
            pixelsPerLonRadian = TILE_SIZE / (2.0d * Math.PI);
        }

        public void ToVirtual(GeoPosition geoPosition)
        {
            geoPosition.longitude = geoPosition.longitude * scale;
            geoPosition.latitude = geoPosition.latitude * scale;
            double siny = Math.Sin(DegreesToRadians(geoPosition.latitude));
            geoPosition.longitude = (geoPosition.longitude * pixelsPerLonDegree);
            geoPosition.latitude = (.5 * Math.Log((1 + siny) / (1 - siny)) * -pixelsPerLonRadian);

            
            geoPosition.altitude = (geoPosition.altitude - terrainService.getMinHeight())/2.0f;
        }

        public void ToVirtual(GeoPosition position, Player player)
        {
            position.latitude = position.latitude - player.geoPosition.latitude;
            position.longitude = position.longitude - player.geoPosition.longitude;
            ToVirtual(position);
        }


        public void ToReal(GeoPosition geoPosition)
        {
            geoPosition.longitude = (geoPosition.longitude) / pixelsPerLonDegree;
            geoPosition.longitude = geoPosition.longitude / scale;
            double latRadians = (geoPosition.latitude) / -pixelsPerLonRadian;
            geoPosition.latitude = (RadiansToDegrees(Math.Atan(Math.Sinh(latRadians))));
            geoPosition.latitude = geoPosition.latitude / scale;
            geoPosition.altitude = (geoPosition.altitude + terrainService.getMinHeight()) *2.0d;
        }

        public void ToReal(GeoPosition position, Player player)
        {
            ToReal(position);
            position.latitude = position.latitude + player.geoPosition.latitude;
            position.longitude = position.longitude + player.geoPosition.longitude;
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
