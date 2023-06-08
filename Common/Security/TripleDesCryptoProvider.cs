/*
 * TPS Confidential
 * All Rights Reserved.
 *
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 *
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Global.AppSettings;
using Microsoft.Win32;

namespace Common.Security
{
    /// <summary>
    /// Provides encryption/decryption services as per IRIS standards.
    /// </summary>
    public static class TripleDesCryptoProvider
    {
        #region Constants
        private const string MasterKey = "IRISWEBSECURITY";
        #endregion

		static string encryptionKey;

        #region Properties

        /// <summary>
        /// Retrieves the encryption key from registery. 
        /// </summary>
        /// <returns>Encrytion key for double encryption</returns>
        public static string EncryptionKey
        {
			get
			{
				if (string.IsNullOrEmpty(encryptionKey))
					encryptionKey = AppSettingKeys.EncryptionKey;
				return encryptionKey;
			}
			set
			{
				encryptionKey = value;
			}
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Decrypts the string using the provided key.
        /// </summary>
        /// <param name="key">Key used for decryption.</param>
        /// <param name="cipherString">Encrypted text</param>
        /// <param name="useHashing">Specifies if the algorithm needs to use hashing.</param>
        /// <returns>Decrypted string.</returns>
        private static string DoDecrypt(string key, string cipherString, bool useHashing)
        {
            byte[] keyArray;

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Encrypts the string using the provided key.
        /// </summary>
        /// <param name="key">Key to be used for encryption.</param>
        /// <param name="toEncrypt">Clear text to be encrypted.</param>
        /// <param name="useHashing">Flag to toggle hashing in algorithm.</param>
        /// <returns>Encrypted string.</returns>
        private static string DoEncrypt(string key, string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Encryption using the customKey. Calls DoEncrypt directly.
        /// </summary>
        /// <param name="key">Key to be used for encryption.</param>
        /// <param name="plainText">Clear text.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Encrypted string.</returns>
        public static string EncryptUsingCustomKey(string key, string plainText, bool useMd5Hash)
        {
            return DoEncrypt(key, plainText, useMd5Hash);
        }

        /// <summary>
        /// Encryption using the master key.
        /// </summary>
        /// <param name="plainText">Clear text that is to be encrypted.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Encrypted string.</returns>
        public static string EncryptUsingMasterKey(string plainText, bool useMd5Hash)
        {
            return DoEncrypt(MasterKey, plainText, useMd5Hash);
        }

        /// <summary>
        /// Encrypts the given cipher text using double encryption.
        /// </summary>
        /// <param name="plainText">Plain text to be encrypted.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText, bool useMd5Hash)
        {
            // Encrypt the encrytion key using the Master Key:
            string plainEncryptionKey = DoDecrypt(MasterKey, EncryptionKey, true);

            // Encrypt the plain text using the clear encryption key (obtained in the previous step):
            string cipherText = DoEncrypt(plainEncryptionKey, plainText, true);

            // Return the cipher text:
            return cipherText;
        }

        /// <summary>
        /// Decryption using the custom key.
        /// </summary>
        /// <param name="key">Key to be used for decryption.</param>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Clear text.</returns>
        public static string DecryptUsingCustomKey(string key, string cipherText, bool useMd5Hash)
        {
            return DoDecrypt(key, cipherText, useMd5Hash);
        }

        /// <summary>
        /// Decrypt using the master key.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Clear text.</returns>
        public static string DecryptUsingMasterKey(string cipherText, bool useMd5Hash)
        {
            return DoDecrypt(MasterKey, cipherText, useMd5Hash);
        }

        /// <summary>
        /// Decrypts the given cipher text using double encryption.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="useMd5Hash">Flag to toggle MD5 hashing.</param>
        /// <returns>Clear text.</returns>
        public static string Decrypt(string cipherText, bool useMd5Hash)
        {
            string plainEncryptionKey = "";
            string plainText = "";
            try
            {
                // Decrypt the encrytion key using the Master Key:
                plainEncryptionKey = DoDecrypt(MasterKey, EncryptionKey, true);

                // Decrypt the plain text using the clear encryption key (obtained in the previous step):
                plainText = DoDecrypt(plainEncryptionKey, cipherText, true);
            }
            catch (Exception)
            {
                if (plainEncryptionKey == "")
                {
                    plainText = DoDecrypt(MasterKey, cipherText, true);
                    return plainText;
                }
                throw;
            }
            // Return the plain text:
            return plainText;
        }
        #endregion
    }
}
