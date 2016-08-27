using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Message
{
    public static List<Message> MESSAGES = new List<Message>();
    private static long SEQUENCE = 1;

    private Color color;
    private Color textColor;
    private String text;
    private long id;

    public Message(String text, Color color, Color textColor)
    {
        this.id = SEQUENCE++;
        this.color = color;
        this.text = text;
        this.textColor = textColor;
    }

    public Color GetColor()
    {
        return this.color;
    }
    public Color GetTextColor()
    {
        return this.textColor;
    }

    public String GetText()
    {
        return this.text;
    }

    public long GetId()
    {
        return this.id;
    }

}

