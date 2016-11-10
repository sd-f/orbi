using UnityEngine;
using GameController;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace CanvasUtility
{
    [AddComponentMenu("App/Canvas/Message/Destroyer")]
    class MessagesDestroyer : MonoBehaviour
    {
        private Boolean fading = false;

        void Awake()
        {
            Invoke("StartFading", 2.5f);
        }

        void StartFading()
        {
            fading = true;
        }

        void Update()
        {
            if (fading)
            {
                Color backgroundColor = gameObject.GetComponent<Image>().color;
                backgroundColor.a = backgroundColor.a - 0.08f;
                gameObject.GetComponent<Image>().color = backgroundColor;
                Color color = gameObject.GetComponentInChildren<Text>().color;
                color.a = color.a - 0.08f;
                gameObject.gameObject.GetComponentInChildren<Text>().color = color;
                if (color.a < 0)
                    Invoke("Destroy", 0);
            }
            
        }

        void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}
