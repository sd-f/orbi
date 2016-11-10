using GameController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/SettingsCanvas")]
    public class SettingsCanvas : MonoBehaviour
    {
        public GameObject contentTabInventory;
        public GameObject contentTabSettings;
        public GameObject contentTabMessages;
        public Canvas inventoryCanvas;
        public Camera inventoryCamera;
        public GameObject messagesNewIndicator;
        public GameObject inventoryNewIndicator;
        private SettingsTabs currentTab = SettingsTabs.Settings;
        private static Color COLOR_ACTIVE = new Color(0.259f,0.522f,0.957f);
        private static Color COLOR_INACTIVE = new Color(0.196f,0.196f,0.196f);

        void OnEnable()
        {
            SetTabState(currentTab, true);
            SetIndicators();
        }

        public void SetIndicators()
        {
            inventoryNewIndicator.SetActive(Game.Instance.GetPlayer().GetInventoryService().HasNewItems());
            List<ServerModel.CharacterMessage> messages = Game.Instance.GetPlayer().GetMessageService().GetMessages();
            messagesNewIndicator.SetActive(((messages != null) && (messages.Count > 0)));
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            if (currentTab == SettingsTabs.Inventory)
            {
                inventoryCanvas.enabled = false;
                inventoryCamera.enabled = false;
            }
                
        }

        public void OpenSettingsTab()
        {
            SetTabState(SettingsTabs.Settings, true);
        }

        public void OpenInventoryTab()
        {
            SetTabState(SettingsTabs.Inventory, true);
        }

        public void OpenMessagesTab()
        {
            SetTabState(SettingsTabs.Messages, true);
        }

        private void SetTabState(SettingsTabs tab, bool state)
        {
            SetTabState(tab, state, false);
        }

        private void SetTabState(SettingsTabs tab, bool state, bool recursive)
        {
            GetContentTab(tab).SetActive(state);
            if (tab == SettingsTabs.Inventory)
            {
                inventoryCanvas.enabled = state;
                inventoryCamera.enabled = state;
            }
                
            GameObject.Find(tab.ToString() + "TabTextInfo").GetComponent<Text>().color = GetColorFromState(state);
            GameObject.Find(tab.ToString() + "TabSeparator").GetComponent<Image>().color = GetColorFromState(state);
            if (!recursive)
                foreach (SettingsTabs otherTab in Enum.GetValues(typeof(SettingsTabs)) )
                    if (otherTab != tab)
                        SetTabState(otherTab, false, true);
            currentTab = tab;
            SetIndicators();
        }

        private GameObject GetContentTab(SettingsTabs tab)
        {
            if (tab == SettingsTabs.Inventory)
                return contentTabInventory;
            if (tab == SettingsTabs.Messages)
                return contentTabMessages;
            return contentTabSettings;
        }

        private Color GetColorFromState(bool state)
        {
            if (state)
                return COLOR_ACTIVE;
            return COLOR_INACTIVE;
        }

        public enum SettingsTabs
        {
            Settings,
            Inventory,
            Messages
        }

    }
}
