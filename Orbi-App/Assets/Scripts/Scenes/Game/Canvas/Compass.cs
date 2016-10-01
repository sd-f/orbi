using GameController;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/Compass")]
    class Compass : MonoBehaviour
    {
        public Image compassImage;
        public Image buttonBackground;
        public GameObject playerBody;

        private FloatFilter magneticFilter = new AngleFilter(10);
        private bool headingNorth = false;

        void Awake()
        {
            Sensor.Activate(Sensor.Type.MagneticField);
            Sensor.Activate(Sensor.Type.Accelerometer);
            headingNorth = false;
            InvokeRepeating("CheckIfNorth", 1, 0.1f);
        }

        void Update()
        {
            Text text = GameObject.Find("DebugText").GetComponent<Text>();
            text.text = "center-pos: " + Game.GetLocation().GetGeoLocation() + "\n"
                + "player-pos: " + Game.GetPlayer().GetModel().geoPosition + " (frozen=" + Game.GetPlayer().IsFrozen() + ")\n"
                + "player: " + Game.GetPlayer().GetPlayerBody().transform.position + "\n"
                + "selected: " + Game.GetPlayer().GetCraftingController().GetSelectedPrefab();
            compassImage.transform.rotation = Quaternion.Slerp(compassImage.transform.rotation, Quaternion.Euler(0, 0, magneticFilter.Update(Sensor.GetOrientation().x)), Time.deltaTime * 2);
        }

        void CheckIfNorth() {
            if (isNorth())
            {
                if (!headingNorth)
                {
                    headingNorth = true;
                    buttonBackground.color = Color.green;
                }
            }
            else
            {
                if (headingNorth)
                {
                    headingNorth = false;
                    buttonBackground.color = Color.white;
                }
            }

        }

        public void OnCalibrate()
        {
            playerBody.GetComponent<PlayerBodyController>().UpdateDeltaCompass();
        }

        private bool isNorth()
        {
            return ((magneticFilter.Value) < 5.0f) && ((magneticFilter.Value) > -5.0f);
        }

        void OnDestroy()
        {
            CancelInvoke();
        }

    }
}
