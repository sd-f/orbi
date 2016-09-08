using UnityEngine;
using System.Collections;
using GameController;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerBodyController")]
    class PlayerBodyController : MonoBehaviour
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
            {
                ApplyGyroRotation();
            }
                
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

    }
}