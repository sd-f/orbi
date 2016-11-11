using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace CanvasUtility
{
    class CanvasDebug
    {
        public static void Log(string text)
        {
            UnityEngine.GameObject.Find("DebugText").GetComponent<Text>().text = text;
        }
    }
}
