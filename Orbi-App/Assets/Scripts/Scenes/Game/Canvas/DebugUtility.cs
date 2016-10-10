using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasUtility
{
    public class DebugUtility
    {

        public static void SetDebugText(string text)
        {
            Debug.Log("Debug log " + text);
            GameObject goPanel = GameObject.Find("DebugPanel");
            if (goPanel != null)
            {
                Text textObject = goPanel.GetComponentInChildren<Text>();
                textObject.text = text;
            }
            
        }
    }

        
}
