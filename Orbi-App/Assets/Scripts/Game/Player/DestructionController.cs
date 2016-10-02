using System.Collections;
using UnityEngine;

namespace GameController
{
    class DestructionController
    {
        public IEnumerator Destroy(long id)
        {
            // effect
            ServerModel.Player player = Game.GetPlayer().GetModel();
            player.selectedObjectId = id;
            yield return Game.GetPlayer().GetPlayerService().RequestDestroy(player);
            yield return Game.GetWorld().UpdateObjects();
        }
    }
}
