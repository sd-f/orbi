using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Model
{
    [Serializable]
    public class GeoPosition
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

        public Position ToPosition()
        {
            return new Position(this.longitude, this.altitude, this.latitude);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
