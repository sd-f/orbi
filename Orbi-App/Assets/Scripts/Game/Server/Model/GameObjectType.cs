using ClientModel;
using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GameObjectType: AbstractModel
    {
        public long id;
        public string prefab;
        public bool supportsUserText;
        public int rarity;
        public bool ai;
        public SimpleGameObjectTypeCategory category;


    }
}
