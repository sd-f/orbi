using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Error
{
    public static void Show(string message)
    {
        Message.MESSAGES.Add(new Message(message, Color.red, Color.white));
    }
}
