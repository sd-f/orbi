using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    private Camera mainCamera;
    private Renderer cameraPlaneRenderer;
    private WebCamTexture cameraTexture;
    private Image imageButtonSwitchView;

    public Sprite cameraSprite;
    public Sprite mapsSprite;
    public LayerMask layersWithTerrain;
    public LayerMask layersWithoutTerrain;

    void Awake()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCamera.cullingMask = layersWithTerrain;
        cameraPlaneRenderer = GameObject.Find("CameraPlane").GetComponent<Renderer>();
        cameraTexture = new WebCamTexture();
        cameraPlaneRenderer.material.mainTexture = cameraTexture;
        imageButtonSwitchView = GameObject.Find("ImageButtonSwitchView").GetComponent<Image>();
        imageButtonSwitchView.sprite = cameraSprite;
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

    void OnDestroy()
    {
        if (cameraTexture.isPlaying)
        {
            cameraTexture.Stop();
        }
    }
}
