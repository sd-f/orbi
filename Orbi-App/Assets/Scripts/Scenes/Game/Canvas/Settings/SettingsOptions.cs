using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/SettingsOptions")]
    class SettingsOptions : MonoBehaviour
    {
        public Button musicToggleOn;
        public Button musicToggleOff;
        public Button desktopToggleOn;
        public Button desktopToggleOff;
        public Button augmentedToggleOn;
        public Button augmentedToggleOff;

        public EasyTween musicToggle;
        public EasyTween desktopToggle;
        public EasyTween augmentedToggle;

        private bool initialized = false;

        void Start()
        {
            if (!Game.Instance.GetSettings().IsDesktopInputEnabled())
                Toggle(desktopToggle, desktopToggleOn, desktopToggleOff);
            if (!Game.Instance.GetSettings().IsMusicEnabled())
                Toggle(musicToggle, musicToggleOn, musicToggleOff);
            Toggle(augmentedToggle, augmentedToggleOn, augmentedToggleOff);
            initialized = true;
        }

        private void ToggleAllOff()
        {
            ToggleMusic();
            ToggleDesktop();
            ToggleAugmented();
        }

        public void SetMusic(bool enabled)
        {
            if (initialized)
                Game.Instance.GetSettings().SetMusicEnabled(enabled);
        }

        public void SetDesktop(bool enabled)
        {
            if (initialized)
                Game.Instance.GetSettings().SetDesktopInputEnabled(enabled);
        }

        public void SetAugmented(bool enabled)
        {
            // always false on start
        }

        public void ToggleMusic()
        {
            bool enabled = Toggle(musicToggle, musicToggleOn, musicToggleOff);
        }

        public void ToggleDesktop()
        {
            bool enabled = Toggle(desktopToggle, desktopToggleOn, desktopToggleOff);
        }

        public void ToggleAugmented()
        {
            bool enabled = Toggle(augmentedToggle, augmentedToggleOn, augmentedToggleOff);
        }

        private bool Toggle(EasyTween tween, Button on, Button off)
        {
            bool enabled = tween.IsObjectOpened();
            if (enabled)
                on.onClick.Invoke();
            else
                off.onClick.Invoke();
            return !enabled;
        }
    }
}
