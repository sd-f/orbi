using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class World: AbstractModel
    {

        public List<GameObject> gameObjects = new List<GameObject>();
        public List<Character> characters = new List<Character>();

    }

    
}
