using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InitScript : MonoBehaviour
{

    void Awake()
    {
    }

    void Start()
    {
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
        // remove old cubes
        GameObject[] oldCubes = GameObject.FindGameObjectsWithTag("world_cube");
        
        foreach (GameObject cube in oldCubes)
        {
            GameObject.Destroy(cube);
        }
        Debug.Log("UpdateWorld");
        GoogleMap map = GameObject.FindGameObjectWithTag("maps_container").GetComponent<GoogleMap>();
        if (map)
        {
            Debug.Log("update maps " + latitude);
            map.centerLocation.latitude = Convert.ToSingle(latitude);
            map.centerLocation.longitude = Convert.ToSingle(longitude);
            map.Refresh();
        }
        Debug.Log("UpdateWorld " + latitude);

        string uri = "http://localhost:8080/api/world";
        uri = uri + "?";
        uri = uri + "latitude=" + latitude;
        uri = uri + "&";
        uri = uri + "longitude=" + longitude;
        uri = uri + "&user=test";
        //Debug.Log("debug = " + uri);
        HTTP.Request someRequest = new HTTP.Request("get", uri);
        someRequest.AddHeader("Accept", "application/json");
        someRequest.Send((request) => {
            // parse some JSON, for example:
            JSONObject world = new JSONObject(request.response.Text);
            if (world.type == JSONObject.Type.OBJECT)
            {
                JSONObject cubes = (JSONObject)world.list[0];
                foreach (JSONObject cube in cubes.list)
                {
                    GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newCube.transform.parent = GameObject.FindGameObjectWithTag("cubes_container").transform;
                    newCube.tag = "world_cube";
                    newCube.name = "cube_" + cube.list[1].n;
                    newCube.transform.position = new Vector3(cube.list[0].list[0].n, cube.list[0].list[1].n, cube.list[0].list[2].n);
                }
            }
            else
            {
                Debug.Log("Sorry nothing here, start create your world!");
            }

        });

        

    }


}
