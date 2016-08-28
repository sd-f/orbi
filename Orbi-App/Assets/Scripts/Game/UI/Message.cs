using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace GameController
{

    public class Message
    {
        private string text = "Message";
        private Color color = Color.black;
        private Color backgroundColor = Color.white;

        public Message(string text, Color color, Color backgroundColor)
        {
            this.text = text;
            this.color = color;
            this.backgroundColor = backgroundColor;
        }

        public Color GetColor()
        {
            return this.color;
        }

        public string GetText()
        {
            return this.text;
        }

        public Color GetBackgroundColor()
        {
            return this.backgroundColor;
        }

    }

}

