using System;

namespace ServerModel
{
    [Serializable]
    public class Rotation: AbstractModel
    {
        public double x = 0.0d;
        public double y = 0.0d;
        public double z = 0.0d;

        public Rotation(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
