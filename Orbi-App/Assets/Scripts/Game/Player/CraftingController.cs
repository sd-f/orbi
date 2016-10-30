using GameScene;
using ServerModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameController
{
    public class CraftingController
    {
        private bool collission = false;
        private GameObjectType selectedType;
        private string userText = "";
        private UnityEngine.GameObject objectToCraft;
        private bool crafting = false;
        private Inventory inventory = new Inventory();
        public List<string> itemsDiscovered { get; set; }

        public CraftingController()
        {
            itemsDiscovered = null;
        }

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

        public GameObjectType GetSelectedType()
        {
            return this.selectedType;
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

        internal InventoryItem GetInventoryItem(string prefab)
        {
            foreach(InventoryItem item in inventory.items)
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

        public void SetSelectedType(GameObjectType type)
        {
            this.selectedType = type;
        }

        public Inventory GetInventory()
        {
            return this.inventory;
        }

        public void SetInventory(Inventory inventory)
        {
            // select first item
            if (GetSelectedType() == null)
            {
                foreach (InventoryItem item in inventory.items)
                {
                    SetSelectedType(item.type);
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

        public string GetUserText()
        {
            return this.userText;
        }

        public void SetUserText(string text)
        {
            if ((text != null) && (text.Length > 16))
                this.userText = text.Substring(0, 15);
            else
                this.userText = text;
        }

        public IEnumerator LoadInventory()
        {
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestInventory();
        }

        public IEnumerator Craft(UnityEngine.GameObject gameObject)
        {
            ServerModel.GameObject newObject = new ServerModel.GameObject();
            newObject.id = -1;
            newObject.name = "new";
            newObject.type = Game.Instance.GetPlayer().GetCraftingController().GetSelectedType();
            if (Game.Instance.GetPlayer().GetCraftingController().GetSelectedType().supportsUserText)
                newObject.userText = Game.Instance.GetPlayer().GetCraftingController().GetUserText();
            newObject.transform.rotation = new Rotation(gameObject.transform.rotation.eulerAngles);
            newObject.transform.geoPosition = new ClientModel.Position(gameObject.transform.position).ToGeoPosition();
            newObject.constraints = (int)RigidbodyConstraints.FreezeAll;
            ServerModel.Player player = Game.Instance.GetPlayer().GetModel();
            player.gameObjectToCraft = newObject;
            yield return Game.Instance.GetPlayer().GetPlayerService().RequestCraft(player);
            yield return LoadInventory();
            yield return Game.Instance.GetWorld().UpdateObjects();
        }

    }
}
