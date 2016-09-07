using ClientModel;
using ServerModel;
using System;
using UnityEngine;

namespace GameController
{
    public static class WorldAdapter
    {
        static GoogleMapsApiProjection projection;
        public static int ZOOM = 18;

        static WorldAdapter()
        {
            projection = new GoogleMapsApiProjection();
        }

        public static Position ToVirtual(GeoPosition realPosition)
        {
            Position virtualPosition = new Position(realPosition.longitude, realPosition.altitude, realPosition.latitude);

            PointF point = projection.fromLatLngToPoint(virtualPosition.z, virtualPosition.x);

            virtualPosition.y = virtualPosition.y + Game.GetWorld().GetTerrainHeight(virtualPosition.x, virtualPosition.z);
            return virtualPosition;
        }

        public static Position ToVirtualRelative(GeoPosition realPosition)
        {
            GeoPosition relativeRealPosition = new GeoPosition();

            // translate
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            relativeRealPosition.latitude = realPosition.latitude - center.latitude;
            relativeRealPosition.longitude = realPosition.longitude - center.longitude;
            
            // scale
            Position virtualPosition = ToVirtual(relativeRealPosition);
            return virtualPosition;
        }


        public static GeoPosition ToReal(Position virtualPosition)
        {
            // scale to map pixel -> move to positive
            Position pixelPosition = new Position();
            pixelPosition.z = virtualPosition.z;
            pixelPosition.x = virtualPosition.x;
            pixelPosition.y = virtualPosition.y;

            GeoPosition realPosition = new GeoPosition(pixelPosition.z, pixelPosition.x, pixelPosition.y);
            realPosition.altitude = realPosition.altitude - Game.GetWorld().GetTerrainHeight(realPosition.longitude, realPosition.latitude);

            //PointF point = projection.toReal(new PointF((float)realPosition.longitude, (float)realPosition.latitude));
            PointF point = projection.fromPointToLatLng(new PointF((float)realPosition.longitude, (float)realPosition.latitude));
            //PointF point = projection.TileToWorldPos(realPosition.latitude, realPosition.longitude);
            realPosition.longitude = point.x;
            realPosition.latitude = point.y;

            Debug.Log(virtualPosition + " -> " + pixelPosition + " -> " + realPosition);
            return realPosition;
        }

        public static GeoPosition ToRealRelative(Position virtualPosition)
        {
            // scale
            GeoPosition realPosition = ToReal(virtualPosition);

            // translate
            GeoPosition center = Game.GetWorld().GetCenterGeoPostion();
            realPosition.latitude = center.latitude - realPosition.latitude;
            realPosition.longitude = center.longitude - realPosition.longitude;

            return realPosition;

        }

        

    }
}
