using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas")]
    public class Canvas : MonoBehaviour
    {

        void Awake()
        {

        }

        public void OnSettings()
        {
            SceneManager.LoadScene("SettingsScene");
        }

    }
}
