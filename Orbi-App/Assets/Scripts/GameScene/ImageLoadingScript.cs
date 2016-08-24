using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class ImageLoadingScript : MonoBehaviour {

    private Image icon;
    float degreesRotated = 0.0f;

    // Use this for initialization
    void Awake () {
        icon = GetComponent<Image>();
        InvokeRepeating("CheckIfRequestsRunning", 0, 0.5f);
	}
	
    void CheckIfRequestsRunning()
    {
        if (Server.RequestsRunning())
            icon.enabled = true;
        else
            icon.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if (icon.enabled)
        {
            degreesRotated += 8;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, (degreesRotated % 360)));
        }
        
	}
}
