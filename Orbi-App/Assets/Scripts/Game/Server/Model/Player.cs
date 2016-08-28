using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Player: AbstractModel
    {

        public GeoPosition geoPosition;
        public GameObject gameObjectToCraft;
        public long selectedObjectId;

    }

    
}
