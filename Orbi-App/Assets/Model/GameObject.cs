using System;
using UnityEngine;

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
        [NonSerialized]
        public UnityEngine.GameObject gameObject = null;
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}
