using UnityEngine;

namespace ServerModel
{
    public abstract class AbstractModel
    {

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}
