using Assets.Model;
using UnityEngine;

namespace GameController
{
    [AddComponentMenu("App/Game/Player")]
    class Player : MonoBehaviour
    {
        private static GeoPosition START_POSITION = new GeoPosition(47.073158d, 15.438000d, 2.0d); // schlossberg
        private GeoPosition geoPosition;

        public Player()
        {
            this.geoPosition = START_POSITION;
        }

    }
}