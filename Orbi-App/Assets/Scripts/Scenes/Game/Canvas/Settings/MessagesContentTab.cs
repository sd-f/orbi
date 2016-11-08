using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/MessagesContentTab")]
    class MessagesContentTab : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject contentContainer;
        public GameObject messageLinePrefab;
        private static Color COLOR_ACTIVE = new Color(0.259f, 0.522f, 0.957f);
        private float offsetY = 0f;
        private List<GameObject> messages = new List<GameObject>();

        public void OnEnable()
        {
            ClearContent();

            foreach (ServerModel.CharacterMessage message in Game.Instance.GetPlayer().GetMessageService().GetMessagesArchive())
                CreateMessageLine(message.message, message.fromCharacter, Color.black);

            foreach (ServerModel.CharacterMessage message in Game.Instance.GetPlayer().GetMessageService().GetMessages())
            {
                CreateMessageLine(message.message, message.fromCharacter, COLOR_ACTIVE);
                Game.Instance.GetPlayer().GetMessageService().GetMessagesArchive().Add(message);
            }

            Game.Instance.GetPlayer().GetMessageService().GetMessages().Clear();

            StartCoroutine(DelayedPositioning());
        }

        private void ClearContent()
        {
            offsetY = 0f;
            messages.Clear();
            GameObjectUtility.DestroyAllChildObjects(contentContainer);
        }

        private void CreateMessageLine(string message, string from, Color titleColor)
        {
            GameObject chatLine = Instantiate(messageLinePrefab) as GameObject;
            chatLine.transform.SetParent(contentContainer.transform, false);
            Text title = chatLine.transform.Find("MessageTitle").GetComponent<Text>();
            title.text = from;
            title.color = titleColor;
            Text icon = chatLine.transform.Find("MessageIcon").GetComponent<Text>();
            icon.color = titleColor;
            chatLine.transform.Find("MessageContent").GetComponent<Text>().text = message;
            messages.Add(chatLine);
        }

        private IEnumerator DelayedPositioning()
        {
            yield return new WaitForEndOfFrame();
            foreach ( GameObject messageLine in messages)
            {
                messageLine.transform.localPosition = new Vector2(0, offsetY);
                offsetY -= (messageLine.transform.Find("MessageContent").GetComponent<RectTransform>().rect.height + 100f); // - header
            }
            contentContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -offsetY);
        }

        
    }
}
