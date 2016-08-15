using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
    [Serializable]
    public class World
    {

        public List<VirtualGameObject> gameObjects;
    }

}
