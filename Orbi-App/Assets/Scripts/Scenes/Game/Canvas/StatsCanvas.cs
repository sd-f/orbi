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
        public Text statsInfoText;
        public GameObject statsInfo;
        public Text newItemsInfoText;
        public GameObject newItemsInfo;
        public GameObject levelUpEffect;
        public GameObject levelUpContainer;
        private bool statsInfoFading = false;
        private bool newItemsInfoFading = false;


        private long inventoryItems = 0;

        void Start()
        {
            UpdateStatsLabels();
            Invoke("UpdateStats", 5f);
            
        }

        void UpdateStats()
        {
            StartCoroutine(PlayerUpdate());
        }

        void StartFadingStatsInfo()
        {
            statsInfoFading = true;
        }

        void StartFadingNewItemsInfo()
        {
            newItemsInfoFading = true;
        }

        void Update()
        {
            if (statsInfoFading)
            {
                Color color = statsInfoText.color;
                color.a = color.a - 0.02f;
                statsInfoText.color = color;
                if (color.a < 0.1)
                {
                    statsInfoFading = false;
                    statsInfo.SetActive(false);
                }
            }

            if (newItemsInfoFading)
            {
                Color color = newItemsInfoText.color;
                color.a = color.a - 0.02f;
                newItemsInfoText.color = color;
                if (color.a < 0.1)
                {
                    newItemsInfoFading = false;
                    newItemsInfo.SetActive(false);
                }
            }

        }

        private IEnumerator PlayerUpdate()
        {
            yield return Game.GetPlayer().GetPlayerService().RequestStatsUpdate();
            yield return Game.GetPlayer().GetPlayerService().RequestInventory();
            UpdateStatsLabels();
            Invoke("UpdateStats", 5f);
        }

        void UpdateStatsLabels()
        {
            long oldLevel = Convert.ToInt64(level.text);
            long newLevel = Game.GetPlayer().GetModel().character.level;
            if ((oldLevel > 0) && (newLevel > oldLevel))
            { 
                GameObject levelUp = GameObject.Instantiate(levelUpEffect) as GameObject;
                levelUp.transform.SetParent(levelUpContainer.transform, false);
            }
            long oldXP = Convert.ToInt64(textXp.text);
            long newXP = Convert.ToInt64(Game.GetPlayer().GetModel().character.xp);
            if ((oldXP > 0) && (newXP > oldXP))
                AddStatsInfo("+ " + (newXP - Convert.ToInt64(textXp.text)) + " xp");
            long newNumberOfInventoryItems = GetNumberOfInventoryItems();
            if (inventoryItems > 0)
                if (inventoryItems < newNumberOfInventoryItems)
                    AddNewItemsInfo("+ " + (newNumberOfInventoryItems - inventoryItems) + " items");
            textXp.text = newXP.ToString();
            textXr.text = Convert.ToInt64(Game.GetPlayer().GetModel().character.xr).ToString();
            level.text = newLevel.ToString();
            inventoryItems = newNumberOfInventoryItems;
        }

        private void AddStatsInfo(string text)
        {
            statsInfo.SetActive(true);
            statsInfoFading = false;
            statsInfoText.text = text;
            statsInfoText.color = Color.red;
            Invoke("StartFadingStatsInfo", 2f);
        }

        private void AddNewItemsInfo(string text)
        {
            newItemsInfo.SetActive(true);
            newItemsInfoFading = false;
            newItemsInfoText.text = text;
            newItemsInfoText.color = Color.red;
            Invoke("StartFadingNewItemsInfo", 2f);
        }

        private long GetNumberOfInventoryItems()
        {
            long itemsCount = 0;
            foreach(ServerModel.InventoryItem item in Game.GetPlayer().GetCraftingController().GetInventory().items)
            {
                itemsCount += item.amount;
            }
            return itemsCount;
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
      
    }

}
