using GameController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/InventoryObjects")]
    public class InventoryObjects : MonoBehaviour
    {
        private Vector3 targetPosition = new Vector3(-950f,0f,5f);

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
            this.targetPosition = new Vector3(-950f - x, this.targetPosition.y, this.targetPosition.z);
        }

    }
}
