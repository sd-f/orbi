using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GameController;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Start
{
    [AddComponentMenu("App/Scenes/Start/Menu")]
    public class Menu : MonoBehaviour
    {
        void Awake()
        {
            if (!Game.GetPlayer().IsLoggedIn())
            {
                GameObject.Find("ButtonLogin").GetComponent<Button>().interactable = true;
            }
        }

        public void OnLogin()
        {
            StartCoroutine(Game.GetPlayer().GetAuthService().RequestLogin());
        }
    }
}
