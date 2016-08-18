using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RefreshIconScript : MonoBehaviour {


    float degreesRotated = 0.0f;

    void Start () {
        //GetComponent<MeshRenderer>().enabled = false;
	}

    void Update()
    {
        if (GetComponent<Image>().enabled)
        {
            degreesRotated += 8;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, (degreesRotated % 360)));
        }
        
    }


}
