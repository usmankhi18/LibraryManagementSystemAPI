/*
 * TPS Confidential
 * All Rights Reserved.
 *
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 *
 */

using System;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace Common.Security
{
    /// <summary>
    /// Provides helper functions for MD5Hashing.
    /// </summary>
    public class HashingHelper
    {
        public enum HashingAlgorithm
        {
            MD5,
            SHA1,
            SHA2,
            HMACMD5,
            HMACSHA1,
            HMACSHA2,
            HMACSHA256
        }

        #region Constructor
        public HashingHelper()
        {
        }
        #endregion

        #region Conversion Function
        /// <summary>
        /// Converts a given byte array to string.
        /// </summary>
        /// <param name="arrInput">Byte array to be converted to string.</param>
        /// <returns>String value after conversion.</returns>
        private static string ByteArrayToString(byte[] arrInput, bool fullLengthPassword = false)
        {
            int i;
            StringBuilder output = new StringBuilder(arrInput.Length);

            if (fullLengthPassword)
            {
                for (i = 0; i < arrInput.Length; i++)
                    output.Append(arrInput[i].ToString("X2"));
            }
            else
            {
                for (i = 0; i < arrInput.Length - 1; i++)
                    output.Append(arrInput[i].ToString("X2"));
            }

            return output.ToString();
        }

        #endregion

        #region Generate Hash
        /// <summary>
        /// Creates a hash value for the provided string.
        /// </summary>
        /// <param name="messageString">Message to be converted to hash.</param>
        /// <returns>Hashed value.</returns>
        public static string GenerateHash(string messageString, HashingAlgorithm algo = HashingAlgorithm.MD5, bool fullLengthPassword = false,string clearPassword="",string salt="")
        {

            byte[] originalMessage;
            byte[] hashedMessage = null;
            byte[] key;

            originalMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(messageString);
            key = System.Text.ASCIIEncoding.ASCII.GetBytes(salt);

            switch (algo)
            {
                case HashingAlgorithm.MD5:
                    hashedMessage = new MD5CryptoServiceProvider().ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.SHA1:
                    hashedMessage = new SHA1CryptoServiceProvider().ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.SHA2:
                    hashedMessage = new SHA512CryptoServiceProvider().ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.HMACMD5:
                    originalMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(clearPassword);
                    hashedMessage = new HMACMD5(key).ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.HMACSHA1:
                    originalMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(clearPassword);
                    hashedMessage = new HMACSHA1(key).ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.HMACSHA2:
                    originalMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(clearPassword);
                    hashedMessage = new HMACSHA512(key).ComputeHash(originalMessage);
                    break;
                case HashingAlgorithm.HMACSHA256:
                    originalMessage = System.Text.ASCIIEncoding.ASCII.GetBytes(clearPassword);
                    hashedMessage = new HMACSHA256(key).ComputeHash(originalMessage);
                    break;
            }

            string strHash = ByteArrayToString(hashedMessage, fullLengthPassword);
            return strHash;
        }
        #endregion

        #region Verify Hash
        /// <summary>
        /// Compares provided hashed values for equality.
        /// </summary>
        /// <param name="compareHashMassage">Hash value.</param>
        /// <param name="sentHashMessage">Hash value to be compared to first value.</param>
        /// <returns>True if the hashes are equal.</returns>
        public static bool VerifyHash(string compareHashMassage, string sentHashMessage)
        {

            if (compareHashMassage.CompareTo(sentHashMessage) == 0)
                return true;

            return false;
        }
        #endregion

        #region MessageEncryption
        public static string Encrypt(string toEncrypt, string key, bool useHashing)
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

            tdes.Key = keyArray;

            tdes.Mode = CipherMode.ECB;


            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, string key ,bool useHashing)
        {
            if (string.IsNullOrEmpty(cipherString) || string.IsNullOrEmpty(key))
            {
                return cipherString;
            }
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region Generate256Hash
       public static string Generate256Hash(string message)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(message));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        #endregion

        #region GenerateSaltValue for Hash

        public static string GenerateHashSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
           return Convert.ToBase64String(buff);
        }

        #endregion
    }
}