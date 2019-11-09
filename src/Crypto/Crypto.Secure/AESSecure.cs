using System;
using System.IO;
using System.Security.Cryptography;
using Crypto.Core;

namespace Crypto.Secure
{
    /// <summary>
    /// This class implements the ISecure interface
    /// </summary>
    public class AESSecure : ISecure, IDisposable
    {
        private Aes _aes;

        public AESSecure()
        {
            _aes = Aes.Create();
        }

        public void Dispose()
        {
            _aes.Dispose();
        }

        /// <summary>
        /// Decrypt the specified text generated from the key and initialization- 
        /// vector (IV) generated while the AESSecure object was first created. 
        /// The KEY and IV created on the start-up can be used for the life-time
        /// of the application
        /// </summary>
        /// <returns>Decrypted information from the input text</returns>
        /// <param name="cipherText">An encrypted string encoded with Base64</param>
        public string Decrypt(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _aes.Key;
                aesAlg.IV = _aes.IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Base64Decode(cipherText)))
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

        /// <summary>
        /// Encrypt the text based on the key and initialization vector(IV) 
        /// generated on startup of this class
        /// </summary>
        /// <returns>The encrypt.</returns>
        /// <param name="plainText">Plain text.</param>
        public string Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _aes.Key;
                aesAlg.IV = _aes.IV;

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
            return Base64Encode(encrypted);
        }

        /// <summary>
        /// Convert byte array to Base64 encoded text
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="plainTextBytes">Plain text bytes.</param>
        private string Base64Encode(byte[] plainTextBytes)
        {
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Convert Base64 ecoded text to byte array
        /// </summary>
        /// <returns>The decode.</returns>
        /// <param name="base64EncodedData">Base64 encoded data.</param>
        private byte[] Base64Decode(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }
    }
}
