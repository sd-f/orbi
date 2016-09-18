using GameController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/Camera")]
    public class CameraScript : MonoBehaviour
    {
        private Vector3 targetPosition = new Vector3(0f,2f,-7f);

        public void SetTargetPosition(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        void Update()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 2f);
        }

        public void MoveToPosition(float x)
        {
            this.targetPosition = new Vector3(x, this.targetPosition.y, this.targetPosition.z);
        }

    }
}
