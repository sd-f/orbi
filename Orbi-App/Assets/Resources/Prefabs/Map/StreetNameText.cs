using UnityEngine;
using System.Collections;

public class StreetNameText : MonoBehaviour {

    private GameObject background;
    private GameObject text;
    private Vector3 scale;

	// Use this for initialization
	void Start () {
        //
        text = transform.FindChild("StreetNameText").gameObject;
        background = transform.FindChild("StreetNameBackground").gameObject;
        TextMesh textMesh = text.GetComponent<TextMesh>();
        //
        scale = background.transform.localScale;
        //scale.x = textMesh.GetComponent<Renderer>().bounds.size.x / 3f;
        Vector3 dimensions = textMesh.GetComponent<Renderer>().bounds.size;
        
        scale.x = textMesh.text.Length * 0.075f;
        background.transform.localScale = scale;
    }
	
}
