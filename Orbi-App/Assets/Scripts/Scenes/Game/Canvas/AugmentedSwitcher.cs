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
            augmentedCamera.enabled = true;
            webcamTexture.Play();
            playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersCamera;
            playerCamera.clearFlags = CameraClearFlags.Depth;
            //Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersCamera;
        }

        void OnDisable()
        {
            augmentedCamera.enabled = false;
            webcamTexture.Stop();
            playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersTerrain;
            playerCamera.clearFlags = CameraClearFlags.Skybox;
            //Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersTerrain;
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
