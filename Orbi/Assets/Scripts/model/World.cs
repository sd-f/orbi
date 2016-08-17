using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.model
{
    [Serializable]
    public class Position
    {
        public Double x;
        public Double y;
        public Double z;
    }
    [Serializable]
    public class VirtualGameObject
    {

        public Double id;
        public Position position;
        public String name;

        public Vector3 ToVector3()
        {
            return new Vector3((float)position.x, (float)position.y, (float)position.z);
        }
    }
    [Serializable]
    public class World
    {

        public List<VirtualGameObject> gameObjects;
    }

}
