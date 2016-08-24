using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Warning
{
    public static void Show(string message)
    {
        ImageMessageScript.ShowWarning(message);
    }
}
