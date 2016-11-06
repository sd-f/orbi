using ServerModel;
using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class InventoryService: AbstractHttpService
    {

        private Inventory inventory = new Inventory();
        private bool newItems = false;
        private bool checkForNew = false;

        public IEnumerator RequestInventory()
        {
            yield return Request("player/inventory", null, OnInventoryRecieved);
        }

        private void OnInventoryRecieved(string data)
        {
            Inventory inventory = JsonUtility.FromJson<Inventory>(data);
            newItems = checkForNew && (inventory.items.Count > this.inventory.items.Count);
            SetInventory(inventory);
            IndicateRequestFinished();
        }

        public bool HasNewItems()
        {
            return this.newItems;
        }

        public void ResetNew()
        {
            this.newItems = false;
        }

        internal InventoryItem GetInventoryItem(string prefab)
        {
            foreach (InventoryItem item in inventory.items)
                if (item.type.prefab.Equals(prefab))
                    return item;
            return null;
        }

        internal InventoryItem GetNextAvailableItem()
        {
            foreach (InventoryItem item in inventory.items)
                if (item.amount > 0)
                    return item;
            return null;
        }

        public Inventory GetInventory()
        {
            return this.inventory;
        }

        internal bool HasInventoryItem(string prefab)
        {
            InventoryItem itemToRemove = GetInventoryItem(prefab);
            return ((itemToRemove != null) && (itemToRemove.amount > 0));
        }

        internal void RemoveInventoryItem(string prefab)
        {
            InventoryItem itemToRemove = GetInventoryItem(prefab);
            if ((itemToRemove != null) && (itemToRemove.amount > 0))
                itemToRemove.amount--;
        }

        public void SetInventory(Inventory inventory)
        {
            // select first item
            if (Game.Instance.GetPlayer().GetCraftingController().GetSelectedType() == null)
            {
                foreach (InventoryItem item in inventory.items)
                {
                    Game.Instance.GetPlayer().GetCraftingController().SetSelectedType(item.type);
                    break;
                }
            }
            checkForNew = true;
            this.inventory = inventory;
        }
    }
}
