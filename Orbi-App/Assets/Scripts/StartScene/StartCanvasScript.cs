using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartCanvasScript : MonoBehaviour
{


    Vector2 lastScreenSize = new Vector2(1920,1080);
    RectTransform bgImageRectTransform;

    // Use this for initialization
    void Awake()
    {
        bgImageRectTransform = GameObject.Find("StartBackgroundImage").GetComponent<RectTransform>();
        InvokeRepeating("OnScreenSizeChanged", 0.5f, 1);
    }


    void OnScreenSizeChanged ()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {
            
            bgImageRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
    }


}
	
