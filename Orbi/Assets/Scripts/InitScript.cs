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
    public static string serverUri = "https://softwaredesign.foundation/orbi/api";
    //public static string serverUri = "http://localhost:8080/api";
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

            foreach (VirtualGameObject dummyGameObject in dummyWorld.gameObjects)
            {
                if (dummyGameObject.position.y < minZ)
                    minZ = dummyGameObject.position.y;
                if (dummyGameObject.position.y > maxZ)
                    maxZ = dummyGameObject.position.y;
            }
            //Debug.Log("min " + minZ + " max " + maxZ);
            float[,] heights = terrain.terrainData.GetHeights(0, 0, hmWidth, hmHeight);


            float height = 0.0f;
            height = (float)CorrectHeightOnTerrain(minZ);
            for (int i = 0; i < hmWidth; i++)
                for (int j = 0; j < hmHeight; j++)
                {
                    if (heights[i, j] == 0)
                    {
                        heights[i, j] = height;
                    }
                    //print(heights[i,j]);
                }
            int x, y;
            Texture2D texture = new Texture2D(16, 16);
            foreach (VirtualGameObject dummyGameObject in dummyWorld.gameObjects)
            {
                height = (float)CorrectHeightOnTerrain(dummyGameObject.position.y);
                y = (int)Math.Floor(dummyGameObject.position.x) + 64;
                x = (int)Math.Floor(dummyGameObject.position.z) + 64;

                //Debug.Log(x + "," + y + " h="+ height);
                heights[x, y] = height;
                texture.SetPixel(x/8, y/8,new Color(height, (float)((dummyGameObject.position.y - minZ) / (maxZ-minZ)), 0.5f));
                //Debug.Log(x/8 + "," + y/8 + " h=" + height);


            }

            // interpolation
            texture.Apply();
            Texture2D newTexuture = ScaleTexture(texture, 129, 129);
            //testplane.material.mainTexture = newTexuture;
            Color[] colors = newTexuture.GetPixels();
            
            for (x = 0; x < hmWidth; x++)
            {
                for (y = 0; y < hmHeight; y++)
                {
                    heights[x, y] = newTexuture.GetPixel(x,y).r;
                }
            }

            terrain.terrainData.SetHeights(0, 0, heights);
            terrain.Flush();

            //Debug.Log("terrain updated");
            UpdateGameObjects();
            UpdatePlayerElevation();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
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
