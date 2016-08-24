using UnityEngine;
using System.Collections;
using Assets.Control;
using UnityEngine.UI;

public class ToggleHeightsScript : MonoBehaviour {

	void Awake () {
        GetComponent<Toggle>().isOn = Game.GetInstance().IsHeightsEnabled();
	}

    public void SetHeightsEnabled(bool enabled)
    {
        Game.GetInstance().SetHeightsEnabled(enabled);
    }
}
