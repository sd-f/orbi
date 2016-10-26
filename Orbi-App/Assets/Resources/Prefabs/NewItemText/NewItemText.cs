using UnityEngine;
using System.Collections;

public class NewItemText : MonoBehaviour {

    private bool fading = false;
    private TextMesh textMesh;
    private static float scaleFactor = 0.002f;
    private Vector3 scaleVector = new Vector3(scaleFactor, scaleFactor, scaleFactor);


    void Start()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
        Invoke("StartFading", 2f);
    }

    void StartFading()
    {
        fading = true;
    }

    void Update()
    {
        if (fading)
        {
            Color color = textMesh.color;
            color.a = color.a - 0.01f;
            textMesh.color = color;
            if (color.a < 0.1)
                Destroy(this.gameObject, 0.1f);
        }

    }
}
