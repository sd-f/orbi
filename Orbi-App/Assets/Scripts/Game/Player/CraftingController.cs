using GameScene;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameController
{
    class CraftingController
    {
        private bool collission = false;
        private string selectedPrefab;
        private UnityEngine.GameObject objectToCraft;
        private bool crafting = false;
        private Inventory inventory = new Inventory();

        public void SetCrafting(Boolean crafting, UnityEngine.GameObject objectToCraft)
        {
            this.objectToCraft = objectToCraft;
            this.crafting = crafting;
        }

        public Boolean IsCrafting()
        {
            return this.crafting;
        }

        public UnityEngine.GameObject GetObjectToCraft()
        {
            return this.objectToCraft;
        }

        public String GetSelectedPrefab()
        {
            return this.selectedPrefab;
        }

        public void SetSelectedPrefab(String selectedPrefab)
        {
            this.selectedPrefab = selectedPrefab;
        }

        public Inventory GetInventory()
        {
            return this.inventory;
        }

        public void SetInventory(Inventory inventory)
        {
            // select first item
            if (GetSelectedPrefab() == null)
            {
                foreach (InventoryItem item in inventory.items)
                {
                    SetSelectedPrefab(item.prefab);
                    break;
                }
            }

            this.inventory = inventory;
        }

        public void SetColliding(bool collission)
        {
            this.collission = collission;
        }

        public bool IsColliding()
        {
            return this.collission;
        }

    }
}
