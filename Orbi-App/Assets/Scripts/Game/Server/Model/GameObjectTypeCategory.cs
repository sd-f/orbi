using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GameObjectTypeCategory : AbstractModel
    {
        public long id;
        public string name;
        public bool craftable;
        public long numberOfItems;
        public int rarity;

    }

    
}
