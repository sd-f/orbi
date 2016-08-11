using UnityEngine;
using System.Collections;

public class CameraControlScript : MonoBehaviour
{

    private Vector3 firstpoint; //change type on Vector3
    private Vector3 secondpoint;
    private float xAngle = 0.0F; //angle for axes x for rotation
    private float yAngle = 0.0F;
    private float xAngTemp = 0.0F; //temp variable for angle
    private float yAngTemp = 0.0F;
    public float speed = 100.0F;

    // gyro
    private float initialYAngle = 0f;
    private float appliedGyroYAngle = 0f;
    private float calibrationYAngle = 0f;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;
        initialYAngle = transform.eulerAngles.y;
        xAngle = 0.0F;
        yAngle = 0.0F;
        this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0F);
    }

    void Update()
    {
        touchCameraRotation();
        keyCameraRotation();
        ApplyGyroRotation();
        ApplyCalibration();
    }

    void keyCameraRotation()
    {
        // fix z axis to zero
        // key rotation of camera for debug only
        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(v.x, v.y, 0.0f);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        }
    }

    void touchCameraRotation()
    {
        //Check count touches
        if (Input.touchCount > 0)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0F / Screen.width;
                yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0F / Screen.height;
                //Rotate camera
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0F);
            }
        }
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
}
