using UnityEngine;
using GameController;
using ServerModel;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/CharacterProperties")]
    class CharacterProperties : MonoBehaviour
    {

        private Character character;

        public void SetCharacter(Character character)
        {
            this.character = character;
        }
        
        public Character GetCharacter()
        {
            return this.character;
        }

        public void OnTouched()
        {
            UnityEngine.GameObject messagesButton = UnityEngine.GameObject.Find("ButtonMessages");
            if (messagesButton != null)
            {
                MessagesCanvas canvas = messagesButton.GetComponent<MessagesCanvas>();
                canvas.ShowInteractionForm(character.name, character.id);
            }
        }

    }

    
}