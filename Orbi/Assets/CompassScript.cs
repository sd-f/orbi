using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour {

    Button button;
    public static bool headingNorth = false;
    public static FloatFilter magneticFilter = new AngleFilter(10);

    void Start()
    {
        button = GameObject.Find("buttonReadCompass").GetComponent<Button>();
        Sensor.Activate(Sensor.Type.MagneticField);
        Sensor.Activate(Sensor.Type.Accelerometer);
        InvokeRepeating("UpdateHeading", 0, 1);
        
        headingNorth = isNorth();
        button.image.color = Color.green;
    }

    void UpdateHeading()
    {

        //camera.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, magneticFilter.Update(Sensor.GetOrientation().x), 0), Time.deltaTime * 2);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
    }

    private bool isNorth()
    {
        return (magneticFilter.Value < 5.0f) && (magneticFilter.Value > -5.0f);
    }

    void Update()
    {
        if (isNorth())
        {
            if (!headingNorth)
            {
                headingNorth = true;
                button.image.color = Color.green;
            }
        }
        else
        {
            if (headingNorth)
            {
                headingNorth = false;
                button.image.color = Color.white;
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
    }
}
