using System.Collections;
using UnityEngine;

namespace GameController
{
    class DestructionController
    {
        public IEnumerator Destroy(long id)
        {
            // effect
            ServerModel.Player player = Game.Instance.GetPlayer().GetModel();
            player.selectedObjectId = id;
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestDestroy(player);
            yield return Game.Instance.GetWorld().UpdateObjects();
        }
    }
}
