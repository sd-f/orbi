using UnityEngine;
using Assets.Control;

public class StartInitScript : MonoBehaviour {

    public ServerType server = ServerType.PROD;

	void Start () {
        Game.GetInstance().SetServer(server);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
