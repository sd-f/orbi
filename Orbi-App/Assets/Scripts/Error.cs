using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Error : MonoBehaviour {

    private static GameObject INSTANCE;

	// Use this for initialization
	void Start () {
        INSTANCE = GameObject.Find("ErrorPanel");
        INSTANCE.SetActive(false);
	}

    public static void Show(string message)
    {
        Debug.LogError(message);
        INSTANCE.SetActive(true);
        Text text = INSTANCE.GetComponentInChildren<Text>();
        text.text = message;
        INSTANCE.GetComponent<Error>().Invoke("Hide", 4);
    }


    void Hide()
    {
        INSTANCE.SetActive(false);
    }
}
