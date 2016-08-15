using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.model;

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

    private GameObject mapsPlane;

    private MeshRenderer cubeToCraft;

    void Start () {

        mapsPlane = GameObject.Find("planeBackgroundMaps");

        buttonAddCube = GameObject.Find("buttonAddCube").GetComponent<Button>();
        buttonAddCubeOk = GameObject.Find("buttonAddCubeOk").GetComponent<Button>();
        buttonAddCubeCancel = GameObject.Find("buttonAddCubeCancel").GetComponent<Button>();

        buttonAddCube.transform.localScale = new Vector3(1f, 1f);
        buttonAddCubeOk.transform.localScale = new Vector3(0f, 0f);
        buttonAddCubeCancel.transform.localScale = new Vector3(0f, 0f);

        buttonCamera = GameObject.Find("buttonChangeBackgroundToCamera").GetComponent<Button>();
        buttonMaps = GameObject.Find("buttonChangeBackgroundToMaps").GetComponent<Button>();

        buttonMaps.interactable = !startInMapsView;
        buttonCamera.interactable = startInMapsView;

        bgCamera = GameObject.Find("planeBackgroundCamera").GetComponent<MeshRenderer>();
        bgMaps = GameObject.Find("planeBackgroundMaps").GetComponent<MeshRenderer>();

        bgCamera.enabled = !startInMapsView;
        bgMaps.enabled = startInMapsView;

        cubeToCraft = GameObject.Find("cubeToCraft").GetComponent<MeshRenderer>();
        cubeToCraft.enabled = false;
    }

	void Update () {
	
	}

    //FloatFilter magneticFilter = new AngleFilter(10);

    public void ReadCompass()
    {
        mapsPlane.transform.Rotate(new Vector3(mapsPlane.transform.rotation.x, mapsPlane.transform.rotation.y, 0)); // = Quaternion.Euler(, Sensor.GetOrientation().y, 0);
        //GUI.TextArea(Rect(10, 10, Screen.width - 10, Screen.height - 10), Sensor.GetOrientation().y);

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
            cubeToCraft.enabled = true;
            buttonAddCube.transform.localScale = new Vector3(0f, 0f);
            buttonAddCubeOk.transform.localScale = new Vector3(1f, 1f);
            buttonAddCubeCancel.transform.localScale = new Vector3(1f, 1f);
        } else
        {
            cubeToCraft.enabled = false;
            
            buttonAddCube.transform.localScale = new Vector3(1f, 1f);
            buttonAddCubeOk.transform.localScale = new Vector3(0f, 0f);
            buttonAddCubeCancel.transform.localScale = new Vector3(0f, 0f);
        }
       
        crafting = !crafting;
    }

    public void CraftCube()
    {
        World world = new World();
        GameObject craftContainer = GameObject.Find("gameObjectCubeToCraftContainer");
        VirtualGameObject objectToCraft = new VirtualGameObject();
        objectToCraft.name = "crafted";
        Position objectToCraftPosition = new Position();
        objectToCraftPosition.x = craftContainer.transform.position.x;
        objectToCraftPosition.y = craftContainer.transform.position.y;
        objectToCraftPosition.z = craftContainer.transform.position.z;
        objectToCraft.position = objectToCraftPosition;
        world.gameObjects.Add(objectToCraft);
        string uri = InitScript.serverUri + "/create";
        uri = uri + "?";
        uri = uri + "latitude=" + LocationScript.latitude;
        uri = uri + "&";
        uri = uri + "longitude=" + LocationScript.longitude;
        uri = uri + "&user=test";
        //Debug.Log("debug = " + uri);
        var encoding = new System.Text.UTF8Encoding();
        string jsonString = JsonUtility.ToJson(world);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Accept", "application/json");
        headers.Add("Content-Type", "application/json");
        headers.Add("Content-Length", encoding.GetByteCount(jsonString).ToString());
        WWW www = new WWW(uri, encoding.GetBytes(jsonString), headers);
        StartCoroutine(WaitForCubeSavedRequest(www));
    }

    IEnumerator WaitForCubeSavedRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            InitScript initScript = GameObject.FindGameObjectWithTag("cubes_container").GetComponent<InitScript>();
            initScript.ConstructWorld(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }


}
