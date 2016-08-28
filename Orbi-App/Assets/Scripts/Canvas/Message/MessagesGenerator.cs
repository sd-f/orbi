using UnityEngine;
using GameController;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CanvasUtility
{
    [AddComponentMenu("App/Canvas/Message/Generator")]
    class MessagesGenerator : MonoBehaviour
    {
        void Awake()
        {
            InvokeRepeating("CheckForNewMessages", 0, 0.5f);
        }

        void CheckForNewMessages()
        {
            GameObject parent = GameObject.Find("Canvas");
            List<GameObject> currentMessages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Message"));
            if (currentMessages.Count == 0)
            {
                if (Game.GetGame().GetUi().GetMessageQueue().Count > 0)
                {
                    Message message = Game.GetGame().GetUi().GetMessageQueue().Dequeue();
                    GenerateMessageObject(parent, message);
                }
               
            }
        }

        void GenerateMessageObject(GameObject parent, Message message)
        {
            GameObject newMessage = UnityEngine.GameObject.Instantiate(Resources.Load<UnityEngine.GameObject>("Prefabs/Message"), parent.transform, false) as GameObject;

            //text.rectTransform.sizeDelta = new Vector2(Screen.width - 50, text.rectTransform.rect.height);
            
            //newMessage.GetComponent<RectTransform>().localPosition = new Vector3(0,-50,0);
            newMessage.GetComponent<Image>().color = message.GetBackgroundColor();
            newMessage.GetComponentInChildren<Text>().text = message.GetText();
            newMessage.GetComponentInChildren<Text>().color = message.GetColor();
            //newMessage.transform.SetParent(parent.transform);

        }

        void OnDestroy()
        {
            CancelInvoke();
        }
    }
}
