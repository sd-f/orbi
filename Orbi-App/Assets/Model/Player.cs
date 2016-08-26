using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Model
{
    [Serializable]
    public class Player
    {

        public GeoPosition geoPosition;
        public GameObject gameObjectToCraft;
        public long selectedObjectId;

    }
}
