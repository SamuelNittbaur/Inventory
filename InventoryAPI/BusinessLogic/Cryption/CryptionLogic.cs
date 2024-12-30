using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Cryption
{
    /// <summary>
    /// Enumeration zur Auswahl der Verschlüsselungsmethoden.
    /// </summary>
    public enum CryptionMethod
    {
        secOne,
        secTwo,
        data
    }

    /// <summary>
    /// Bietet Verschlüsselungs- und Entschlüsselungslogik für verschiedene Anwendungen.
    /// </summary>
    public static class CryptionLogic
    {
        //Die folgenden Variablen dürfen so gespeichert werden, da sie nur die Verbindung zwischen Client und Server verschlüsseln
        private static string clientKey = "Sj29jiasask2o9asp12ß";  
        private static string clientSecOneKey = "A8ß92kLmZpQ#s1Roa!?j";
        private static string clientSecTwoKey = "9sAp@ßLo1KZ2&j3qa!R";

        private static byte[] keyUserDataBase = JsonConvert.DeserializeObject<byte[]>(Environment.GetEnvironmentVariable("keyUserDataBase"));
        private static byte[] iVUserDataBase = JsonConvert.DeserializeObject<byte[]>(Environment.GetEnvironmentVariable("ivUserDataBase"));

        /// <summary>
        /// Verschlüsselt eine Eingabezeichenfolge basierend auf einer ausgewählten Verschlüsselungsmethode.
        /// </summary>
        /// <param name="input">Die zu verschlüsselnde Zeichenfolge.</param>
        /// <param name="key">Die ausgewählte <see cref="CryptionMethod"/>.</param>
        /// <returns>Die verschlüsselte Zeichenfolge in Base64-Darstellung.</returns>
        public static string Encrypt(string input, CryptionMethod key)
        {
            string cryptionKey = clientKey;
            if (key == CryptionMethod.secOne)
            {
                cryptionKey = clientSecOneKey;
            }
            else if (key == CryptionMethod.secTwo)
            {
                cryptionKey = clientSecTwoKey;
            }

            if (String.IsNullOrEmpty(input)) return String.Empty;
            else
            {
                byte[] inputBytes = Encoding.UTF32.GetBytes(input);
                byte[] keyBytes = Encoding.UTF32.GetBytes(cryptionKey);

                byte[] encryptedBytes = new byte[inputBytes.Length];

                for (int i = 0; i < inputBytes.Length; i++)
                {
                    encryptedBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
                }
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Entschlüsselt eine verschlüsselte Eingabezeichenfolge basierend auf einer ausgewählten Verschlüsselungsmethode.
        /// </summary>
        /// <param name="encryptedInput">Die verschlüsselte Zeichenfolge in Base64-Darstellung.</param>
        /// <param name="key">Die ausgewählte <see cref="CryptionMethod"/>.</param>
        /// <returns>Die entschlüsselte Zeichenfolge.</returns>
        public static string Decrypt(string encryptedInput, CryptionMethod key)
        {
            string cryptionKey = clientKey;
            if (key == CryptionMethod.secOne)
            {
                cryptionKey = clientSecOneKey;
            }
            else if (key == CryptionMethod.secTwo)
            {
                cryptionKey = clientSecTwoKey;
            }

            if (String.IsNullOrEmpty(encryptedInput)) return String.Empty;
            else
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);
                byte[] keyBytes = Encoding.UTF32.GetBytes(cryptionKey);

                byte[] decryptedBytes = new byte[encryptedBytes.Length];

                for (int i = 0; i < encryptedBytes.Length; i++)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
                }

                return Encoding.UTF32.GetString(decryptedBytes);
            }
        }

        /// <summary>
        /// Verschlüsselt eine Verbindungszeichenfolge mit einem festgelegten Schlüssel.
        /// </summary>
        /// <param name="input">Die zu verschlüsselnde Verbindungszeichenfolge.</param>
        /// <returns>Die verschlüsselte Zeichenfolge in Base64-Darstellung.</returns>
        public static string EncryptConnection(string input)
        {
            if (String.IsNullOrEmpty(input)) return String.Empty;
            else
            {
                byte[] inputBytes = Encoding.UTF32.GetBytes(input);
                byte[] keyBytes = Encoding.UTF32.GetBytes(clientKey);

                byte[] encryptedBytes = new byte[inputBytes.Length];

                for (int i = 0; i < inputBytes.Length; i++)
                {
                    encryptedBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
                }
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Entschlüsselt eine verschlüsselte Verbindungszeichenfolge mit einem festgelegten Schlüssel.
        /// </summary>
        /// <param name="encryptedInput">Die verschlüsselte Zeichenfolge in Base64-Darstellung.</param>
        /// <returns>Die entschlüsselte Zeichenfolge.</returns>
        public static string DecryptConnection(string encryptedInput)
        {
            if (String.IsNullOrEmpty(encryptedInput)) return String.Empty;
            else
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);
                byte[] keyBytes = Encoding.UTF32.GetBytes(clientKey);

                byte[] decryptedBytes = new byte[encryptedBytes.Length];

                for (int i = 0; i < encryptedBytes.Length; i++)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
                }

                return Encoding.UTF32.GetString(decryptedBytes);
            }
        }

        /// <summary>
        /// Verschlüsselt eine Benutzerzeichenfolge mit AES.
        /// </summary>
        /// <param name="plaintext">Die zu verschlüsselnde Zeichenfolge.</param>
        /// <returns>Die verschlüsselte Zeichenfolge in Base64-Darstellung.</returns>
        public static string EncryptUserString(string plaintext)
        {
            try
            {
                byte[] ciphertext = AesEncryption.Encrypt(plaintext, keyUserDataBase, iVUserDataBase);
                string encryptedText = Convert.ToBase64String(ciphertext);
                return encryptedText;
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Entschlüsselt eine Benutzerzeichenfolge mit AES.
        /// </summary>
        /// <param name="encrypted">Die verschlüsselte Zeichenfolge in Base64-Darstellung.</param>
        /// <returns>Die entschlüsselte Zeichenfolge.</returns>
        public static string DecryptUserString(string encrypted)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(encrypted);
                string decryptedText = AesEncryption.Decrypt(bytes, keyUserDataBase, iVUserDataBase);
                return decryptedText;
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Bietet AES-Verschlüsselungs- und Entschlüsselungslogik.
    /// </summary>
    public static class AesEncryption
    {
        /// <summary>
        /// Verschlüsselt eine Zeichenfolge mit AES.
        /// </summary>
        /// <param name="plaintext">Die zu verschlüsselnde Zeichenfolge.</param>
        /// <param name="key">Der AES-Schlüssel.</param>
        /// <param name="iv">Der Initialisierungsvektor (IV) für AES.</param>
        /// <returns>Die verschlüsselten Bytes.</returns>
        public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }

        /// <summary>
        /// Entschlüsselt AES-verschlüsselte Bytes.
        /// </summary>
        /// <param name="ciphertext">Die verschlüsselten Bytes.</param>
        /// <param name="key">Der AES-Schlüssel.</param>
        /// <param name="iv">Der Initialisierungsvektor (IV) für AES.</param>
        /// <returns>Die entschlüsselte Zeichenfolge.</returns>
        public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
