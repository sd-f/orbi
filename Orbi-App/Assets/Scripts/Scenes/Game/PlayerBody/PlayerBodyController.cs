using UnityEngine;
using GameController;
using System;
using UnityEngine.UI;
using ServerModel;
using ClientModel;
using UnityStandardAssets.Characters.FirstPerson;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerBodyController")]
    class PlayerBodyController : MonoBehaviour
    {

        // handheld movement
        private Vector3 gyroRotation = new Vector3(0,0,0);
        private bool gyroEnabled = false;
        private Vector3 targetPosition = new Vector3(0, 0, 0);
        private float deltaCompass = 0.0f;
        private MyFirstPersonController firstPersonController;
        public Camera cam;

        void Awake()
        {
            Input.gyro.enabled = true;
            SensorHelper.ActivateRotation();
            //SensorHelper.ActivateRotation();
            gyroEnabled = Game.GetGame().GetSettings().IsHandheldInputEnabled();

            if (gyroEnabled)
            {
                // TODO iphone initial attitude maybe different
                gyroRotation.x = Input.gyro.attitude.eulerAngles.y - 90f;
                //gyroRotation.y = Input.gyro.attitude.eulerAngles.z;
            }

            Invoke("UpdateDeltaCompass", 1f);
            Invoke("UpdateTransformInModel", 1f);
        }

        void Start()
        {
            firstPersonController = GetComponent<MyFirstPersonController>();
            Input.gyro.enabled = true;
            // restore rotation + position

            SetTargetPosition(Game.GetPlayer().GetModel().character.transform.position.ToVector3());
            SetRotation(Game.GetPlayer().GetModel().character.transform.rotation.ToVector3());
            //SensorHelper.ActivateRotation();
        }

        void Update()
        {
            
            if (!Game.GetPlayer().IsFrozen() && gyroEnabled)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 2);
            }
            if (gyroEnabled)
                ApplyGyroRotation();
                
        }

        public void SetMouseRotationEnabled(bool enabled)
        {
            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                firstPersonController.enabled = true;
                firstPersonController.mouseLook.SetCursorLock(true);
                
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                firstPersonController.enabled = false;
                firstPersonController.mouseLook.SetCursorLock(false);
            }
        }

        void UpdateTransformInModel()
        {
            Game.GetPlayer().GetModel().character.transform.geoPosition = new Position(this.transform.position).ToGeoPosition();
            Game.GetPlayer().GetModel().character.transform.position = new Position(this.transform.position);
            Game.GetPlayer().GetModel().character.transform.rotation = new Rotation(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,0);
            Invoke("UpdateTransformInModel", 1f);
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            this.targetPosition.x = targetPosition.x;
            this.targetPosition.z = targetPosition.z;
            this.targetPosition.y = this.transform.position.y;
        }

        public void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, rotation.y, 0));
            cam.transform.rotation = Quaternion.Euler(new Vector3(rotation.x, cam.transform.rotation.eulerAngles.y, 0));
        }

        public void UpdateDeltaCompass()
        {
            Game.GetLocation().SetCompassDelta(gyroRotation.y - Game.GetLocation().GetCompassValue());
            deltaCompass = Game.GetLocation().GetCompassDelta();
        }

        void ApplyGyroRotation()
        {
            gyroRotation.x = SensorHelper.rotation.eulerAngles.x;
            gyroRotation.y = SensorHelper.rotation.eulerAngles.y;

            
            
           /* Text text = GameObject.Find("DebugText").GetComponent<Text>();
            text.text = "Input.gyro.attitude.eulerAngles: " + Input.gyro.attitude.eulerAngles
                + "\nGetCompassValue: " + Game.GetLocation().GetCompassValue()
                + "\nSensorHelper.rotation.eulerAngles: " + SensorHelper.rotation.eulerAngles
                + "\ndeltaCompass: " + deltaCompass;
                */
            // y on body
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(transform.rotation.eulerAngles.x, gyroRotation.y - deltaCompass, 0.0f)
                , Time.deltaTime * 5f);

            // x on cam
            cam.transform.rotation = ClampRotationAroundXAxis(
                Quaternion.Slerp(
                    cam.transform.rotation,
                    Quaternion.Euler(gyroRotation.x, cam.transform.rotation.eulerAngles.y, 0.0f),
                    Time.deltaTime * 5f
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
            height = Game.GetWorld().GetHeight(x, z);
            newHeight = Game.GetWorld().GetHeight(x - 0.5f, z - 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.GetWorld().GetHeight(x + 0.5f, z - 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.GetWorld().GetHeight(x - 0.5f, z + 0.5f);
            if (newHeight > height)
                height = newHeight;
            newHeight = Game.GetWorld().GetHeight(x + 0.5f, z + 0.5f);
            if (newHeight > height)
                height = newHeight;
            return height;
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}