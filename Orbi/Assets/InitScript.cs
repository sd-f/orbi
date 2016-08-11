using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;

public class InitScript : MonoBehaviour
{
    public GameObject cubePrefab;

    void Awake()
    {
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        UdpateWorld(47.067700d, 15.555200d, 0.0d);
       
    }

    void Update()
    {
        // closing app
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void UdpateWorld(double latitude, double longitude, double elevation)
    {
        
        Debug.Log("UpdateWorld");
        if (GameObject.FindGameObjectWithTag("maps_container"))
        {
            GoogleMap map = GameObject.FindGameObjectWithTag("maps_container").GetComponent<GoogleMap>();
            if (map)
            {
                Debug.Log("update maps " + latitude);
                map.centerLocation.latitude = Convert.ToSingle(latitude);
                map.centerLocation.longitude = Convert.ToSingle(longitude);
                map.Refresh();
            }
        }
        
        Debug.Log("UpdateWorld " + latitude);

        string uri = "https://softwaredesign.foundation/orbi/api/world";
        uri = uri + "?";
        uri = uri + "latitude=" + latitude;
        uri = uri + "&";
        uri = uri + "longitude=" + longitude;
        uri = uri + "&user=test";
        //Debug.Log("debug = " + uri);
        Dictionary<string,string> headers = new Dictionary<string, string>();
        headers.Add("Accept", "application/json");
        WWW www = new WWW(uri, null, headers);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            JSONObject world = new JSONObject(www.text);
            //Debug.Log("world " + world);
            if (world.type == JSONObject.Type.OBJECT)
            {
                // remove old cubes
                GameObject[] oldCubes = GameObject.FindGameObjectsWithTag("world_cube");

                foreach (GameObject cube in oldCubes)
                {
                    GameObject.Destroy(cube);
                }
                JSONObject cubes = (JSONObject)world.list[0];
                foreach (JSONObject cube in cubes.list)
                {
                    //Debug.Log("adding cube " + cube);

                    GameObject newCube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    // Modify the clone to your heart's content
                    newCube.transform.parent = GameObject.FindGameObjectWithTag("cubes_container").transform;
                    newCube.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);
                    newCube.tag = "world_cube";
                    newCube.name = "cube_" + cube.list[1].n;
                    newCube.transform.position = new Vector3(cube.list[0].list[0].n, cube.list[0].list[1].n, cube.list[0].list[2].n);
                }
            }
            else
            {
                //Debug.Log("Sorry nothing here, start create your world!");
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

}
