using UnityEngine;
using GameController;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerBodyController")]
    class PlayerBodyController : MonoBehaviour
    {

        // handheld movement
        private Quaternion gyroRotation;
        private float deltaCompass = 0.0f;
        private bool gyroEnabled = false;
        private Vector3 targetPosition = new Vector3(0, 0, 0);
        public Camera cam;

        void Awake()
        {
            gyroEnabled = Game.GetGame().GetSettings().IsHandheldInputEnabled();
        }
        void Start()
        {
            SensorHelper.ActivateRotation();
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

        public void SetTargetPosition(Vector3 targetPosition)
        {
            this.targetPosition.x = targetPosition.x;
            this.targetPosition.z = targetPosition.z;
            this.targetPosition.y = this.transform.position.y;
        }

        public void UpdateDeltaCompass()
        {
            deltaCompass = gyroRotation.eulerAngles.y;
        }

        void ApplyGyroRotation()
        {
            gyroRotation = SensorHelper.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(transform.rotation.eulerAngles.x, gyroRotation.eulerAngles.y - deltaCompass, 0.0f)
            , Time.deltaTime * 10f);
            cam.transform.rotation = ClampRotationAroundXAxis(Quaternion.Slerp(cam.transform.rotation,
            Quaternion.Euler(gyroRotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, 0.0f)
            , Time.deltaTime * 10f));
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

    }
}