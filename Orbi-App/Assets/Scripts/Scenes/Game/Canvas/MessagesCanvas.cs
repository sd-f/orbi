﻿using GameController;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace GameScene
{

    [AddComponentMenu("App/Scenes/Game/Canvas/MessagesCanvas")]
    public class MessagesCanvas : MonoBehaviour
    {
        public GameObject messagesScrollView;
        public GameObject messagesContainer;
        public GameObject messagePrefab;

        public GameObject interactionPanel;
        public InputField messageInputField;
        public Text characterNameText;
        private long selectedCharacterId = -1;

        private bool isChatWindowActive = false;
        private bool isInteractionWindowActive = false;
        private bool hasNewMessages = false;

        void Start()
        {
            InvokeRepeating("CheckForMessages", 1f, 5f);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                ToggleMessagesScrollView();

        }

        public void ToggleMessagesScrollView()
        {
            messagesScrollView.SetActive(!isChatWindowActive);
            if (!isChatWindowActive)
            {
                Read();
                Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(false);
                
                GameObjectUtility.DestroyAllChildObjects(messagesContainer);
                foreach(ServerModel.CharacterMessage message in Game.GetPlayer().GetMessages())
                {
                    GameObject chatLine = GameObject.Instantiate(messagePrefab) as GameObject;
                    chatLine.transform.SetParent(messagesContainer.transform, false);
                    chatLine.GetComponent<Text>().text = message.fromCharacter + " > " + message.message;
                }
            } else
            {
                Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(true);
            }
            isChatWindowActive = !isChatWindowActive;
        }

        public void OnCancelInteraction()
        {
            if (!isInteractionWindowActive)
                Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(true);
            CleanupAndHideForm();
        }

        public void OnSendMessage()
        {
            StartCoroutine("SendMessage");
        }

        IEnumerator SendMessage()
        {
            Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(true);
            
            yield return Game.GetPlayer().GetMessageService().RequestMessage(messageInputField.text, selectedCharacterId);
            CleanupAndHideForm();
        }

        void CheckForMessages()
        {
            StartCoroutine("LoadMessages");
        }

        IEnumerator LoadMessages()
        {
            yield return Game.GetPlayer().GetMessageService().RequestMessages(this);
        }

        public void ShowInteractionForm(string characterName, long characterId)
        {
            Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(false);
            this.selectedCharacterId = characterId;
            this.characterNameText.text = characterName;
            this.interactionPanel.SetActive(true);
        }

        void CleanupAndHideForm()
        {
            if (!isChatWindowActive)
                Game.GetPlayer().GetPlayerBodyController().SetMouseRotationEnabled(true);
            this.selectedCharacterId = -1;
            this.messageInputField.text = null;
            this.characterNameText.text = "...";
            this.interactionPanel.SetActive(false);
        }

        public void Unread()
        {
            GetComponent<Image>().color = Color.green;
        }

        void Read()
        {
            GetComponent<Image>().color = Color.white;
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
    }

}