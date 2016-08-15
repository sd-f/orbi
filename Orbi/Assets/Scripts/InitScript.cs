using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using Assets.Scripts.model;
using Assets.Scripts.enums;

public class InitScript : MonoBehaviour
{
    //public static string serverUri = "https://softwaredesign.foundation/orbi/api";
    public static string serverUri = "http://localhost:8080/api";
    public GameObject cubePrefab;

    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    Camera mainCamera;
    Terrain terrain;

    void Awake()
    {
    }

    void Start()
    {
        
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        hmWidth = terrain.terrainData.heightmapWidth;
        hmHeight = terrain.terrainData.heightmapHeight;
        Terrain.activeTerrain.heightmapMaximumLOD = 0;
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
        
        UpdateGoogleMaps();
        UpdatePlayerElevation();
        UpdateGameObjects();
        UpdateTerrain();
    }

    public void UpdateTerrain()
    {
        WWW www = Request(Api.terrain);
        
        StartCoroutine(WaitForTerrainRequest(www));
    }

    IEnumerator WaitForTerrainRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            
            float[,] heights = terrain.terrainData.GetHeights(0, 0, hmWidth, hmHeight);
            for (int i = 0; i < hmWidth; i++)
                for (int j = 0; j < hmHeight; j++)
                {
                    heights[i, j] = 0;
                    //print(heights[i,j]);
                }
            World dummyWorld = JsonUtility.FromJson<World>(www.text);
            Debug.Log(hmWidth + "," + hmHeight);
            double maxX = 0.0d;
            double maxY = 0.0d;
            double maxZ = 0.0d;
            double minZ = 1000000d;
            foreach (VirtualGameObject dummyGameObject in dummyWorld.gameObjects)
            {
                if (dummyGameObject.position.x + 100 > maxX)
                {
                    maxX = dummyGameObject.position.x + 100;
                }
                if (dummyGameObject.position.z + 100 > maxY)
                {
                    maxY = dummyGameObject.position.x + 100;
                }
                if (dummyGameObject.position.y> maxZ)
                {
                    maxZ = dummyGameObject.position.y;
                }
                if (dummyGameObject.position.y < minZ)
                {
                    minZ = dummyGameObject.position.y;
                }
            }
            terrain.transform.position = new Vector3(-50, (float)minZ, -50);
            Debug.Log("max= " + maxX + "," + maxY);
            foreach (VirtualGameObject dummyGameObject in dummyWorld.gameObjects)
            {
                int xHeight = (int)(((Math.Round(dummyGameObject.position.x) + maxX) / (maxX * 2.0d)) * 32.0d);
                int yHeight = (int)(((Math.Round(dummyGameObject.position.z) + maxY) / (maxY * 2.0d)) * 32.0d);
                //Debug.Log(xHeight + "," + yHeight);
                if ((xHeight < 32 && xHeight >= 0) && (yHeight < 32 && yHeight >= 0))
                {
                    Debug.Log((float)dummyGameObject.position.y / maxZ);
                    heights[xHeight, yHeight] = (float) ((dummyGameObject.position.y - minZ) / (maxZ - minZ));
                }


            }
            terrain.terrainData.SetHeights(0, 0, heights);

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
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Position position = JsonUtility.FromJson<Position>(www.text);
            mainCamera.transform.Translate(new Vector3(0, (float) position.y, 0));

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void UpdateGameObjects()
    {
        WWW www = Request(Api.world);
        StartCoroutine(WaitForGameObjectsRequest(www));
    }

    IEnumerator WaitForGameObjectsRequest(WWW www)
    {
        yield return www;

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
            newCube.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);
            newCube.tag = "world_cube";
            newCube.name = "cube_" + gameObject.id + "_" + gameObject.name;
            newCube.transform.rotation = Quaternion.Euler(0.0001f, 0.00001f, 0.0f);
            newCube.transform.position = new Vector3((float) gameObject.position.x, (float) gameObject.position.y, (float) gameObject.position.z);
        }
        /*
        if (world.type == JSONObject.Type.OBJECT)
        {
            // remove old cubes
            
            JSONObject cubes = (JSONObject)world.list[0];
            foreach (JSONObject cube in cubes.list)
            {
                //Debug.Log("adding cube " + cube);

                

            }
        }
        else
        {
            //Debug.Log("Sorry nothing here, start create your world!");
        }
        */
    }

}
