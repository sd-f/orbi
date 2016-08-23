using System;

namespace Assets.Model
{
    [Serializable]
    public class GameObject
    {
        public long id;
        public string name;
        public string prefab;
        public GeoPosition geoPosition;
        public Position position;
        public Rotation rotation;
        
    }
}
