using UnityEngine;
using UnityEngine.UI;

public class XPEarnedText : MonoBehaviour {

    private bool fading = false;
    private TextMesh textMesh;
    private static float scaleFactor = 0.01f;
    private Vector3 scaleVector = new Vector3(scaleFactor, scaleFactor, scaleFactor);

    public void SetAmount(long amount)
    {
        GetComponent<TextMesh>().text = amount + " XP";
    }

    void Start()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
        Invoke("StartFading", 0.5f);
    }

    void StartFading()
    {
        fading = true;
    }

    void Update()
    {
        if (fading)
        {
            transform.position += (Vector3.up / 50f);
            if (transform.localScale.x > 0f)
                transform.localScale += scaleVector;
            Color color = textMesh.color;
            color.a = color.a - 0.02f;
            gameObject.GetComponent<TextMesh>().color = color;
            if (color.a < 0.1)
                Destroy(this.gameObject, 0.1f);
        }

    }
}
