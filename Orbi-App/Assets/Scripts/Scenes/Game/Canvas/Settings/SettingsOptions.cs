using System;
using GameController;
using UnityEngine;
using UnityEngine.UI;
using CanvasUtility;
using GameController.Services;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Canvas/SettingsCanvas/SettingsOptions")]
    class SettingsOptions : MonoBehaviour
    {
#pragma warning disable 0649
        public Button musicToggleOn;
        public Button musicToggleOff;
        public Button desktopToggleOn;
        public Button desktopToggleOff;
        public Button augmentedToggleOn;
        public Button augmentedToggleOff;

        public EasyTween musicToggle;
        public EasyTween desktopToggle;
        public EasyTween augmentedToggle;

        public GameObject augmentedCanvas;
        public Text songInfoText;
        public MusicController musicController;

        private bool initialized = false;

        void Start()
        {
            if (!Game.Instance.GetSettings().IsDesktopInputEnabled())
                Toggle(desktopToggle, desktopToggleOn, desktopToggleOff);
            if (!Game.Instance.GetSettings().IsMusicEnabled())
                Toggle(musicToggle, musicToggleOn, musicToggleOff);
            if (!Game.Instance.GetSettings().IsAugmentedEnabled())
                Toggle(augmentedToggle, augmentedToggleOn, augmentedToggleOff);
            initialized = true;
        }

        private void OnEnable()
        {
            InvokeRepeating("UpdateSongInformation", 1, 1);
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
            {
                Game.Instance.GetSettings().SetMusicEnabled(enabled);
            }
                
        }

        public void SetDesktop(bool enabled)
        {
            if (initialized)
                Game.Instance.GetSettings().SetDesktopInputEnabled(enabled);
        }

        public void SetAugmented(bool enabled)
        {
            if (initialized)
            {
                Game.Instance.GetSettings().SetAugmentedEnabled(enabled);
                augmentedCanvas.SetActive(enabled);
            }
        }

        public void OpenPrivacyPolicy()
        {
            Application.OpenURL("https://softwaredesign.foundation/orbi/privacy");
        }

        public void ToggleMusic()
        {
            Toggle(musicToggle, musicToggleOn, musicToggleOff);
        }

        public void NextSong()
        {
            musicController.LoadNext();
        }

        public void UpdateSongInformation()
        {
            if (musicController.GetCurrentSong() != null)
            {
                songInfoText.text = musicController.GetCurrentSong().title;
            }
        }

        public void ToggleDesktop()
        {
            Toggle(desktopToggle, desktopToggleOn, desktopToggleOff);
        }

        public void ToggleAugmented()
        {
            Toggle(augmentedToggle, augmentedToggleOn, augmentedToggleOff);
        }

        private bool Toggle(EasyTween tween, Button on, Button off)
        {
            bool enabled = !tween.IsObjectOpened();
            if (enabled)
                on.onClick.Invoke();
            else
                off.onClick.Invoke();
            return !enabled;
        }

        public void OnDisable()
        {
            CancelInvoke();
        }
    }
}
