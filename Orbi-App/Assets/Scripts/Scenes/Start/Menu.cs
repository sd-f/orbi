using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GameController;

namespace Assets.Scripts.Scenes.Start
{
    [AddComponentMenu("App/Scenes/Start/Menu")]
    public class Menu : MonoBehaviour
    {
        public void OnLogin()
        {
            StartCoroutine(Game.GetPlayer().GetAuthService().RequestLogin());
        }
    }
}
