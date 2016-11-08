using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/ObjectProperties")]
    class ObjectProperties : MonoBehaviour
    {

        private ServerModel.GameObject obj;

        public void SetObject(ServerModel.GameObject obj)
        {
            this.obj = obj;
        }
        
        public ServerModel.GameObject GetObject()
        {
            return this.obj;
        }

        public void OnTouched()
        {
            if (this.gameObject != null)
                GameObject.Find("Canvas").GetComponent<MainCanvas>().OpenObjectInfos(obj);
        }

    }

    
}