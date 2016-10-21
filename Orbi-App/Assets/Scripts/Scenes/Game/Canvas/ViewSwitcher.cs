using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/ViewSwitcher")]
    class ViewSwitcher : MonoBehaviour
    {
#pragma warning disable 0649
        public Camera playerCamera;
        public GameObject buttonImage;
        public Sprite cameraSprite;
        public Sprite mapsSprite;
        public RawImage image;
        private WebCamTexture webcamTexture;

        void Awake()
        {
            webcamTexture = new WebCamTexture();
            image.material.mainTexture = webcamTexture;
            if (Game.GetWorld().backGroundLayerMask == Game.GetWorld().backgroundLayersCamera)
            {
                OnSwitchView();
            }
        }

        // camera, terrain switch
        public void OnSwitchView()
        {
            if (playerCamera.cullingMask == Game.GetWorld().backgroundLayersTerrain)
            {
                buttonImage.GetComponent<Image>().sprite = mapsSprite;
                webcamTexture.Play();
                playerCamera.cullingMask = Game.GetWorld().backgroundLayersCamera;
                playerCamera.clearFlags = CameraClearFlags.Depth;
                Game.GetWorld().backGroundLayerMask = Game.GetWorld().backgroundLayersCamera;
            }
            else
            {
                buttonImage.GetComponent<Image>().sprite = cameraSprite;
                webcamTexture.Stop();
                playerCamera.cullingMask = Game.GetWorld().backgroundLayersTerrain;
                playerCamera.clearFlags = CameraClearFlags.Skybox;
                Game.GetWorld().backGroundLayerMask = Game.GetWorld().backgroundLayersTerrain;
            }
        }


        void OnDestroy()
        {
            webcamTexture.Stop();
        }

    }
}
