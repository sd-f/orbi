using System;
using GameController;
using GameController.Services;
using ServerModel;
using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/MusicController")]
    public class MusicController : GameMonoBehaviour
    {
        private bool musicOn = false;
        private bool settingsReady = false;
        private bool playMusic = false;
        private WWW musicWWW = null;
        private Song song;

        public AudioSource source;
        public MusicService musicService;

        public override void OnReady()
        {
            base.OnReady();
            settingsReady = true;
            musicOn = Game.Instance.GetSettings().IsMusicEnabled();
            if (musicOn)
                LoadNext();
        }

        private void Stop() {
            musicWWW = null;
            song = null;
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        public Song GetCurrentSong()
        {
            return this.song;
        }

        public void PlayNext(Song song)
        {
            //Debug.Log(song);
            Stop();
            this.song = song;
            musicWWW = new WWW(song.url);
            source.clip = musicWWW.GetAudioClip();
        }

        public override void OnMusicSettingsChanged()
        {
            musicOn = Game.Instance.GetSettings().IsMusicEnabled();
            if (musicOn)
                LoadNext();
            else
                Stop();
        }

        public void LoadNext()
        {
            StartCoroutine(musicService.RequestNextSong());
        }

        private void PlaySong()
        {
            source.Play();
            Invoke("LoadNext", source.clip.length);
        }

        void Update()
        {
            
            // settings loaded
            if (settingsReady)
            {
                // user enabled music
                if (!musicOn && source.isPlaying)
                    Stop();
                playMusic = musicOn && (musicWWW != null) && !source.isPlaying && (source.clip.loadState == AudioDataLoadState.Loaded);
                if (playMusic)
                    PlaySong();
            }
            
        }

    }
}
