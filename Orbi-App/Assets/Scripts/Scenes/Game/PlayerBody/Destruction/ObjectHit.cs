using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/ObjectHit")]
    class ObjectHit : MonoBehaviour
    {

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Bullet")
            {
                Debug.Log("collision " + col.gameObject.name);
                GameObject gameObject = FindObject(col.gameObject);
                if (gameObject != null)
                {
                    Debug.Log("Yeah " + gameObject.name);
                }
            }
            
            
        }

        private GameObject FindObject(GameObject gameObject)
        {
            if (gameObject.tag == "DynamicGameObject")
            {
                return gameObject;
            } else
            {
                if (gameObject.transform.parent != null)
                {
                    return FindObject(gameObject.transform.parent.gameObject);
                }
            }
            return null;
        }
    }
}
