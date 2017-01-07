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

        public virtual void Awake()
        {
            Settings.OnInputModeChanged += SetInputMode;
            Settings.OnMusicChanged += OnMusicSettingsChanged;
            Game.OnTypingModeChanged += SetTypingMode;
            Game.OnReady += SetReady;
        }

        public virtual void Start()
        {
            ready = Game.Instance.IsReady();
            typingMode = Game.Instance.IsInTypingMode();
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        public virtual void OnEnable()
        {
            if (Game.Instance != null)
            {
                ready = Game.Instance.IsReady();
                typingMode = Game.Instance.IsInTypingMode();
                desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
            }
        }


        public virtual void OnDestroy()
        {
            Settings.OnInputModeChanged -= SetInputMode;
            Game.OnTypingModeChanged -= SetTypingMode;
            Settings.OnMusicChanged -= OnMusicSettingsChanged;
            Game.OnReady -= SetReady;
        }
        

        private void SetReady()
        {
            ready = Game.Instance.IsReady();
            if (ready)
            {
                SetInputMode();
                SetTypingMode();
                OnReady();
            }
                
        }

        private void SetInputMode()
        {
            desktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
            OnInputModeChanged();
        }

        private void SetTypingMode()
        {
            typingMode = Game.Instance.IsInTypingMode();
            OnTypingModeChanged();
        }

        public virtual void OnReady()
        {
        }

        public virtual void OnTypingModeChanged()
        {
        }

        public virtual void OnInputModeChanged()
        {
        }

        public virtual void OnMusicSettingsChanged()
        {
        }



    }
}
