using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Inventory: AbstractModel
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public List<GameObjectTypeCategory> categories = new List<GameObjectTypeCategory>();

    }

    
}
