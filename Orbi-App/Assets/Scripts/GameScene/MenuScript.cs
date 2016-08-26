using UnityEngine;
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

    public void OnCraftingStart()
    {
        this.crafting = true;
        buttonCraft.GetComponent<Image>().enabled = false;
        imageButtonCraft.GetComponent<Image>().enabled = false;
        buttonCraftOk.GetComponent<Image>().enabled = true;
        imageButtonCraftOk.GetComponent<Image>().enabled = true;
        buttonCraftCancel.GetComponent<Image>().enabled = true;
        imageButtonCraftCancel.GetComponent<Image>().enabled = true;
    }

    public void OnCraftingCancel()
    {
        this.crafting = false;
        buttonCraft.GetComponent<Image>().enabled = true;
        imageButtonCraft.GetComponent<Image>().enabled = true;
        buttonCraftOk.GetComponent<Image>().enabled = false;
        imageButtonCraftOk.GetComponent<Image>().enabled = false;
        buttonCraftCancel.GetComponent<Image>().enabled = false;
        imageButtonCraftCancel.GetComponent<Image>().enabled = false;
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
