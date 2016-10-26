using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GameController
{
    public class SettingsData
    {
        public Boolean heightsEnabled = false;
        public Boolean satelliteOverlayEnabled = false;
        public Boolean handheldInputEnabled = false;
        public Boolean desktopInputEnabled = true;
        public String token;
        public int clientVersion;
        public String email;
    }

    public static class SecurePreferences
    {

        /// <summary>
        /// Encrypts string with the key derived from device ID
        /// </summary>
        /// <returns>Base64 encoded encrypted data</returns>
        /// <param name="stringToEncrypt">String to encrypt</param>
        public static string EncryptStringified(string stringToEncrypt)
        {
            if (stringToEncrypt == null)
                throw new ArgumentNullException("stringToEncrypt");

            byte[] key = DeviceIdToDesKey();
            byte[] plainData = Encoding.UTF8.GetBytes(stringToEncrypt);
            byte[] encryptedData = Encrypt(key, plainData);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts Base64 encoded data with the key derived from device ID
        /// </summary>
        /// <returns>Decrypted string</returns>
        /// <param name="b64DataToDecrypt">Base64 encoded data to decrypt</param>
        public static string DecryptStringified(string b64DataToDecrypt)
        {
            if (b64DataToDecrypt == null)
                throw new ArgumentNullException("b64DataToDecrypt");

            byte[] key = DeviceIdToDesKey();
            byte[] encryptedData = Convert.FromBase64String(b64DataToDecrypt);
            try
            {
                byte[] decryptedData = Decrypt(key, encryptedData);
                return Encoding.UTF8.GetString(decryptedData);
            }
#pragma warning disable 0168
            catch (CryptographicException e)
            {
                //Debug.LogError(e);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }

        }

        private static byte[] DeviceIdToDesKey()
        {
            string deviceId = "UNAVAILABLEUNAVAILABLEUNAVAILABLEUNAVAILABLE";
            if (SystemInfo.deviceUniqueIdentifier != null)
                deviceId = SystemInfo.deviceUniqueIdentifier;
            // Compute hash of device ID so we are sure enough bytes have been gathered for the key
            byte[] bytes = null;
            using (SHA256Managed sha1 = new SHA256Managed())
                bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(deviceId));
            // Get last 8 bytes from device ID hash as a key
            byte[] desKey = new byte[8];
            Array.Copy(bytes, bytes.Length - desKey.Length, desKey, 0, desKey.Length);
            return desKey;
        }

        private static byte[] Encrypt(byte[] key, byte[] plainData)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (plainData == null)
                throw new ArgumentNullException("plainData");

            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                if (!desProvider.ValidKeySize(key.Length * 8))
                    throw new CryptographicException("Key with invalid size has been specified");
                desProvider.Key = key;
                // desProvider.IV should be automatically filled with random bytes when DESCryptoServiceProvider instance is created
                desProvider.Mode = CipherMode.CBC;
                desProvider.Padding = PaddingMode.PKCS7;

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    // Write IV at the beginning of memory stream
                    encryptedStream.Write(desProvider.IV, 0, desProvider.IV.Length);

                    // Perform encryption and append encrypted data to the memory stream
                    using (ICryptoTransform encryptor = desProvider.CreateEncryptor())
                    {
                        byte[] encryptedData = encryptor.TransformFinalBlock(plainData, 0, plainData.Length);
                        encryptedStream.Write(encryptedData, 0, encryptedData.Length);
                    }

                    return encryptedStream.ToArray();
                }
            }
        }

        private static byte[] Decrypt(byte[] key, byte[] encryptedData)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (encryptedData == null)
                throw new ArgumentNullException("encryptedData");

            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                if (!desProvider.ValidKeySize(key.Length * 8))
                    throw new CryptographicException("Key with invalid size has been specified");
                desProvider.Key = key;
                if (encryptedData.Length <= desProvider.IV.Length)
                    throw new CryptographicException("Too short encrypted data has been specified");
                // Read IV from the beginning of encrypted data
                // Note: New byte array needs to be created because data written to desprovider.IV are ignored
                byte[] iv = new byte[desProvider.IV.Length];
                Array.Copy(encryptedData, 0, iv, 0, iv.Length);
                desProvider.IV = iv;
                desProvider.Mode = CipherMode.CBC;
                desProvider.Padding = PaddingMode.PKCS7;

                // Remove IV from the beginning of encrypted data and perform decryption
                using (ICryptoTransform decryptor = desProvider.CreateDecryptor())
                    return decryptor.TransformFinalBlock(encryptedData, desProvider.IV.Length, encryptedData.Length - desProvider.IV.Length);
            }
        }

    }
}
