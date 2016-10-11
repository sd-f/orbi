using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Inventory: AbstractModel
    {
        public long numberOfObjectTypes = 0;
        public List<InventoryItem> items = new List<InventoryItem>();

    }

    
}
