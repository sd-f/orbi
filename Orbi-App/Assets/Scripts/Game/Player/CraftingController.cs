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

        public IEnumerator LoadInventory()
        {
            yield return Game.GetPlayer().GetPlayerService().RequestInventory();
        }

        public IEnumerator Craft(UnityEngine.GameObject gameObject)
        {
            ServerModel.GameObject newObject = new ServerModel.GameObject();
            newObject.id = -1;
            newObject.name = "new";
            newObject.prefab = Game.GetPlayer().GetCraftingController().GetSelectedPrefab();
            newObject.transform.rotation = new Rotation(gameObject.transform.rotation.eulerAngles);

            newObject.transform.geoPosition = new ClientModel.Position(gameObject.transform.position).ToGeoPosition();
            
            ServerModel.Player player = Game.GetPlayer().GetModel();
            player.gameObjectToCraft = newObject;
            yield return Game.GetPlayer().GetPlayerService().RequestCraft(player);
            yield return Game.GetWorld().UpdateObjects();
        }

    }
}
