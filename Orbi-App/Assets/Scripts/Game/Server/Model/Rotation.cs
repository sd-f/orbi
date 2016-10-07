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

        public Rotation()
        {
        }

        public Rotation(Quaternion rotation)
        {
            this.x = rotation.eulerAngles.x;
            this.y = rotation.eulerAngles.y;
            this.z = rotation.eulerAngles.z;
        }

        public Rotation(Vector3 rotation)
        {
            this.x = rotation.x;
            this.y = rotation.y;
            this.z = rotation.z;
        }

        public Rotation(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }

        public Quaternion ToQuaternion()
        {
            return Quaternion.Euler(this.ToVector3());
        }
    }
}
