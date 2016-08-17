using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;
	
	void Start() {
		if(loadOnStart) Refresh();	
	}
	
	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}
		_Refresh();
	}
	
	void _Refresh ()
	{
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + WWW.EscapeURL(centerLocation.address);
			else {
				qs += "center=" + WWW.EscapeURL(string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude));
			}
		
			qs += "&zoom=" + zoom.ToString ();
		}
		qs += "&size=" + WWW.EscapeURL(string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");
		
		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.EscapeURL(loc.address);
				else
					qs += "|" + WWW.EscapeURL(string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.EscapeURL(loc.address);
				else
					qs += "|" + WWW.EscapeURL(string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}

        WWW www = new WWW(url + "?" + qs);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            var tex = new Texture2D(size, size);
            tex.LoadImage(www.bytes);
            //GetComponent<Renderer>().material.mainTexture = tex;
           
            Terrain terrain = GameObject.Find("MapsTerrain").GetComponent<Terrain>();

            SplatPrototype[] sp = terrain.terrainData.splatPrototypes;

            //Debug.Log(tex.width);
            // TODO rotation fix for image -90°
            /*
            Color32[] pixels = tex.GetPixels32();
            pixels = RotateMatrix(pixels, tex.width);
            tex.SetPixels32(pixels);
            tex.Apply();
            pixels = RotateMatrix(pixels, tex.width);
            tex.SetPixels32(pixels);
            tex.Apply();
            pixels = RotateMatrix(pixels, tex.width);
            tex.SetPixels32(pixels);
            tex.Apply();
            */
            sp[0].texture = tex;
            terrain.terrainData.splatPrototypes = sp;

            terrain.Flush();
            terrain.terrainData.RefreshPrototypes();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }


   
    static Color32[] RotateMatrix(Color32[] matrix, int n)
    {
        Color32[] ret = new Color32[n * n];

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                ret[i * n + j] = matrix[(n - j - 1) * n + i];
            }
        }

        return ret;
    }
    
}

/* maps scales
20 : 1128.497220
19 : 2256.994440
18 : 4513.988880
17 : 9027.977761
16 : 18055.955520
15 : 36111.911040
14 : 72223.822090
13 : 144447.644200
12 : 288895.288400
11 : 577790.576700
10 : 1155581.153000
9  : 2311162.307000
8  : 4622324.614000
7  : 9244649.227000
6  : 18489298.450000
5  : 36978596.910000
4  : 73957193.820000
3  : 147914387.600000
2  : 295828775.300000
1  : 591657550.500000
*/

/*
public void test()
{
    // Get the attached terrain component
    Terrain terrain = GetComponent();

    // Get a reference to the terrain data
    TerrainData terrainData = terrain.terrainData;

    // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
    float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

    for (int y = 0; y < terrainData.alphamapHeight; y++)
    {
        for (int x = 0; x < terrainData.alphamapWidth; x++)
        {
            // Normalise x/y coordinates to range 0-1 
            float y_01 = (float)y / (float)terrainData.alphamapHeight;
            float x_01 = (float)x / (float)terrainData.alphamapWidth;

            // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
            float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));

            // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
            Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

            // Calculate the steepness of the terrain
            float steepness = terrainData.GetSteepness(y_01, x_01);

            // Setup an array to record the mix of texture weights at this point
            float[] splatWeights = new float[terrainData.alphamapLayers];

            // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

            // Texture[0] has constant influence
            splatWeights[0] = 0.5f;

            // Texture[1] is stronger at lower altitudes
            splatWeights[1] = Mathf.Clamp01((terrainData.heightmapHeight - height));

            // Texture[2] stronger on flatter terrain
            // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
            // Subtract result from 1.0 to give greater weighting to flat surfaces
            splatWeights[2] = 1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 5.0f));

            // Texture[3] increases with height but only on surfaces facing positive Z axis 
            splatWeights[3] = height * Mathf.Clamp01(normal.z);

            // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
            float z = splatWeights.Sum();

            // Loop through each terrain texture
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {

                // Normalize so that sum of all texture weights = 1
                splatWeights[i] /= z;

                // Assign this point to the splatmap array
                splatmapData[x, y, i] = splatWeights[i];
            }
        }
    }

    // Finally assign the new splatmap to the terrainData:
    terrainData.SetAlphamaps(0, 0, splatmapData);
}
*/
public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}