using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace DisciplineTradingJournalAPI.Helper
{
    public static class EncryptionDecryptionHelper
    {
        public static string AESEncryption(this string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }

            if (Key == null || Key.Length == 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (IV == null || IV.Length == 0)
            {
                throw new ArgumentNullException("IV");
            }

            byte[] inArray;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(plainText);
                }

                inArray = memoryStream.ToArray();
            }

            return Convert.ToBase64String(inArray);
        }

        public static string AESDecryption(this byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length == 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            if (Key == null || Key.Length == 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (IV == null || IV.Length == 0)
            {
                throw new ArgumentNullException("IV");
            }
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream stream = new MemoryStream(cipherText);
            using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(stream2);
            return streamReader.ReadToEnd();
        }

        public static string SHA512Encrypt(this string inputSource)
        {
            using SHA512 sHA = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputSource);
            return BitConverter.ToString(sHA.ComputeHash(bytes)).Replace("-", string.Empty)?.ToLower();
        }

        public static string MD5Encrypt(this string inputSource)
        {
            using MD5 mD = MD5.Create();
            return BitConverter.ToString(mD.ComputeHash(Encoding.UTF8.GetBytes(inputSource))).Replace("-", string.Empty)?.ToLower();
        }
    }
}
