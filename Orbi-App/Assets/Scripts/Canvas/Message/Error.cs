using GameController;
using UnityEngine;

namespace CanvasUtility
{
    public class Error
    {
        public static void Show(string message)
        {
            Game.GetGame().GetUi().GetMessageQueue().Enqueue(new Message(message, Color.white, Color.red));
        }
    }
}

