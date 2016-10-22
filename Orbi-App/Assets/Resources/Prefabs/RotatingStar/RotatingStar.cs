using UnityEngine;
using System.Collections;

public class RotatingStar : MonoBehaviour {

	void Update () {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, transform.localEulerAngles.y + 5f, 0f), 0.5f);
	}
}
