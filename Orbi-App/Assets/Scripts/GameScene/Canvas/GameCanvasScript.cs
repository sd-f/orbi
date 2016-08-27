using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCanvasScript : MonoBehaviour {

    Vector2 lastScreenSize = new Vector2(1920, 1080);
    RectTransform bgImageRectTransform;

    // Use this for initialization
    void Awake()
    {
        bgImageRectTransform = GameObject.Find("CanvasBackgroundImage").GetComponent<RectTransform>();
        InvokeRepeating("OnScreenSizeChanged", 0, 1);
    }


    void OnScreenSizeChanged()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {
            bgImageRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
