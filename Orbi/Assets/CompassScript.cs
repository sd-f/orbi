using UnityEngine;
using System.Collections;

public class CompassScript : MonoBehaviour {

    void Start()
    {
        Sensor.Activate(Sensor.Type.MagneticField);
        Sensor.Activate(Sensor.Type.Accelerometer);
        InvokeRepeating("UpdateHeading", 0, 1);
    }

    FloatFilter magneticFilter = new AngleFilter(10);

    void UpdateHeading()
    {
        Camera camera = GameObject.Find("cameraMain").GetComponent<Camera>();
        //Debug.Log("update compass" + magneticFilter);
        CameraControlScript.magneticFilter = magneticFilter;
        //camera.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, magneticFilter.Update(Sensor.GetOrientation().x), 0), Time.deltaTime * 2);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
    }
}
