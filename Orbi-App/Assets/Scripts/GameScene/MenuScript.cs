﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    private Camera mainCamera;
    private Renderer cameraPlaneRenderer;
    private WebCamTexture cameraTexture;
    private Image imageButtonSwitchView;

    private bool crafting = false;

    public Sprite cameraSprite;
    public Sprite mapsSprite;
    public LayerMask layersWithTerrain;
    public LayerMask layersWithoutTerrain;

    private GameObject buttonCraft;
    private GameObject imageButtonCraft;

    private GameObject buttonCraftOk;
    private GameObject imageButtonCraftOk;

    private GameObject buttonCraftCancel;
    private GameObject imageButtonCraftCancel;

    private GameObject buttonDelete;
    private GameObject imageButtonDelete;

    private GameObject buttonDeleteOk;
    private GameObject imageButtonDeleteOk;

    private GameObject buttonDeleteCancel;
    private GameObject imageButtonDeleteCancel;

    void Awake()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCamera.cullingMask = layersWithTerrain;
        cameraPlaneRenderer = GameObject.Find("CameraPlane").GetComponent<Renderer>();
        cameraTexture = new WebCamTexture();
        cameraPlaneRenderer.material.mainTexture = cameraTexture;
        imageButtonSwitchView = GameObject.Find("ImageButtonSwitchView").GetComponent<Image>();
        imageButtonSwitchView.sprite = cameraSprite;

        buttonCraft = GameObject.Find("ButtonCraft");
        imageButtonCraft = GameObject.Find("ImageButtonCraft");
        buttonCraftOk = GameObject.Find("ButtonCraftOk");
        imageButtonCraftOk = GameObject.Find("ImageButtonCraftOk");
        buttonCraftCancel = GameObject.Find("ButtonCraftCancel");
        imageButtonCraftCancel = GameObject.Find("ImageButtonCraftCancel");

        buttonDelete = GameObject.Find("ButtonDelete");
        imageButtonDelete = GameObject.Find("ImageButtonDelete");
        buttonDeleteOk = GameObject.Find("ButtonDeleteOk");
        imageButtonDeleteOk = GameObject.Find("ImageButtonDeleteOk");
        buttonDeleteCancel = GameObject.Find("ButtonDeleteCancel");
        imageButtonDeleteCancel = GameObject.Find("ImageButtonDeleteCancel");
    }

    // camera, terrain switch
	public void OnSwitchView()
    {
        if (mainCamera.cullingMask == layersWithTerrain)
        {
            imageButtonSwitchView.sprite = mapsSprite;
            cameraTexture.Play();
            mainCamera.cullingMask = layersWithoutTerrain;
            mainCamera.clearFlags = CameraClearFlags.Depth;
        } else
        {
            imageButtonSwitchView.sprite = cameraSprite;
            cameraTexture.Stop();
            mainCamera.cullingMask = layersWithTerrain;
            mainCamera.clearFlags = CameraClearFlags.Skybox;
        }
    }

    public void OnCraftingCancel()
    {
        this.crafting = false;
        buttonCraft.GetComponent<Button>().interactable = true;
        buttonCraft.GetComponent<Image>().enabled = true;
        imageButtonCraft.GetComponent<Image>().enabled = true;

        buttonCraftOk.GetComponent<Button>().interactable = false;
        buttonCraftOk.GetComponent<Image>().enabled = false;
        imageButtonCraftOk.GetComponent<Image>().enabled = false;

        buttonCraftCancel.GetComponent<Button>().interactable = false;
        buttonCraftCancel.GetComponent<Image>().enabled = false;
        imageButtonCraftCancel.GetComponent<Image>().enabled = false;

        // enable delete
        buttonDelete.GetComponent<Button>().interactable = true;
    }

    public void OnCraftingStart()
    {
        this.crafting = true;
        buttonCraft.GetComponent<Button>().interactable = false;
        buttonCraft.GetComponent<Image>().enabled = false;
        imageButtonCraft.GetComponent<Image>().enabled = false;

        buttonCraftOk.GetComponent<Button>().interactable = true;
        buttonCraftOk.GetComponent<Image>().enabled = true;
        imageButtonCraftOk.GetComponent<Image>().enabled = true;

        buttonCraftCancel.GetComponent<Button>().interactable = true;
        buttonCraftCancel.GetComponent<Image>().enabled = true;
        imageButtonCraftCancel.GetComponent<Image>().enabled = true;

        // disable delete
        buttonDelete.GetComponent<Button>().interactable = false;
    }

    public void OnDestroyingCancel()
    {

        buttonDelete.GetComponent<Button>().interactable = true;
        buttonDelete.GetComponent<Image>().enabled = true;
        imageButtonDelete.GetComponent<Image>().enabled = true;

        buttonDeleteOk.GetComponent<Button>().interactable = false;
        buttonDeleteOk.GetComponent<Image>().enabled = false;
        imageButtonDeleteOk.GetComponent<Image>().enabled = false;

        buttonDeleteCancel.GetComponent<Button>().interactable = false;
        buttonDeleteCancel.GetComponent<Image>().enabled = false;
        imageButtonDeleteCancel.GetComponent<Image>().enabled = false;

        // enable craft
        buttonCraft.GetComponent<Button>().interactable = true;
    }

    public void OnDestroyingStart()
    {
        buttonDelete.GetComponent<Button>().interactable = false;
        buttonDelete.GetComponent<Image>().enabled = false;
        imageButtonDelete.GetComponent<Image>().enabled = false;

        buttonDeleteOk.GetComponent<Button>().interactable = true;
        buttonDeleteOk.GetComponent<Image>().enabled = true;
        imageButtonDeleteOk.GetComponent<Image>().enabled = true;

        buttonDeleteCancel.GetComponent<Button>().interactable = true;
        buttonDeleteCancel.GetComponent<Image>().enabled = true;
        imageButtonDeleteCancel.GetComponent<Image>().enabled = true;

        // enable craft
        buttonCraft.GetComponent<Button>().interactable = true;
    }

    public bool IsCrafting()
    {
        return this.crafting;
    }

    void OnDestroy()
    {
        if (cameraTexture.isPlaying)
        {
            cameraTexture.Stop();
        }
    }

}