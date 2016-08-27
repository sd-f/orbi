using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Warning
{
    public static void Show(string message)
    {
        Message.MESSAGES.Add(new Message(message, Color.yellow, Color.black));
    }
}
