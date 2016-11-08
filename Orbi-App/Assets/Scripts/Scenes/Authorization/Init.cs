using GameController;
using UnityEngine;

namespace AuthorizationScene
{
    [AddComponentMenu("App/Scenes/Authorization/Init")]
    class Init : MonoBehaviour
    {

        void Awake()
        {
            Game.Instance.EnterTypingMode();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Game.Instance.Quit();
        }

        void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}