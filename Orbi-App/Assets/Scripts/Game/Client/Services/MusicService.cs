using GameScene;
using ServerModel;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameController.Services
{

    public class MusicService: AbstractHttpService
    {

        public MusicController musicController;

        public IEnumerator RequestNextSong()
        {
            yield return Request("music/next", null, OnNextSong);
        }

        private void OnNextSong(string data)
        {
            Song song = JsonUtility.FromJson<Song>(data);
            musicController.PlayNext(song);
        }

    }
}
