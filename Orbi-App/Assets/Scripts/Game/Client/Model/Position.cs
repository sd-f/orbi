using GameController;
using ServerModel;
using System;
using UnityEngine;

namespace ClientModel
{
    [Serializable]
    public class Position: AbstractModel
    {
        public double x = 0.0d;
        public double y = 0.0d;
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

        public Vector3 ToVector3()
        {
            return new Vector3((float)this.x,(float)this.y,(float)this.z);
        }

        public GeoPosition ToGeoPosition()
        {
            return WorldAdapter.ToRealRelative(this);
        }

        public override string ToString()
        {
            return "(" + this.z + "," + this.x + "," + this.y + ")";
        }
    }
}
