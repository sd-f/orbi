using CanvasUtility;
using GameScene;
using ServerModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Services
{

    public class MessageService: AbstractHttpService
    {

        private List<CharacterMessage> messages = new List<CharacterMessage>();
        private List<CharacterMessage> messagesArchive = new List<CharacterMessage>();

        public List<CharacterMessage> GetMessages()
        {
            return messages;
        }

        public List<CharacterMessage> GetMessagesArchive()
        {
            return messagesArchive;
        }

        public IEnumerator RequestMessage(string messageText, long toCharacterId)
        {
            ServerModel.CharacterMessage newMessage = new ServerModel.CharacterMessage();
            newMessage.message = messageText;
            newMessage.toCharacterId = toCharacterId;
            yield return Request("player/message", JsonUtility.ToJson(newMessage), OnMessageSent);
        }

        private void OnMessageSent(string data)
        {
            Info.Show("Message sent.");
        }

        public IEnumerator RequestMessages()
        {
            yield return Request("player/messages", null, OnMessagesRecieved);
        }

        private void OnMessagesRecieved(string data)
        {
            CharacterMessages messagesObject = JsonUtility.FromJson<CharacterMessages>(data);
            if ((messagesObject.messages != null) && (messagesObject.messages.Count > 0))
            {
                messages = messagesObject.messages;
            }
        }


    }
}
