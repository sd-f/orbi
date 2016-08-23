using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Model
{
    [Serializable]
    public class Position
    {
        public double x = 0.0d;
        public double y = 0.0d;
        public double z = 0.0d;

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
    }
}
