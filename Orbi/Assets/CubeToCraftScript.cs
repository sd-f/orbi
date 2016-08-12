using UnityEngine;
using System.Collections;

public class CubeToCraftScript : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Mathf.Sin(Time.time * 5f)*100f);
        Vector3 newRotVector = Random.insideUnitCircle * 100f;
        Quaternion newRot = Quaternion.Euler(newRotVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.1f * Time.deltaTime);
    }
}
