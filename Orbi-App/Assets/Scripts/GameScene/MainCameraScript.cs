using UnityEngine;
using System.Collections;
using Assets.Control;

public class MainCameraScript : MonoBehaviour {

    // keyboard rotation - debug only
    private float KEY_ROTATION_SPEED = 10.0F;

    // gyro rotation
    private Quaternion gyroRotation;
    private float deltaCompass = 0.0f;

    void Awake () {
        SensorHelper.ActivateRotation();
    }
	
	void Update () {
        // keyboard rotation - debug only
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
           KeyboardRotation();
        }
        else
            GyroRotation();
    }

    public void UpdateDeltaCompass()
    {
        deltaCompass = gyroRotation.eulerAngles.y;
    }

    void GyroRotation()
    {
        gyroRotation = SensorHelper.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(gyroRotation.eulerAngles.x, gyroRotation.eulerAngles.y - deltaCompass, gyroRotation.eulerAngles.z)
            , Time.deltaTime * 10f);
    }

    void KeyboardRotation()
    {
        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(v.x, v.y, 0.0f);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Rotate(new Vector3(KEY_ROTATION_SPEED * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Rotate(new Vector3(-KEY_ROTATION_SPEED * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(new Vector3(0, -KEY_ROTATION_SPEED * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(new Vector3(0, KEY_ROTATION_SPEED * Time.deltaTime, 0));
    }
}
