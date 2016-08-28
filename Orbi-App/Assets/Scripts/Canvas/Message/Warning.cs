﻿using UnityEngine;
using GameController;

namespace CanvasUtility
{
    public class Warning
    {
        public static void Show(string message)
        {
            Game.GetGame().GetUi().GetMessageQueue().Enqueue(new Message(message, Color.black, Color.yellow));
        }
    }
}
