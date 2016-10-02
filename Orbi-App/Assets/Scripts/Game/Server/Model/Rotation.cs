using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Rotation: AbstractModel
    {
        public double x = 0.0d;
        public double y = 0.0d;
        public double z = 0.0d;

        public Rotation(Vector3 rotation)
        {
            this.x = rotation.x;
            this.y = rotation.y;
            this.z = rotation.z;
        }

        public Rotation(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
