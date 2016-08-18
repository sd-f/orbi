﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update () {
        this.GetComponent<Text>().text = "gyro = " + CameraControlScript.gyroRotation.eulerAngles
            + "\ndelta = " + CameraControlScript.deltaCompass
            + "\ncorrected = " + CameraControlScript.gyroRotationCorrected.eulerAngles
            + "\nfiltered = " + CompassScript.magneticFilter.Value
            + "\nSensor.GetOrientation().x = " + Sensor.GetOrientation().x;
    }
}
