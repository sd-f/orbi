using UnityEngine;
using System.Collections;
using GameScene;

namespace GameScene
{
    public class CraftingCollision : MonoBehaviour
    {

        private ConstructionController controller;
        private int numberOfCollisions = 0;

        public void SetController(ConstructionController controller)
        {
            this.controller = controller;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (controller != null)
            {
                numberOfCollisions++;
                controller.SetColliding(true);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (controller != null)
            {
                numberOfCollisions--;
                if (numberOfCollisions <= 0)
                    controller.SetColliding(false);
            }
        }
    }
}