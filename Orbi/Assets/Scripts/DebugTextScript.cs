using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Text>().text = "x = " + Sensor.GetOrientation().x + " y = " + Sensor.GetOrientation().y + " q= " + Quaternion.Euler(Sensor.GetOrientation().x, Sensor.GetOrientation().y, 0);

    }
}
