using UnityEngine;
using Assets.Control;

public class GameStartScript : MonoBehaviour {

    public ServerType server = ServerType.PROD;

	// Use this for initialization
	void Start () {
        Game.GetInstance().SetServer(server);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
