using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Whispr.Client.Components
{
    public class EncryptionService
    {
        private static RSACryptoServiceProvider rsakeypair = new RSACryptoServiceProvider();

        public EncryptionService() { }

        public string GetRSAPublicKey()
        {
            return rsakeypair.ToXmlString(false);
        }

        // Encrypts plain text using a public RSA key
        // Using OAEP padding
        // Returns base64 string
        public string RSAEncrypt(string PlainText, RSACryptoServiceProvider PublicKey)
        {
            byte[] encryptedData;
            encryptedData = PublicKey.Encrypt(Encoding.UTF8.GetBytes(PlainText), true);
            return Convert.ToBase64String(encryptedData);
        }

        // Decrypts RSA Encrypted text data from base64 string
        // Using OAEP padding
        // Returns plain text
        public string RSADecrypt(string Base64Text)
        {
            byte[] encryptedData = Convert.FromBase64String(Base64Text);
            string PlainText = Encoding.UTF8.GetString(rsakeypair.Decrypt(encryptedData, true));
            return PlainText;
        }

        // Returns the Encrypted Text (base64 encoded), the key (base64 encoded), and the Initialization Vector AKA Salt (base64 encoded)
        public Tuple<string, string, string> AESEncrypt(string PlainText)
        {
            AesManaged aes = new AesManaged();
            aes.KeySize = 256;
            aes.GenerateIV();
            aes.GenerateKey();

            byte[] EncryptedText = EncryptStringToBytes_Aes(PlainText, aes.Key, aes.IV);

            return new Tuple<String, String, String>(Convert.ToBase64String(EncryptedText), Convert.ToBase64String(aes.Key), Convert.ToBase64String(aes.IV));
        }

        // Decryptes Encrypted Base64 encoded text
        public string AESDecrypt(string Base64Text, string Key, string InitializationVector)
        {
            return DecryptStringFromBytes_Aes(Convert.FromBase64String(Base64Text), Convert.FromBase64String(Key), Convert.FromBase64String(InitializationVector));
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public string RSAEncrypt(string plaintext, object p)
        {
            throw new NotImplementedException();
        }
    }
}
