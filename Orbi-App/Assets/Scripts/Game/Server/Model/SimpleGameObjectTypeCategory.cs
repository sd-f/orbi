using System;
using System.Collections.Generic;

namespace ServerModel
{
    [Serializable]
    public class SimpleGameObjectTypeCategory : AbstractModel
    {
        public long id;
        public string name;
        public bool craftable;
        public int rarity;

    }

    
}
