using UnityEngine;
using System.Collections;

public class TestGyro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        var x = Input.gyro.rotationRateUnbiased.x;
        var y = Input.gyro.rotationRateUnbiased.y;
        var z = Input.gyro.rotationRateUnbiased.z;
        //Debug.Log("x " + x + " y " + y + " z " + z);
        transform.eulerAngles = new Vector3(x, y, z);
        */
    }
    /* from http://forum.unity3d.com/threads/sharing-gyroscope-camera-script-ios-tested.241825/
      private float initialYAngle = 0f;
      private float appliedGyroYAngle = 0f;
      private float calibrationYAngle = 0f;

      void Start()
      {
          Application.targetFrameRate = 60;
          initialYAngle = transform.eulerAngles.y;
      }

      void Update()
      {
          ApplyGyroRotation();
          ApplyCalibration();
      }

      void OnGUI()
      {
          if (GUILayout.Button("Calibrate", GUILayout.Width(300), GUILayout.Height(100)))
          {
              CalibrateYAngle();
          }
      }

      public void CalibrateYAngle()
      {
          calibrationYAngle = appliedGyroYAngle - initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
      }

      void ApplyGyroRotation()
      {
          transform.rotation = Input.gyro.attitude;
          transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
          transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
          appliedGyroYAngle = transform.eulerAngles.y; // Save the angle around y axis for use in calibration.
      }

      void ApplyCalibration()
      {
          transform.Rotate(0f, -calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
      }
      */

}
