using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class ServerInfo: AbstractModel
    {

        public string version;
        public List<MessageOfTheDay> messages = new List<MessageOfTheDay>();

    }

    
}
