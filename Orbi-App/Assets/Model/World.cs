using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Model
{
    [Serializable]
    class World
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public long clientVersion;

        public void ReplaceGameObjects(List<GameObject> gameObjects)
        {
            this.gameObjects = new List<GameObject>();
            foreach(GameObject gameObject in gameObjects)
            {
                this.gameObjects.Add(gameObject);
            }
        }
    }
}
