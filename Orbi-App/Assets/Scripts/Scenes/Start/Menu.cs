using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes.Start
{
    [AddComponentMenu("App/Scenes/Start/Menu")]
    public class Menu : MonoBehaviour
    {
        public void OnClickStart()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
