using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Control;

public class SettingsCanvasScript : MonoBehaviour {

    public void Back()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
