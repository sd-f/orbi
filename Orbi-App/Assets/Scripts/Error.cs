using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Error
{
    public static void Show(string message)
    {
        ImageMessageScript.ShowError(message);
    }
}
