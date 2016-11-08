using UnityEngine;
using GameScene;

public class ObjectSelector : GameMonoBehaviour
{
    private Vector3 firstpoint;
    private Vector3 secondpoint;
    private Vector2 centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
    private float touchThresholdX = Screen.width * 20;
    private float touchThresholdY = Screen.height * 20;

    public override void Start()
    {
        base.Start();
        centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        touchThresholdX = Screen.width * 20;
        touchThresholdX = Screen.height * 20;
    }

    void Update()
    {
        if (desktopMode)
        {
            if (Input.GetKeyDown(KeyCode.F))
                checkTouchObjectSingleTouch(centerOfScreen);
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    firstpoint = Input.GetTouch(0).position;
                if (Input.GetTouch(0).phase == TouchPhase.Ended) { 
                    secondpoint = Input.GetTouch(0).position;
                    float movedX = (secondpoint.x - firstpoint.x) / touchThresholdX;
                    float movedY = (secondpoint.y - firstpoint.y) / touchThresholdY;
                    if ((movedX > -5) && (movedX < 5) && (movedY > -5) && (movedY < 5))
                    {
                        checkTouchObjectSingleTouch(secondpoint);
                    }
                }

            }
        }
    }

    private void checkTouchObjectSingleTouch(Vector2 position)
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit))
        {
            hit.transform.gameObject.SendMessage("OnTouched", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}
