
using System.Collections.Generic;
using UnityEngine;


namespace GameController
{

    public class Ui
    {
        private Queue<Message> messages = new Queue<Message>();

        public Queue<Message> GetMessageQueue()
        {
            return this.messages;
        }
    }

}

