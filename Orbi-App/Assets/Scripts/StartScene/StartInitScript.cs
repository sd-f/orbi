using UnityEngine;
using Assets.Control;

public class StartInitScript : MonoBehaviour {

    public static ServerType server = ServerType.PROD;

	void Start () {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        Game.GetInstance().SetServer(server);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
