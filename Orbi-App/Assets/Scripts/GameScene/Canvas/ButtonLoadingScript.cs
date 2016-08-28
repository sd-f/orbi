using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class ButtonLoadingScript : MonoBehaviour {

    private Image bgIcon;
    private Image refreshIcon;
    float degreesRotated = 0.0f;

    void Awake() {
        bgIcon = GetComponent<Image>();
        refreshIcon = GameObject.Find("ImageButtonLoading").GetComponent<Image>();
        InvokeRepeating("CheckIfRequestsRunning", 0, 0.5f);
	}
	
    void CheckIfRequestsRunning()
    {
        if (Assets.Control.util.Server.RequestsRunning())
        {
            bgIcon.enabled = true;
            refreshIcon.enabled = true;
        }
        else
        {
            bgIcon.enabled = false;
            refreshIcon.enabled = false;
        }
            
    }

	// Update is called once per frame
	void Update () {
        if (bgIcon.enabled)
        {
            degreesRotated += 8;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, (degreesRotated % 360))), 1);
        }
        
	}

    void OnDestroy()
    {
        CancelInvoke();
    }
}
