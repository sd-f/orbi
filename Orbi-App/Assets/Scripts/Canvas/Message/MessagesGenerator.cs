using UnityEngine;
using GameController;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace CanvasUtility
{

    [AddComponentMenu("App/Canvas/Message/Generator")]
    class MessagesGenerator : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject messagePrefab;

        void Awake()
        {
            Invoke("CheckForNewMessages", 0.5f);
        }

        void CheckForNewMessages()
        {
            List<GameObject> currentMessages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Message"));
            if (currentMessages.Count == 0)
            {
                if (Game.Instance.GetUi().GetMessageQueue().Count > 0)
                {
                    Message message = Game.Instance.GetUi().GetMessageQueue().Dequeue();
                    GenerateMessageObject(message);
                }
            }
            if (!IsInvoking("CheckForNewMessages"))
                Invoke("CheckForNewMessages", 0.5f);
        }

        void GenerateMessageObject(Message message)
        {
            GameObject parent = GameObject.Find("Canvas");
            GameObject newMessage = UnityEngine.GameObject.Instantiate(messagePrefab, parent.transform, false) as GameObject;
            StartCoroutine(DelayedPositioning(newMessage, message));
        }

        private IEnumerator DelayedPositioning(GameObject newMessage, Message message)
        {
            yield return new WaitForEndOfFrame();
            //newMessage.GetComponent<RectTransform>().sizeDelta = new Vector3(Screen.width, 100);
            Color bgColor = message.GetBackgroundColor();
            bgColor.a = 0.7f;
            newMessage.GetComponent<Image>().color = bgColor;
            newMessage.GetComponentInChildren<Text>().text = message.GetText();
            newMessage.GetComponentInChildren<Text>().color = message.GetColor();
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
    }
}
