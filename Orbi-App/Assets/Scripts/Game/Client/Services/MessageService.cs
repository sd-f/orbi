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
            WWW request = Request("player/message", JsonUtility.ToJson(newMessage));
            yield return request;
            if (request.error == null)
            {
                Info.Show("Message sent.");
                IndicateRequestFinished();
            }
            else
                HandleError(request);
        }

        public IEnumerator RequestMessages(MessagesCanvas canvas)
        {
            WWW request = Request("player/messages", null);
            yield return request;
            if (request.error == null)
            {
                
                CharacterMessages messagesObject = JsonUtility.FromJson<CharacterMessages>(request.text);
                if (messagesObject.messages != null)
                {
                    List<ServerModel.CharacterMessage> messages = messagesObject.messages;
                    Game.GetPlayer().GetMessages().AddRange(messages);
                    if (messages.Count > 0)
                    {
                        canvas.Unread();
                    }
                }
                
                IndicateRequestFinished();
            }
            else
                HandleError(request);
        }

    }
}
