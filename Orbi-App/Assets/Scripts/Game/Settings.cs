using CanvasUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace GameController
{
    class Settings
    {
        private static string SETTINGS_FILE_PATH = "/settings.data";
        private SettingsData data = new SettingsData();

        public void SetHeightsEnabled(bool enabled)
        {
            this.data.heightsEnabled = enabled;
            Save();
        }

        public void SetSatelliteOverlayEnabled(bool enabled)
        {
            this.data.satelliteOverlayEnabled = enabled;
            Save();
        }

        public void SetHandheldInputEnabled(bool enabled)
        {
            this.data.handheldInputEnabled = enabled;
            Save();
        }

        public void SetDesktopInputEnabled(bool enabled)
        {
            this.data.satelliteOverlayEnabled = enabled;
            Save();
        }

        public void SetToken(string token)
        {
            this.data.token = token;
            Save();
        }

        public void SetClientVersion(int version)
        {
            this.data.clientVersion = version;
            Save();
        }

        public bool IsHeightsEnabled()
        {
            return this.data.heightsEnabled;
        }

        public bool IsSatelliteOverlayEnabled()
        {
            return this.data.satelliteOverlayEnabled;
        }

        public bool IsHandheldInputEnabled()
        {
            return this.data.handheldInputEnabled;
        }

        public bool IsDesktopInputEnabled()
        {
            return this.data.satelliteOverlayEnabled;
        }

        public string GetToken()
        {
            return this.data.token;
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

        public void Load()
        {
            if (File.Exists(Application.persistentDataPath + SETTINGS_FILE_PATH))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + SETTINGS_FILE_PATH, FileMode.Open);
                    this.data = (SettingsData)bf.Deserialize(file);
                    file.Close();
                }
                catch (Exception ex)
                {
                    Error.Show("Error loading save game: " + ex.Message);
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
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + SETTINGS_FILE_PATH);
                bf.Serialize(file, data);
                file.Close();
            } catch (Exception ex)
            {
                Error.Show("Error saving game: " + ex.Message);
            }
        }
    }

    [Serializable]
    class SettingsData
    {
        public Boolean heightsEnabled = false;
        public Boolean satelliteOverlayEnabled = false;
        public Boolean handheldInputEnabled = false;
        public Boolean desktopInputEnabled = true;
        public String token;
        public int clientVersion;
        public String email;
    }
}
