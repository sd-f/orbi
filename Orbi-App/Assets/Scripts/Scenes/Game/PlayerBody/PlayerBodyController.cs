﻿using UnityEngine;
using GameController;
using System;
using UnityEngine.UI;
using ServerModel;
using ClientModel;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using CanvasUtility;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerBodyController")]
    class PlayerBodyController : GameMonoBehaviour
    {

        // handheld movement
        private Vector3 gyroRotation = new Vector3(0,0,0);
        public Vector3 targetPosition = new Vector3(0, GameController.Player.HEIGHT, 0);
        private float deltaCompass = 0.0f;
        private MyFirstPersonController firstPersonController;
#pragma warning disable 0649
        public Camera cam;

        private long updateCounter = 0;

        public override void Start()
        {
            base.Start();
            targetPosition = new Vector3(0, GameController.Player.HEIGHT, 0);
            Input.gyro.enabled = true;
            SensorHelper.ActivateRotation();
            //SensorHelper.ActivateRotation();
            if (!desktopMode)
            {
                // TODO iphone initial attitude maybe different
                gyroRotation.x = Input.gyro.attitude.eulerAngles.y - 90f;
                //gyroRotation.y = Input.gyro.attitude.eulerAngles.z;
            }
            firstPersonController = GetComponent<MyFirstPersonController>();
            SetTransform(new Vector3(0, GameController.Player.HEIGHT, 0), new Vector3(0, 0.5f, 0));
        }

        

        public override void Awake()
        {
            base.Awake();
            Input.gyro.enabled = true;
            SensorHelper.ActivateRotation();
        }

        public override void OnReady()
        {
            base.OnReady();
            if (!IsInvoking("UpdateTransformInModel"))
                InvokeRepeating("UpdateTransformInModel", 1f, 1f);
            if (!IsInvoking("UpdateDeltaCompass"))
                Invoke("UpdateDeltaCompass", 2f);
            Game.Instance.GetLocation().SetCompassDelta(gyroRotation.y - Game.Instance.GetLocation().GetCompassValue());
            deltaCompass = Game.Instance.GetLocation().GetCompassDelta();
        }


        void Update()
        {
           // CanvasDebug.Log(targetPosition.ToString());
            if (!desktopMode && ready)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 2);
                ApplyGyroRotation();
            }
              
        }

        public override void OnInputModeChanged()
        {
            firstPersonController.enabled = desktopMode;
            if (desktopMode) {
                CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);
                firstPersonController.enabled = !typingMode;
                firstPersonController.mouseLook.SetCursorLock(!typingMode);
            } else
            {
                CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Touch);
            }
        }

        public override void OnTypingModeChanged()
        {
            if (desktopMode)
            {
                firstPersonController.enabled = !typingMode;
                firstPersonController.mouseLook.SetCursorLock(!typingMode);
            }
                
        }

        public void UpdateTransformInModel()
        {
            updateCounter++;
            Position pos = new Position(this.transform.position);
            
            Game.Instance.GetPlayer().GetModel().character.transform.geoPosition = pos.ToGeoPosition();
            Game.Instance.GetPlayer().GetModel().character.transform.position = pos;
            Game.Instance.GetPlayer().GetModel().character.transform.rotation = new Rotation(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,0);

            if (updateCounter > 5)
            {
                updateCounter = 0;
                StartCoroutine(Game.Instance.GetPlayer().GetPlayerService().RequestUpdateTransform());
            }
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {

            this.targetPosition.x = targetPosition.x;
            this.targetPosition.z = targetPosition.z;
            this.targetPosition.y = targetPosition.y;
        }

        public void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, rotation.y, 0));
            cam.transform.rotation = Quaternion.Euler(new Vector3(rotation.x, cam.transform.rotation.eulerAngles.y, 0));
        }

        public void UpdateDeltaCompass()
        {
            if (!Game.Instance.GetLocation().IsDeltaUpdated())
            {
                Game.Instance.GetLocation().SetCompassDelta(gyroRotation.y - Game.Instance.GetLocation().GetCompassValue());
                deltaCompass = Game.Instance.GetLocation().GetCompassDelta();
            }
            Game.Instance.GetLocation().SetDeltaUpdated(true);
        }

        void ApplyGyroRotation()
        {
            gyroRotation.x = SensorHelper.rotation.eulerAngles.x;
            gyroRotation.y = SensorHelper.rotation.eulerAngles.y;

            
            
           /* Text text = GameObject.Find("DebugText").GetComponent<Text>();
            text.text = "Input.gyro.attitude.eulerAngles: " + Input.gyro.attitude.eulerAngles
                + "\nGetCompassValue: " + Game.Instance.GetLocation().GetCompassValue()
                + "\nSensorHelper.rotation.eulerAngles: " + SensorHelper.rotation.eulerAngles
                + "\ndeltaCompass: " + deltaCompass;
                */
            // y on body
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(transform.rotation.eulerAngles.x, gyroRotation.y - deltaCompass, 0.0f)
                , Time.deltaTime * 10f);

            // x on cam
            cam.transform.rotation = ClampRotationAroundXAxis(
                Quaternion.Slerp(
                    cam.transform.rotation,
                    Quaternion.Euler(gyroRotation.x, cam.transform.rotation.eulerAngles.y, 0.0f),
                    Time.deltaTime * 10f
                    )
            );
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, -45f, 90f);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        // player box todo collider
        private float GetHeight(float x, float z)
        {
            float newHeight = 0.0f;
            float height = 0.0f;
            height = Game.Instance.GetWorld().GetHeight(x, z);
            newHeight = Game.Instance.GetWorld().GetHeight(x - 0.5f, z - 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.Instance.GetWorld().GetHeight(x + 0.5f, z - 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.Instance.GetWorld().GetHeight(x - 0.5f, z + 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.Instance.GetWorld().GetHeight(x + 0.5f, z + 0.5f);
            if (newHeight > height)
                height = newHeight;
            return height;
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(0.0f, GameController.Player.HEIGHT, 0.0f);
            targetPosition = transform.position;
            this.GetComponent<Rigidbody>().transform.position = new Vector3(0.0f, GameController.Player.HEIGHT, 0.0f);
        }

        public void SetTransform(Vector3 position, Vector3 rotation)
        {
            transform.position = position;
            targetPosition = position;
            //this.GetComponent<Rigidbody>().transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
            SetRotation(rotation);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CancelInvoke();
        }

    }
}