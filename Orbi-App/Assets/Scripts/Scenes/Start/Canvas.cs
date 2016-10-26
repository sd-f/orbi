using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene
{
    [AddComponentMenu("App/Scenes/Start/Canvas")]
    public class Canvas : MonoBehaviour
    {
        public Text textNumberOfObjects;
        public Text textVersion;

        void Start()
        {
            textVersion.text = "version " +  Client.VERSION.ToString();
        }

        public void UpdateStats()
        {
            textNumberOfObjects.text = "objects " + Game.Instance.GetWorld().GetStatistics().numberOfObjects.ToString();
        }


    }
}
