using System.Text;

namespace Logic.Cryption
{

    public enum CryptionMethod
    {
        secOne,
        secTwo,
        data
    }
    public static class CryptionLogic
    {
        private static string clientKey = "Sj29jiasask2o9asp12ß";
        private static string clientSecOneKey = "A8ß92kLmZpQ#s1Roa!?j";
        private static string clientSecTwoKey = "9sAp@ßLo1KZ2&j3qa!R";
        public static string Encrypt(string input, CryptionMethod key = CryptionMethod.data)
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

        public static string Decrypt(string encryptedInput, CryptionMethod key = CryptionMethod.data)
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

        //public static string EncryptConnection(string input)
        //{

        //    if (String.IsNullOrEmpty(input)) return String.Empty;
        //    else
        //    {
        //        byte[] inputBytes = Encoding.UTF32.GetBytes(input);
        //        byte[] keyBytes = Encoding.UTF32.GetBytes(clientConnectionKey);

        //        byte[] encryptedBytes = new byte[inputBytes.Length];

        //        for (int i = 0; i < inputBytes.Length; i++)
        //        {
        //            encryptedBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
        //        }
        //        return Convert.ToBase64String(encryptedBytes);
        //    }

        //}

        //public static string DecryptConnection(string encryptedInput)
        //{

        //    if (String.IsNullOrEmpty(encryptedInput)) return String.Empty;
        //    else
        //    {
        //        byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);
        //        byte[] keyBytes = Encoding.UTF32.GetBytes(clientConnectionKey);

        //        byte[] decryptedBytes = new byte[encryptedBytes.Length];

        //        for (int i = 0; i < encryptedBytes.Length; i++)
        //        {
        //            decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
        //        }

        //        return Encoding.UTF32.GetString(decryptedBytes);
        //    }

        //}

        public static (string, string) CreateSecurityHeader()
        {
            string headerKey = Encrypt(DateTime.Now.ToString("dd/MM/yyyy"), CryptionMethod.secTwo);
            string headerValue = Encrypt($"{Guid.NewGuid()}?{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}", CryptionMethod.secOne);

            return (headerKey, headerValue);
        }
    }
}
