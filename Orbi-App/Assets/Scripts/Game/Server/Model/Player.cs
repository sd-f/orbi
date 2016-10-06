﻿using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class Player: AbstractModel
    {

        public Character character = new Character();
        public GameObject gameObjectToCraft;
        public long selectedObjectId;

    }

    
}
