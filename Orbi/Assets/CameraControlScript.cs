using UnityEngine;
using System.Collections;

public class CameraControlScript : MonoBehaviour
{

    private Vector3 firstpoint; //change type on Vector3
    private Vector3 secondpoint;
    private float xAngle = 0.0F; //angle for axes x for rotation
    private float yAngle = 0.0F;
    private float xAngTemp = 0.0F; //temp variable for angle
    private float yAngTemp = 0.0F;
    public float speed = 100.0F;

    // Use this for initialization
    void Start()
    {
        xAngle = 0.0F;
        yAngle = 0.0F;
        this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0F);
    }

    void Update()
    {
        touchCameraRotation();
        keyCameraRotation();
        
    }

    void keyCameraRotation()
    {
        // fix z axis to zero
        // key rotation of camera for debug only
        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(v.x, v.y, 0.0f);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        }
    }

    void touchCameraRotation()
    {
        //Check count touches
        if (Input.touchCount > 0)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0F / Screen.width;
                yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0F / Screen.height;
                //Rotate camera
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0F);
            }
        }
    }
}
