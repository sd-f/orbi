using ServerModel;
using System;

namespace ClientModel
{
    [Serializable]
    public class Transform: AbstractModel
    {
        public Position position = new Position(); // longitude
        public GeoPosition geoPosition = new GeoPosition();
        public Rotation rotation = new Rotation();

        public Transform()
        {
        }

    }
}
