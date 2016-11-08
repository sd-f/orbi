using GameController;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/GameMonoBehaviour")]
    public class GameMonoBehaviour : MonoBehaviour
    {

        protected bool ready = false;
        protected bool desktopMode = false;
        protected bool typingMode = false;

        public virtual void Start()
        {
            ready = Game.Instance.IsReady();
            typingMode = Game.Instance.IsInTypingMode();
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        public virtual void OnEnable()
        {
            Settings.OnInputModeChanged += SetInputMode;
            Game.OnTypingModeChanged += SetTypingMode;
            Game.OnReady += SetReady;
        }

        
        public virtual void OnDisable()
        {
            Settings.OnInputModeChanged -= SetInputMode;
            Game.OnTypingModeChanged -= SetTypingMode;
            Game.OnReady -= SetReady;
        }
        

        public virtual void SetReady()
        {
            ready = Game.Instance.IsReady();
            if (ready)
                OnReady();
        }

        public virtual void SetInputMode()
        {
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        public virtual void SetTypingMode()
        {
            typingMode = Game.Instance.IsInTypingMode();
        }

        public virtual void OnReady()
        {
            SetInputMode();
            SetTypingMode();
        }



    }
}
