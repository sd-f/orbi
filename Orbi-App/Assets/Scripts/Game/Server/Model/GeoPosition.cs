using ClientModel;
using GameController;
using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GeoPosition: AbstractModel
    {
        public double latitude = 0.0d;
        public double longitude = 0.0d;
        public double altitude = 0.0d;

        public GeoPosition(double latitude, double longitude, double altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }

        public GeoPosition()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is GeoPosition))
                return false;
            else
            {
                GeoPosition geoObj = obj as GeoPosition;
                return ((geoObj.latitude == this.latitude)
                    && (geoObj.longitude == this.longitude)
                    && (geoObj.altitude == this.altitude));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Position ToPosition()
        {
            return WorldAdapter.ToVirtualRelative(this);
        }

        public override string ToString()
        {
            return "(" + this.latitude + ", " + this.longitude + ", " + this.altitude + ")";
        }


    }
}
