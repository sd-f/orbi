using GameController;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/AugmentedSwitcher")]
    class AugmentedSwitcher : MonoBehaviour
    {
#pragma warning disable 0649
        public Camera playerCamera;
        public Camera augmentedCamera;
        public RawImage image;
        private WebCamTexture webcamTexture;

        void OnEnable()
        {
            augmentedCamera.gameObject.SetActive(true);
            webcamTexture.Play();
            playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersCamera;
            Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersCamera;
        }

        void OnDisable()
        {
            augmentedCamera.gameObject.SetActive(false);
            webcamTexture.Stop();
            playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersTerrain;
            Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersTerrain;
        }

        void Awake()
        {
            webcamTexture = new WebCamTexture();
            image.material.mainTexture = webcamTexture;
        }


        void OnDestroy()
        {
            webcamTexture.Stop();
        }

    }
}
