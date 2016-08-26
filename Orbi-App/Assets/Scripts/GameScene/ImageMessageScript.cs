using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageMessageScript : MonoBehaviour {

    private Image bgImage;
    private Text text;
    private static int SHOW_FOR_SECONDS = 0;
    private static ImageMessageScript INSTANCE;
    Vector2 lastScreenSize = new Vector2(1920, 1080);

    void Awake () {
        INSTANCE = this;
        bgImage = GetComponent<Image>();
        text = GameObject.Find("TextMessage").GetComponent<Text>();
        Invoke("CheckToShow", 0);
        InvokeRepeating("OnScreenSizeChanged", 0.5f, 2);
    }

    public static void ShowError(string text)
    {
        INSTANCE.text.text = text;
        INSTANCE.text.color = Color.white;
        INSTANCE.bgImage.color = Color.red;
        SHOW_FOR_SECONDS = 3;
    }

    public static void ShowWarning(string text)
    {
        INSTANCE.bgImage.color = Color.yellow;
        INSTANCE.text.color = Color.black;
        INSTANCE.text.text = text;
        SHOW_FOR_SECONDS = 3;
    }

    public static void ShowInfo(string text)
    {
        INSTANCE.bgImage.color = Color.green;
        INSTANCE.text.color = Color.black;
        INSTANCE.text.text = text;
        SHOW_FOR_SECONDS = 3;
    }

    void CheckToShow()
    {
        if (SHOW_FOR_SECONDS > 0)
        {
            Show();
            SHOW_FOR_SECONDS--;
        } else
        {
            Hide();
        }
        Invoke("CheckToShow", 1);
    }

	void Hide () {
        bgImage.enabled = false;
        text.enabled = false;
    }

    void Show()
    {
        bgImage.enabled = true;
        text.enabled = true;
    }

    void OnScreenSizeChanged()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {
            bgImage.rectTransform.sizeDelta = new Vector2(Screen.width, bgImage.rectTransform.rect.height);
            text.rectTransform.sizeDelta = new Vector2(Screen.width-50, text.rectTransform.rect.height);
            //text.rectTransform.localPosition = new Vector3((bgImage.rectTransform.rect.width/2) + 25, -25, 0);
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
