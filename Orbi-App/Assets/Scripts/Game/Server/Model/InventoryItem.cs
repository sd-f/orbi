using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class InventoryItem: AbstractModel
    {

        public String prefab;
        public int amount;
        public bool supportsUserText;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;

    }

    
}
