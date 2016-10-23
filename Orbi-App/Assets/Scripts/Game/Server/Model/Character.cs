using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Character: AbstractModel
    {
        public long id;
        public ClientModel.Transform transform = new ClientModel.Transform();
        public string name;
        public long xp;
        public long xr;
        public long level;
        public CharacterDevelopment characterDevelopment;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;

    }

    
}
