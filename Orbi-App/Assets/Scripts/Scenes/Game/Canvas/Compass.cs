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
        public GameObject playerBody;

        private float heading = 0.0f;
        private float headingVelocity = 0.0f;
        private bool headingNorth = false;

        void Start()
        {
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            //Sensor.Activate(Sensor.Type.MagneticField);
            //Sensor.Activate(Sensor.Type.Accelerometer);
            //SensorHelper.ActivateRotation();
            headingNorth = false;
            //InvokeRepeating("CheckIfNorth", 1, 0.1f);
        }

        void Awake()
        {
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            //Sensor.Activate(Sensor.Type.MagneticField);
            //Sensor.Activate(Sensor.Type.Accelerometer);
            //SensorHelper.ActivateRotation();
            headingNorth = false;
            //InvokeRepeating("CheckIfNorth", 1, 0.1f);
        }

        void Update()
        {
            heading = Mathf.LerpAngle(heading, Input.compass.trueHeading, Time.deltaTime * 5f);
            compassImage.transform.rotation = Quaternion.Slerp(compassImage.transform.rotation, Quaternion.Euler(0, 0, heading), Time.deltaTime * 2);
            Game.GetLocation().SetCompassValue(compassImage.transform.rotation.eulerAngles.z);
        }

        void CheckIfNorth() {
            if (isNorth())
            {
                if (!headingNorth)
                {
                    headingNorth = true;
                    buttonBackground.color = Color.green;
                }
            }
            else
            {
                if (headingNorth)
                {
                    headingNorth = false;
                    buttonBackground.color = Color.white;
                }
            }

        }

        public void OnCalibrate()
        {
            playerBody.GetComponent<PlayerBodyController>().UpdateDeltaCompass();
        }

        private bool isNorth()
        {
            return ((heading) < 5.0f) && ((heading) > -5.0f);
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}
