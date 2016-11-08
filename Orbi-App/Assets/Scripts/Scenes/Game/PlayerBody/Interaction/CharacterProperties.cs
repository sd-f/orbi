using UnityEngine;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/CharacterProperties")]
    class CharacterProperties : MonoBehaviour
    {

        private ServerModel.Character character;

        public void SetCharacter(ServerModel.Character character)
        {
            this.character = character;
        }
        
        public ServerModel.Character GetCharacter()
        {
            return this.character;
        }

        public void OnTouched()
        {
            if (this.gameObject != null)
                GameObject.Find("Canvas").GetComponent<MainCanvas>().OpenCharacterInfos(character);
        }

    }

    
}