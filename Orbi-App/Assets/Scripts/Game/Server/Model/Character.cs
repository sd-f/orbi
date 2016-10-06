using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Character: AbstractModel
    {

        public ClientModel.Transform transform = new ClientModel.Transform();
        public string name;
        public long xp;

    }

    
}
