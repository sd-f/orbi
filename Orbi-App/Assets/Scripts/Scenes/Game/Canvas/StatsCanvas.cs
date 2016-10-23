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
        public Text level;
        public GameObject levelUpEffect;
        public GameObject levelUpContainer;

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
            if (Convert.ToInt64(level.text) > 0)
                if (Game.GetPlayer().GetModel().character.level > Convert.ToInt64(level.text))
                {
                    GameObject levelUp = GameObject.Instantiate(levelUpEffect) as GameObject;
                    levelUp.transform.SetParent(levelUpContainer.transform, false);
                }

            textXp.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.xp).ToString();
            textXr.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.xr).ToString();
            level.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.level).ToString();
            
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
      
    }

}
