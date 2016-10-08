using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/CraftingCollision")]
    class CraftingCollision : MonoBehaviour
    {

        public int collisions = 0;

        void Update()
        {
        }

        void OnCollisionEnter(Collision col)
        {
            collisions++;
            Game.GetPlayer().GetCraftingController().SetColliding(collisions > 0);
        }

        void OnCollisionExit(Collision col)
        {
            collisions--;
            Game.GetPlayer().GetCraftingController().SetColliding(collisions > 0);
        }

    }
}
