using UnityEngine;
using System.Collections;
using GameController;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerCamera")]
    class PlayerCamera : MonoBehaviour
    {

        // handheld movement
        private Quaternion gyroRotation;
        private float deltaCompass = 0.0f;

        // mouse/keyboard
        private float speedH = 1.5f;
        private float speedV = 1.5f;
        private float yaw = 0.0f;
        private float pitch = 0.0f;
        private float speed = 2f;
        private float spacing = 1.0f;
        private Vector3 positionTmp;

        public GameObject frontOfCamera;
        public GameObject behindOfCamera;
        public GameObject rightOfCamera;
        public GameObject leftOfCamera;

        void Start()
        {
            SensorHelper.ActivateRotation();
        }

        void Awake()
        {
            yaw = 0.0f;
            pitch = 0.0f;
        }

        public void ResetPosition()
        {
            positionTmp = transform.position;
        }

        void Update()
        {
            
            if (Game.GetGame().GetSettings().IsDesktopInputEnabled())
            {
                ApplyMouseRotation();
                ApplyKeyboardMove();
            } 
            else 
                ApplyGyroRotation();
        }

        public void UpdateDeltaCompass()
        {
            deltaCompass = gyroRotation.eulerAngles.y;
        }

        void ApplyGyroRotation()
        {
            gyroRotation = SensorHelper.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(gyroRotation.eulerAngles.x, gyroRotation.eulerAngles.y - deltaCompass, 0.0f)
            , Time.deltaTime * 10f);
        }

        void ApplyMouseRotation()
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        void ApplyKeyboardMove()
        {
            if (Input.GetKey(KeyCode.W))
                transform.position = Vector3.MoveTowards(transform.position, MoveToPositionWithHeight(frontOfCamera.transform.position), speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
                transform.position = Vector3.MoveTowards(transform.position, MoveToPositionWithHeight(behindOfCamera.transform.position), speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                transform.position = Vector3.MoveTowards(transform.position, MoveToPositionWithHeight(leftOfCamera.transform.position), speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                transform.position = Vector3.MoveTowards(transform.position, MoveToPositionWithHeight(rightOfCamera.transform.position), speed * Time.deltaTime);
        }

        Vector3 MoveToPositionWithHeight(Vector3 moveToPosition)
        {
            Vector3 newMoveToPosition = moveToPosition;
            moveToPosition.y = Game.GetWorld().GetHeight(moveToPosition.x, moveToPosition.z);
            return newMoveToPosition;
        }

    }
}