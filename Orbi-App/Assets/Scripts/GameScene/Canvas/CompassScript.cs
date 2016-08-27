using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour {

    private FloatFilter magneticFilter = new AngleFilter(10);
    private bool headingNorth = true;
    private Image image;
    private MainCameraScript cameraScript;
    private GameObject buttonCompass;

    void Awake()
    {
        Sensor.Activate(Sensor.Type.MagneticField);
        Sensor.Activate(Sensor.Type.Accelerometer);
        cameraScript = GameObject.Find("MainCamera").GetComponent<MainCameraScript>();
        image = GetComponent<Image>();
        image.color = Color.green;
        buttonCompass = this.gameObject;
    }

    private bool isNorth()
    {
        return ((magneticFilter.Value) < 5.0f) && ((magneticFilter.Value) > -5.0f);
    }

    void Update()
    {
        buttonCompass.transform.rotation = Quaternion.Slerp(buttonCompass.transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
        if (isNorth())
        {
            if (!headingNorth)
            {
                headingNorth = true;
                image.color = Color.green;
            }
        }
        else
        {
            if (headingNorth)
            {
                headingNorth = false;
                image.color = Color.white;
            }
        }
    }

    public void SetDeltaCompass()
    {
        cameraScript.UpdateDeltaCompass();
    }

}
