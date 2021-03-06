﻿using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Character: AbstractModel
    {
        public long id;
        public ClientModel.Transform transform = new ClientModel.Transform();
        public string name;
        public long xp = 0;
        public long xr = 0;
        public long level = 0;
        public long nextLevelXp = 0;
        public long lastLevelXp = 0;
        public CharacterDevelopment characterDevelopment;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;

    }

    
}
