﻿using GameController;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/Compass")]
    class Compass : MonoBehaviour
    {
#pragma warning disable 0649
        //public Image compassImage;
        public Image buttonBackground;
        public Text compassIcon;
        public PlayerBodyController controller;

        private float heading = 0.0f;
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
            compassIcon.transform.rotation = Quaternion.Slerp(compassIcon.transform.rotation, Quaternion.Euler(0, 0, heading), Time.deltaTime * 2);
            Game.Instance.GetLocation().SetCompassValue(compassIcon.transform.rotation.eulerAngles.z);
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
            Game.Instance.GetLocation().SetDeltaUpdated(false);
            controller.UpdateDeltaCompass();
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
