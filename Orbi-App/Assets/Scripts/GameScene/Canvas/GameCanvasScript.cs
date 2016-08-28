using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Control;

public class GameCanvasScript : MonoBehaviour {

    private GameController.Player player;
    Vector2 lastScreenSize = new Vector2(1920, 1080);
    RectTransform bgImageRectTransform;

    void Start()
    {
        player = GameController.Game.GetPlayer();
    }

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
