using GameController;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/Compass")]
    class Compass : MonoBehaviour
    {
        public Image compassImage;
        public Image buttonBackground;
        public GameObject playerCamera;

        private FloatFilter magneticFilter = new AngleFilter(10);
        private bool headingNorth;

        void Start()
        {
            headingNorth = true;
            Sensor.Activate(Sensor.Type.MagneticField);
            Sensor.Activate(Sensor.Type.Accelerometer);
        }

        void Update()
        {
            Text text = GameObject.Find("DebugText").GetComponent<Text>();
            text.text = "location: " + Game.GetLocation().GetGeoLocation() + "\n";


            compassImage.transform.rotation = Quaternion.Slerp(compassImage.transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
            if (isNorth())
                if (headingNorth)
                {
                    headingNorth = true;
                    buttonBackground.color = Color.green;
                }
                else
                    Debug.Log("Still");
            else
                if (headingNorth)
            {
                headingNorth = false;
                buttonBackground.color = Color.white;
            }
            
        }

        public void OnCalibrate()
        {
            playerCamera.SendMessage("UpdateDeltaCompass");
        }

        private bool isNorth()
        {
            return ((magneticFilter.Value) < 5.0f) && ((magneticFilter.Value) > -5.0f);
        }

    }
}
