using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/InputModeMonoBehaviour")]
    public class InputModeMonoBehaviour : MonoBehaviour
    {

        protected bool desktopMode = false;
        protected bool typingMode = false;

        void OnEnable()
        {
            Settings.OnInputModeChanged += SetInputMode;
            Game.OnTypingModeChanged += SetTypingMode;
        }

        void OnDisable()
        {
            Settings.OnInputModeChanged -= SetInputMode;
            Game.OnTypingModeChanged -= SetTypingMode;
        }

        public virtual void SetInputMode()
        {
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        public virtual void SetTypingMode()
        {
            typingMode = Game.Instance.IsInTypingMode();
        }


    }
}
