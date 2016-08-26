using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class CanvasBackgroundImageScript : MonoBehaviour {

    private Image image;

	void Awake() {
        image = GetComponent<Image>();
        Invoke("CheckIfLoaded",2);
	}
	
	// Update is called once per frame
	void CheckIfLoaded () {
	    if (!Server.RequestsRunning())
        {
            image.enabled = false;
        } else
        {
            Invoke("CheckIfLoaded", 0.5f);
        }
	}

    void OnDestroy()
    {
        CancelInvoke();
    }
}
