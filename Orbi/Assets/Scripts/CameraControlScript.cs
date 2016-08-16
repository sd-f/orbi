﻿using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    // keyboard + touch
    private Vector3 firstpoint; //change type on Vector3
    private Vector3 secondpoint;
    private float xAngle = 0.0F; //angle for axes x for rotation
    private float yAngle = 0.0F;
    private float xAngTemp = 0.0F; //temp variable for angle
    private float yAngTemp = 0.0F;
    public float speed = 100.0F;

    public static Quaternion gyroRotation;
    public static Quaternion gyroRotationCorrected;
    //public static Quaternion compass;
    public static float deltaCompass = 0.0f;

    // Use this for initialization
    void Start()
    {
        SensorHelper.ActivateRotation();
        //SensorHelper.TryForceRotationFallback(RotationFallbackType.MagneticField);
        //this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0F);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, magneticFilter.Update(Sensor.GetOrientation().x), 0);
        //InvokeRepeating("UpdateDirectionFromCompass", 0, 5);
    }

    public static void fixNorthHeading()
    {
        deltaCompass = gyroRotation.y * 270.0f;
    }

    void Update()
    {
        gyroRotation = SensorHelper.rotation;
        // todo better slerp speed
        gyroRotationCorrected = Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(gyroRotation.eulerAngles.x, gyroRotation.eulerAngles.y - deltaCompass, gyroRotation.eulerAngles.z)
            , Time.deltaTime * 10f);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(SensorHelper.rotation.x, SensorHelper.rotation.y, SensorHelper.rotation.z), Time.deltaTime * 2);
        //transform.rotation = Quaternion.Euler(SensorHelper.rotation.x, SensorHelper.rotation.y, SensorHelper.rotation.z);
        transform.rotation = gyroRotationCorrected;
        //SensorHelper.
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90f, 0), Time.deltaTime * 2);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(SensorHelper.rotation.x, SensorHelper.rotation.y, SensorHelper.rotation.z), Time.deltaTime * 2);
        //Debug.Log("gyro enabled " + Input.gyro.enabled);
        //Debug.Log("gyro supported " + SystemInfo.supportsGyroscope);
        //touchCameraRotation();
        //keyCameraRotation();
        //ApplyGyroRotation();
        //ApplyCalibration();
        //var x = Input.gyro.rotationRateUnbiased.x;
        //var y = Input.gyro.rotationRateUnbiased.y;
        //var z = Input.gyro.rotationRateUnbiased.z;
        //Quaternion rotFix = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        //Debug.Log("x " + Input.gyro.attitude.x + " y " + Input.gyro.attitude.y + " z " + Input.gyro.attitude.z);
        //transform.eulerAngles = new Vector3(x, y, z);
        //transform.localRotation = rotFix;
        //Quaternion newRot = Quaternion.Euler(Input.compass.rawVector.y, -Input.compass.rawVector.x, 0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.1f * Time.deltaTime);
        //Debug.Log(Input.compass);
        //Debug.Log(Input.compass.rawVector);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(xRot, yRot, zRot)), Time.deltaTime * rotSpeed);
        //Debug.Log(transform.rotation);
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

}