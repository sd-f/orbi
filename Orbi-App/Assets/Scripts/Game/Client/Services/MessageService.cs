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

        public IEnumerator RequestMessages(MessagesCanvas canvas)
        {
            yield return Request("player/messages", null, OnMessagesRecieved, canvas);
        }

        private void OnMessagesRecieved(string data, object canvas)
        {
            CharacterMessages messagesObject = JsonUtility.FromJson<CharacterMessages>(data);
            if (messagesObject.messages != null)
            {
                List<ServerModel.CharacterMessage> messages = messagesObject.messages;
                Game.GetPlayer().GetMessages().AddRange(messages);
                if ((messages.Count > 0) && (canvas != null))
                    ((MessagesCanvas)canvas).Unread();
            }
        }


    }
}
