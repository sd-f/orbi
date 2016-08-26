using UnityEngine;
using Assets.Control;
using System.Collections;

public class StartInitScript : MonoBehaviour {

    public static ServerType server = ServerType.LOCAL;

    void Start()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        Game.GetInstance().SetServer(server);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
