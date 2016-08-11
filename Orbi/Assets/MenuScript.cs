using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public bool startInMapsView = true;
    private bool crafting = false;

    private Button buttonCamera;
    private Button buttonMaps;
    private MeshRenderer bgCamera;
    private MeshRenderer bgMaps;

    private Button buttonAddCube;
    private Button buttonAddCubeOk;
    private Button buttonAddCubeCancel;

    void Start () {

        buttonAddCube = GameObject.Find("buttonAddCube").GetComponent<Button>();
        buttonAddCubeOk = GameObject.Find("buttonAddCubeOk").GetComponent<Button>();
        buttonAddCubeCancel = GameObject.Find("buttonAddCubeCancel").GetComponent<Button>();

        buttonAddCube.transform.localScale = new Vector3(1f, 1f);
        buttonAddCubeOk.transform.localScale = new Vector3(0f, 0f);
        buttonAddCubeCancel.transform.localScale = new Vector3(0f, 0f);

        buttonCamera = GameObject.Find("buttonChangeBackgroundToCamera").GetComponent<Button>();
        buttonMaps = GameObject.Find("buttonChangeBackgroundToMaps").GetComponent<Button>();

        buttonMaps.interactable = startInMapsView;
        buttonCamera.interactable = !startInMapsView;

        bgCamera = GameObject.Find("planeBackgroundCamera").GetComponent<MeshRenderer>();
        bgMaps = GameObject.Find("planeBackgroundMaps").GetComponent<MeshRenderer>();

        bgCamera.enabled = startInMapsView;
        bgMaps.enabled = !startInMapsView;
    }

	void Update () {
	
	}

    public void ToggleMap()
    {
        buttonCamera.interactable = !buttonCamera.interactable;
        buttonMaps.interactable = !buttonMaps.interactable;
        bgMaps.enabled = !bgMaps.enabled;
        bgCamera.enabled = !bgCamera.enabled;
    }

    public void ToggleCrafting()
    {
        if (!crafting)
        {
            buttonAddCube.transform.localScale = new Vector3(0f, 0f);
            buttonAddCubeOk.transform.localScale = new Vector3(1f, 1f);
            buttonAddCubeCancel.transform.localScale = new Vector3(1f, 1f);
        } else
        {
            buttonAddCube.transform.localScale = new Vector3(1f, 1f);
            buttonAddCubeOk.transform.localScale = new Vector3(0f, 0f);
            buttonAddCubeCancel.transform.localScale = new Vector3(0f, 0f);
        }
       
        crafting = !crafting;
    }
}
