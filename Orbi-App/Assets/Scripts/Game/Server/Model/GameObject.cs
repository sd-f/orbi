using ClientModel;
using System;
using UnityEngine;

namespace ServerModel
{
    [Serializable]
    public class GameObject: AbstractModel
    {
        public long id;
        public string name;
        public GameObjectType type;
        public string userText;
        public int constraints;
        public ClientModel.Transform transform = new ClientModel.Transform();
        public AiProperties aiProperties;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;
        
    }
}
