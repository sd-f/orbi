using UnityEngine;
using System.Collections;

public class LevelUpText : MonoBehaviour {

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
            transform.localRotation = Quaternion.Slerp(transform.localRotation,  Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 10f, transform.localEulerAngles.z), 0.2f);
            transform.position += (Vector3.up / 75f);
            if (transform.localScale.x > 0f)
                transform.localScale += scaleVector;
            Color color = textMesh.color;
            color.a = color.a - 0.01f;
            textMesh.color = color;
            if (color.a < 0.1)
                Destroy(this.gameObject, 0.1f);
        }

    }
}
