using UnityEngine;
using System.Collections;
using Assets.Model;
using Assets.Control;

[AddComponentMenu("Game/Moving/Move Location")]
public class MoveLocationScript : MonoBehaviour {

    UnityEngine.GameObject moveToObject;

    void Start()
    {
        moveToObject = UnityEngine.GameObject.Find("InFrontOfCamera");
    }

	// Update is called once per frame
	void Update () {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            moveWithKeys();
        }

    }

    void moveWithKeys()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            GeoPosition geoPosition = new GeoPosition(moveToObject.transform.position.z, moveToObject.transform.position.x, moveToObject.transform.position.y);
            Game.GetInstance().GetAdapter().ToReal(geoPosition, Game.GetInstance().player);
            Game.GetInstance().player.geoPosition.latitude = geoPosition.latitude;
            Game.GetInstance().player.geoPosition.longitude = geoPosition.longitude;
        }
    }
}
