using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class MessageOfTheDay: AbstractModel
    {

        public string message;
        public DateTime created;

    }

    
}
