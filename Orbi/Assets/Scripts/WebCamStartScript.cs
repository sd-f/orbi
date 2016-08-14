using UnityEngine;
using System.Collections;

public class WebCamStartScript : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        checkCameraPermission();
        startWebCamTexture();
    }

    void startWebCamTexture()
    {
        WebCamTexture webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void checkCameraPermission()
    {
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //Debug.Log("No Camera Permission");
        }
        else
        {
            //Debug.Log("No Camera Permission");
        }
    }
}
