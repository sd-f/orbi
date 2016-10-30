using ClientModel;
using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class AiProperties: AbstractModel
    {
        public ClientModel.Transform target;
        public DateTime lastTargetUpdate;


    }
}
