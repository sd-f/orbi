using UnityEngine;
using System.Collections;

public class CameraPlaneScript : MonoBehaviour {

    private Vector2 lastScreenSize = new Vector2(1920, 1080);
    private Camera cameraCamera;
    private GameObject plane;

    IEnumerator Start()
    {
        cameraCamera = GameObject.Find("CameraCamera").GetComponent<Camera>();
        plane = this.gameObject;
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield return new WaitForSeconds(3);
            Application.Quit();
            yield break;
        }

        AdjustPlaneSize();
        InvokeRepeating("OnScreenSizeChanged", 0, 1);

    }

    void AdjustPlaneSize()
    {
        //Get a world space vector to the upper right corner of the screen
        Vector3 UpRight = cameraCamera.ViewportToWorldPoint(new Vector3(1, 1, 128));
        //Get a would space vector to the lower left corner of the screen
        Vector3 DownLeft = cameraCamera.ViewportToWorldPoint(new Vector3(0, 0, 128));
        //Set our width scale to be right - left
        //Set our height scale to be up - down

        float width = UpRight.x - DownLeft.x;
        float height = UpRight.y - DownLeft.y;
        //plane.transform.position = new Vector3((width / 2), (height / 2) + 2.0f, 128);
        plane.transform.localScale = new Vector3(width/10, 0, height/10);
    }

    void OnScreenSizeChanged()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {

            AdjustPlaneSize();
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

}
