using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class ToggleSatelliteScript : MonoBehaviour {

	void Awake () {
        GetComponent<Toggle>().isOn = GoogleMapsService.satellite;
	}

    public void SetSatelliteOverlayEnabled(bool enabled)
    {
        GoogleMapsService.satellite = enabled;
    }
}
