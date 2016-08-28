using ClientModel;
using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GameObject: AbstractModel
    {
        public long id;
        public string name;
        public string prefab;
        public GeoPosition geoPosition;
        public Rotation rotation;
        [NonSerialized]
        public Position position;
        
    }
}
