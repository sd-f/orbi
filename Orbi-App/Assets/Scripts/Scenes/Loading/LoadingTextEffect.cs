using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingTextEffect : MonoBehaviour {

    void FixedUpdate()
    {
        transform.Rotate(Vector3.back * 6f);
    }
}
