using CanvasUtility;
using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Init")]
    class Init : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("StartScene");
        }

    }
}