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
        private Boolean headingNorth = false;

        void Update()
        {
            Text text = GameObject.Find("DebugText").GetComponent<Text>();
            text.text = "location: " + Game.GetLocation().GetGeoLocation() + "\n"
                + "heading: " + Game.GetLocation().GetHeading();
            float heading = Game.GetLocation().GetHeading();
            compassImage.transform.rotation = Quaternion.Slerp(compassImage.transform.rotation, Quaternion.Euler(0, 0, heading), Time.deltaTime * 2);
            if (isHeadingNorth())
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

        private bool isHeadingNorth()
        {
            float heading = Game.GetLocation().GetHeading();
            return ((heading) < 5.0f) && ((heading) > -5.0f);
        }

    }
}
