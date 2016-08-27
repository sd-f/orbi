using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Game/Moving/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public float sensitivityX = 1F;
	public float sensitivityY = 1F;

	private float minimumY = -60F;
    private float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
        if (SystemInfo.deviceType == DeviceType.Desktop)
            if (Input.GetMouseButton(0))
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
	}
	
}