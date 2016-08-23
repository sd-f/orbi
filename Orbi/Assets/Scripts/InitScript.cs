using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using Assets.Scripts.model;
using Assets.Scripts.enums;
using UnityEngine.UI;

public class InitScript : MonoBehaviour
{
    //public static string serverUri = "https://softwaredesign.foundation/orbi/api";
    public static string serverUri = "http://localhost:8080/api";
    public GameObject cubePrefab;

    static int hmWidth; // heightmap width
    static int hmHeight; // heightmap height
    static float terrainHeight = 128.0f;

    public static double maxZ = 0d;
    public static double minZ = 100000d;
    Image refreshImage;
    int requestsRunning = 0;

    Camera mainCamera;
    Terrain terrain;

    void Awake()
    {
    }

    void Start()
    {
        refreshImage = GameObject.Find("imageRefresh").GetComponent<Image>();
        terrain = GameObject.Find("MapsTerrain").GetComponent<Terrain>();
        
        hmWidth = terrain.terrainData.heightmapWidth;
        hmHeight = terrain.terrainData.heightmapHeight;
        
        //Debug.Log("heightmapScale=" + terrain.terrainData.size);
        //Debug.Log("heightmap[" + hmHeight + "," + hmWidth + "]");
        //Terrain.activeTerrain.heightmapMaximumLOD = 0;
        UpdateWorld();
        mainCamera = GameObject.Find("cameraMain").GetComponent<Camera>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        // closing app
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public static WWW Request(Api api)
    {
        double latitude = LocationScript.latitude;
        double longitude = LocationScript.longitude;
        string uri = serverUri + "/"+ api.ToString();
        uri = uri + "?latitude=" + latitude;
        uri = uri + "&longitude=" + longitude;
        //Debug.Log("debug = " + uri);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Accept", "application/json");
        return new WWW(uri, null, headers);
    }

    public void UpdateGoogleMaps()
    {
        if (GameObject.FindGameObjectWithTag("maps_container"))
        {
            GoogleMap map = GameObject.FindGameObjectWithTag("maps_container").GetComponent<GoogleMap>();
            if (map)
            {
                map.centerLocation.latitude = Convert.ToSingle(LocationScript.latitude);
                map.centerLocation.longitude = Convert.ToSingle(LocationScript.longitude);
                map.Refresh();
            }
        }
    }

   

    public void UpdateWorld()
    {
        UpdateTerrain();
        UpdateGoogleMaps();
        
    }

    public void UpdateTerrain()
    {
        WWW www = Request(Api.terrain);
        
        StartCoroutine(WaitForTerrainRequest(www));
    }

    public static float CorrectHeightOnTerrain(double height)
    {
        float correctedHeight = (float)((height) / (minZ + terrainHeight));
        if (((height) / (minZ + terrainHeight)) > 1)
            return 1;
        return correctedHeight;
    }

    public static float CorrectHeightInScene(double height)
    {
        float correctedHeight = (float)((height) / (minZ + terrainHeight));
        //Debug.Log("minZ=" + minZ + " height= " + (height - minZ) + " -> " + correctedHeight);
        if (((height) / (minZ + terrainHeight)) > 1)
            return terrainHeight;
        return correctedHeight * terrainHeight;
    }

    IEnumerator WaitForTerrainRequest(WWW www)
    {
        RequestIndicatorRequestStarted();
        yield return www;
        RequestIndicatorRequestFinished();

        // check for errors
        if (www.error == null)
        {
            World dummyWorld = JsonUtility.FromJson<World>(www.text);

            
            //Debug.Log("terrain updated");
            UpdateGameObjects();
            UpdatePlayerElevation();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    

    public void UpdatePlayerElevation()
    {
        WWW www = Request(Api.elevation);
        StartCoroutine(WaitForElevationRequest(www));
    }

    IEnumerator WaitForElevationRequest(WWW www)
    {

        RequestIndicatorRequestStarted();
        yield return www;
        RequestIndicatorRequestFinished();

        // check for errors
        if (www.error == null)
        {
            Position position = JsonUtility.FromJson<Position>(www.text);
           // Debug.Log(position + " corrected= " + CorrectHeightInScene(position.y));
            mainCamera.transform.position = new Vector3(0, CorrectHeightInScene( position.y + 8.0f), 0);

        }
        else
        {
            requestsRunning--;
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void UpdateGameObjects()
    {
        WWW www = Request(Api.world);
        Debug.Log(www.url);
        StartCoroutine(WaitForGameObjectsRequest(www));
    }

    IEnumerator WaitForGameObjectsRequest(WWW www)
    {
        RequestIndicatorRequestStarted();
        yield return www;
        RequestIndicatorRequestFinished();

        // check for errors
        if (www.error == null)
        {
            
            ConstructGameObjects(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }


    
    
    public void ConstructGameObjects(String worldString)
    {
        World world = JsonUtility.FromJson<World>(worldString);

        GameObject[] oldCubes = GameObject.FindGameObjectsWithTag("world_cube");

        foreach (GameObject cube in oldCubes)
        {
            GameObject.Destroy(cube);
        }
       

        foreach (VirtualGameObject gameObject in world.gameObjects)
        {
            GameObject newCube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            // Modify the clone to your heart's content
            newCube.transform.parent = GameObject.FindGameObjectWithTag("cubes_container").transform;
            newCube.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
            newCube.tag = "world_cube";
            newCube.name = "cube_" + gameObject.id + "_" + gameObject.name;
            newCube.transform.rotation = Quaternion.Euler(0.0001f, 0.00001f, 0.0f);
            newCube.transform.position = new Vector3((float) gameObject.position.x, CorrectHeightInScene( gameObject.position.y ), (float) gameObject.position.z);
           
        }
    }

    private void RequestIndicatorRequestStarted()
    {
        //Debug.Log("started running=" + requestsRunning);
        requestsRunning++;
        refreshImage.enabled = true;
    }

    private void RequestIndicatorRequestFinished()
    {
        //Debug.Log("finished running=" + requestsRunning);
        requestsRunning--;
        if (requestsRunning < 0)
        {
            requestsRunning = 0;
        }
        if (requestsRunning == 0)
            refreshImage.enabled = false;
    }


}
