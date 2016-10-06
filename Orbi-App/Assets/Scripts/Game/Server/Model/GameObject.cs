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
        public string prefab;
        public ClientModel.Transform transform = new ClientModel.Transform();
        [NonSerialized]
        public Position position;
        [NonSerialized]
        public UnityEngine.GameObject gameObject;

    }
}
