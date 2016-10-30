using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class InventoryItem: AbstractModel
    {

        public GameObjectType type;
        public int amount;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;
        public int categoryId;

    }

    
}
