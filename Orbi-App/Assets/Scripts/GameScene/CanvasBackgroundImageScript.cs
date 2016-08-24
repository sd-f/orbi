using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class CanvasBackgroundImageScript : MonoBehaviour {

    private Image image;

	void Start () {
        image = GetComponent<Image>();
        Invoke("CheckIfLoaded",0.5f);
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
}
