﻿using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;
using GameScene;

public class InventoryItemScript : MonoBehaviour {

    ServerModel.InventoryItem item;
    private bool newIndicator = false;

    void Start()
    {
        if (item != null)
        {
            if (newIndicator)
                transform.Find("NewIndicator").gameObject.SetActive(true);
            GameObject label = transform.Find("LabelBackground").gameObject;
            label.SetActive(true);
            GameObject gameObjectContainer = transform.Find("InventoryItemGameObject").gameObject;
            if (Game.Instance.GetPlayer().GetCraftingController().GetSelectedType().id == item.type.id)
                gameObjectContainer.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0.5f);
            gameObjectContainer.transform.Find("UnknownText").gameObject.SetActive(false);
            transform.Find("LabelBackground").GetComponent<Image>().color = GetItemColor(item.type);
            label.transform.Find("AmountText").GetComponent<Text>().text = "x " + item.amount;
            gameObjectContainer.GetComponent<Button>().onClick.AddListener(() => { OnSelected(); });

            GameObject newObject = GameObjectFactory.CreateObject(gameObjectContainer.transform, item.type.prefab, item.type.id, null, LayerMask.NameToLayer("Inventory"));
            
            newObject.transform.localRotation = Quaternion.Euler(-5f, 25f, 0f);
            newObject.transform.localPosition = new Vector3(0f,15f, -100f);
            GameObjectUtility.DisableAI(newObject);
            GameObjectUtility.NormalizeScale(newObject);
           // GameObject realObject = GameObjectFactory.GetObject(newObject);
            newObject.transform.localScale = newObject.transform.localScale * 0.75f;
        }
    }

    public void OnSelected()
    {
        Game.Instance.GetPlayer().GetCraftingController().SetSelectedType(item.type);
        GameObject.Find("Canvas").GetComponent<GameScene.Canvas>().CloseSettings();
    }


    private Color GetItemColor(ServerModel.GameObjectType type)
    {
        int rarity = type.rarity;
        if (rarity == 0)
            return Color.white;
        else if (rarity == 1)
            return Color.gray;
        else if (rarity == 2)
            return Color.green;
        else if (rarity == 3)
            return Color.blue;
        else if (rarity == 4)
            return new Color(1f, 0.843F, 0); // gold
        return Color.white;
    }

    public void SetNewIndicator(bool newIndicator)
    {
        this.newIndicator = newIndicator;
    }

	public void SetItem(ServerModel.InventoryItem item)
    {
        this.item = item;
    }

    internal void SetOffset(float offsetX, float offsetY)
    {
        throw new NotImplementedException();
    }
}
