using UnityEngine;
using System.Collections;
using GameController;

public class ShadowText : MonoBehaviour {

    private TextMesh foreGround;
    private TextMesh shadow;

    private Color foreGroundColor = Color.black;
    private Color shadowColor = Color.white;

    private TextAlignment textAlign = TextAlignment.Left;
    private TextAnchor anchor = TextAnchor.UpperLeft;

    private string text;

    void Start()
    {
        foreGround = GameObjectUtility.FindChildWithName(this.gameObject, "ForegroundText").GetComponent<TextMesh>();
        shadow = GameObjectUtility.FindChildWithName(this.gameObject, "ShadowText").GetComponent<TextMesh>();
        SetValues();
    }

    public void SetForeGroundColor(Color color)
    {
        this.foreGroundColor = color;
    }

    public void SetShadowColor(Color color)
    {
        this.shadowColor = color;
    }

    public void SetAlignment(TextAlignment textAlign, TextAnchor anchor)
    {
        this.textAlign = textAlign;
        this.anchor = anchor;
    }

    public void SetText(string text)
    {
        this.text = text;
    }

    private void SetValues()
    {
        foreGround.text = text;
        foreGround.color = foreGroundColor;
        foreGround.alignment = textAlign;
        foreGround.anchor = anchor;
        shadow.text = text;
        shadow.color = shadowColor;
        shadow.alignment = textAlign;
        shadow.anchor = anchor;
    }
}
