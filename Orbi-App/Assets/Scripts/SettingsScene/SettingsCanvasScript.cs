using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Control;

public class SettingsCanvasScript : MonoBehaviour {

    public void Back()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (SystemInfo.deviceType == DeviceType.Handheld))
            Application.Quit();
    }
}
