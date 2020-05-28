using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Transcript.Core.Service
{
    /// <summary>
    /// Encrypt/Decrypt string
    /// </summary>
    public class CryptoService
    {
        private static AesManaged _aesManaged;

        public CryptoService()
        {
            if (_aesManaged == null)
                _aesManaged = new AesManaged { BlockSize = 128 };
        }

        public string Encrypt(string plainString, string salt)
        {
            Rfc2898DeriveBytes rfc = GetRfcBytes(salt);
            _aesManaged.Key = _aesManaged.IV = rfc.GetBytes(16);

            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream encrypt = new CryptoStream(stream, _aesManaged.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt
                    byte[] bytes = Encoding.UTF8.GetBytes(plainString);
                    encrypt.Write(bytes, 0, bytes.Length);
                    encrypt.FlushFinalBlock();
                    encrypt.Close();

                    // Return the encrypted data
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedString, string salt)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedString);
            Rfc2898DeriveBytes rfc = GetRfcBytes(salt);
            _aesManaged.Key = _aesManaged.IV = rfc.GetBytes(16);

            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream decrypt = new CryptoStream(stream, _aesManaged.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    // Decrypt
                    decrypt.Write(encryptedData, 0, encryptedData.Length);
                    decrypt.Flush();
                    decrypt.Close();

                    // Return the unencrypted data
                    byte[] decryptedData = stream.ToArray();
                    return Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                }
            }
        }

        private Rfc2898DeriveBytes GetRfcBytes(string salt)
        {

            byte[] saltBytes = new UTF8Encoding().GetBytes(salt);
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(salt, saltBytes);
            return rfc;
        }
    }
}