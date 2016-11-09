using ServerModel;
using System;
using System.Collections;
using UnityEngine;

namespace GameController.Services
{

    public class InventoryService: AbstractHttpService
    {
        public static String ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB = "Cubes/Bricks";

        private Inventory inventory = new Inventory();
        private Inventory newInventory = new Inventory();
        private bool newItems = false;
        private bool checkForNew = false;
        private bool initialized = false;

        public IEnumerator RequestInventory()
        {
            yield return Request("player/inventory", null, OnInventoryRecieved);
        }

        private void OnInventoryRecieved(string data)
        {
            Inventory inventory = JsonUtility.FromJson<Inventory>(data);
            newItems = checkForNew && (inventory.items.Count > this.inventory.items.Count);
            
            SetInventory(inventory);
            initialized = true;
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
            foreach (InventoryItem item in inventory.items)
                if (item.type.prefab.Equals(ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB))
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

        public int GetNumberOfItems()
        {
            int items = 0;
            foreach (InventoryItem item in this.inventory.items)
                items += item.amount;
            return items;
        }

        public void SetInventory(Inventory inventory)
        {
            // select first item
            if (Game.Instance.GetPlayer().GetConstructionController().GetSelectedType() == null)
            {
                foreach (InventoryItem item in inventory.items)
                {
                    Game.Instance.GetPlayer().GetConstructionController().SetSelectedType(item.type);
                    break;
                }
            }
            if (initialized)
            {
                bool alreadythere = false;

                foreach (InventoryItem item in inventory.items)
                {
                    alreadythere = false;
                    foreach (InventoryItem oldItem in this.inventory.items)
                        if (oldItem.type.id.Equals(item.type.id))
                            alreadythere = true;
                    if (!alreadythere)
                        this.newInventory.items.Add(item);
                }

            }

            checkForNew = true;
            this.inventory = inventory;
        }
    }
}
