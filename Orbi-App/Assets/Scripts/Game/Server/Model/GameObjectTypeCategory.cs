using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GameObjectTypeCategory : SimpleGameObjectTypeCategory
    {
        public List<GameObjectType> types = new List<GameObjectType>();

    }

    
}
