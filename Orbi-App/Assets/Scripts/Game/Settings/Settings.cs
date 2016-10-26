using CanvasUtility;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GameController
{
    public class Settings : MonoBehaviour
    {
        private static string SETTINGS_FILE_PATH = "/settings.data";
        private SettingsData data = new SettingsData();

        void Start()
        {
            Load();
        }

        public void Init()
        {
            Load();
        }

        public void SetHeightsEnabled(bool enabled)
        {
            this.data.heightsEnabled = false; // TODO disabled
            Save();
        }

        public void SetSatelliteOverlayEnabled(bool enabled)
        {
            this.data.satelliteOverlayEnabled = false; // TODO disabled
            Save();
        }

        public void SetHandheldInputEnabled(bool enabled)
        {
            this.data.handheldInputEnabled = enabled;
            Save();
        }

        public void SetDesktopInputEnabled(bool enabled)
        {
            this.data.desktopInputEnabled = enabled;
            Save();
        }

        public void SetToken(string token)
        {
            if (!String.IsNullOrEmpty(token))
                this.data.token = SecurePreferences.EncryptStringified(token);
            else
                this.data.token = null;
            Save();
        }

        public void SetClientVersion(int version)
        {
            this.data.clientVersion = version;
            Save();
        }

        public bool IsHeightsEnabled()
        {
            return false; // TODO disabled
            //return this.data.heightsEnabled;
        }

        public bool IsSatelliteOverlayEnabled()
        {
            return false; // TODO disabled no satellite available
            //return this.data.satelliteOverlayEnabled;
        }

        public bool IsHandheldInputEnabled()
        {
            return this.data.handheldInputEnabled;
        }

        public bool IsDesktopInputEnabled()
        {
            return this.data.desktopInputEnabled;
        }

        public string GetToken()
        {
            if (this.data.token == null)
                return null;
            return SecurePreferences.DecryptStringified(this.data.token);
        }

        public string GetEmail()
        {
            return this.data.email;
        }

        public void SetEmail(String email)
        {
            this.data.email = email;
            Save();
        }

        private void Load()
        {
            if (File.Exists(Application.persistentDataPath + SETTINGS_FILE_PATH))
            {
                try
                {
                    string readText = File.ReadAllText(Application.persistentDataPath + SETTINGS_FILE_PATH);
                    this.data = JsonUtility.FromJson<SettingsData>(readText);
                    //this.data = (SettingsData)bf.Deserialize(file);
                }
                catch (Exception ex)
                {

                    Error.Show("Error loading save game, Settings will be set to default");
                    Debug.LogError(ex.Message);
                    File.Delete(Application.persistentDataPath + SETTINGS_FILE_PATH);
                    Save();
                }
            }
            else
            {
                this.data.handheldInputEnabled = (SystemInfo.deviceType == DeviceType.Handheld);
                this.data.desktopInputEnabled = (SystemInfo.deviceType == DeviceType.Desktop);
                Save();
            } 
        }

        void Save()
        {
            try
            {
                File.WriteAllText(Application.persistentDataPath + SETTINGS_FILE_PATH, JsonUtility.ToJson(this.data));
            } catch (Exception ex)
            {
                Error.Show("Error saving game: " + ex.Message);
            }
        }

    }


    
}
