using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Info
{
    public static void Show(string message)
    {
        Message.MESSAGES.Add(new Message(message, Color.green, Color.black));
    }
}
