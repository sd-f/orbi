using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class CharacterMessage: AbstractModel
    {
        public String message;
        public String fromCharacter;
        public long fromCharacterId;
        public String toCharacter;
        public long toCharacterId;
        public string date;

    }

    
}
