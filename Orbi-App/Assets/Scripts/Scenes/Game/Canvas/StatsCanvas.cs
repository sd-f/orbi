using GameController;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace GameScene
{

    [AddComponentMenu("App/Scenes/Game/Canvas/StatsCanvas")]
    public class StatsCanvas : MonoBehaviour
    {
        public Text textXp;
        public Text textXr;

        void Start()
        {
            UpdateStatsLabels();
            Invoke("UpdateStats", 5f);
        }

        void UpdateStats()
        {
            StartCoroutine(PlayerUpdate());
        }

        private IEnumerator PlayerUpdate()
        {
            yield return Game.GetPlayer().GetPlayerService().RequestUpdate();
            UpdateStatsLabels();
            Invoke("UpdateStats", 5f);
        }

        void UpdateStatsLabels()
        {
            textXp.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.xp.ToString()).ToString();
            textXr.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.xr.ToString()).ToString();
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
      
    }

}
