using GameController;
using ServerModel;
using System;
using UnityEngine;

namespace ClientModel
{
    [Serializable]
    public class Position: AbstractModel
    {
        public double x = 0.0d; // longitude
        public double y = 0.0d; // latitude
        public double z = 0.0d;

        public Position()
        {

        }

        public Position(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public Position(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Position(Mapity.Position world)
        {
            this.x = world.Longitude;
            this.z = world.Latitude;
            this.y = world.Elevation;
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)this.x,(float)this.y,(float)this.z);
        }

        public Vector3 ToVector2()
        {
            return new Vector3((float)this.z, (float)this.x);
        }

        public GeoPosition ToGeoPosition()
        {
            return WorldAdapter.ToRealRelative(this);
        }

        public override string ToString()
        {
            return "(" + this.z + ", " + this.x + ", " + this.y + ")";
        }
    }
}
