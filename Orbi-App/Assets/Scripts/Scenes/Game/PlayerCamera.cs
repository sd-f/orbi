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

        public GameObject frontOfCamera;

        void Start()
        {
            SensorHelper.ActivateRotation();
        }

        void Update()
        {
            if (Game.GetGame().GetSettings().IsDesktopInputEnabled())
            {
                // fps controller
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

        Vector3 MoveToPositionWithHeight(Vector3 moveToPosition)
        {
            Vector3 newMoveToPosition = moveToPosition;
            newMoveToPosition.y = GetHeight(moveToPosition.x, moveToPosition.z) + Player.HEIGHT;
            return newMoveToPosition;
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

        public void AdjustHeight()
        {
            
            transform.position = MoveToPositionWithHeight(transform.position);
            
        }

        public void MoveToPosition(Vector3 moveToPosition)
        {
            transform.position = MoveToPositionWithHeight(moveToPosition);
            
        }

        public void ResetPosition()
        {
            MoveToPosition(new Vector3(0, Player.HEIGHT, 0));
        }

    }
}