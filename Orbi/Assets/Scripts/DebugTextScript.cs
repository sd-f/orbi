using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update () {
        GameObject craftcontainer = GameObject.Find("gameObjectCubeToCraftContainer");
        this.GetComponent<Text>().text = "x = " + craftcontainer.transform.position.x
            + ", y = " + +craftcontainer.transform.position.z;

    }
}
