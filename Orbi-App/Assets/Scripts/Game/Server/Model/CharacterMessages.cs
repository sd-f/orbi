using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class CharacterMessages: AbstractModel
    {
        public List<CharacterMessage> messages = new List<CharacterMessage>();
    }

    
}
