using GameController;
using System.Text;
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
        public Text icon;
        public RawImage image;
        private WebCamTexture webcamTexture;

        void Start()
        {
            if (Game.Instance.GetWorld().backGroundLayerMask == Game.Instance.GetWorld().backgroundLayersCamera)
            {
                OnSwitchView();
            }
        }

        void Awake()
        {
            webcamTexture = new WebCamTexture();
            image.material.mainTexture = webcamTexture;
        }

        // camera, terrain switch
        public void OnSwitchView()
        {
            if (playerCamera.cullingMask == Game.Instance.GetWorld().backgroundLayersTerrain)
            {
                icon.text = '\uf041'.ToString();
                webcamTexture.Play();
                playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersCamera;
                //playerCamera.clearFlags = CameraClearFlags.SolidColor;
                Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersCamera;
            }
            else
            {
                icon.text = '\uf030'.ToString();
                webcamTexture.Stop();
                playerCamera.cullingMask = Game.Instance.GetWorld().backgroundLayersTerrain;
                //playerCamera.clearFlags = CameraClearFlags.Skybox;
                Game.Instance.GetWorld().backGroundLayerMask = Game.Instance.GetWorld().backgroundLayersTerrain;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
                OnSwitchView();
        }


        void OnDestroy()
        {
            webcamTexture.Stop();
        }

    }
}
