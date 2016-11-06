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

        public CraftingController()
        {
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

        

        public void SetSelectedType(GameObjectType type)
        {
            this.selectedType = type;
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
            yield return Game.Instance.GetPlayer().GetInventoryService().RequestInventory();
            yield return Game.Instance.GetWorld().UpdateObjects();
        }

    }
}
