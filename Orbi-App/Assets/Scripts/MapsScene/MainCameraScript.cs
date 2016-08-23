using UnityEngine;
using System.Collections;
using Assets.Control;

public class MainCameraScript : MonoBehaviour {

    // keyboard rotation - debug only
    private float KEY_ROTATION_SPEED = 100.0F;

    // gyro rotation
    public static Quaternion GYRO_ROTATION;
    public static float DELTA_COMPASS = 0.0f;

    void Start () {
	}
	
	void Update () {
        // keyboard rotation - debug only
        if (SystemInfo.deviceType == DeviceType.Desktop)
            KeyboardRotation();
        else
            GyroRotation();
    }

    void GyroRotation()
    {
        GYRO_ROTATION = SensorHelper.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(GYRO_ROTATION.eulerAngles.x, GYRO_ROTATION.eulerAngles.y - DELTA_COMPASS, GYRO_ROTATION.eulerAngles.z)
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
