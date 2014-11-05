using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dapperism.Extensions.Cryptography
{
    internal class TripleDesCrypto
    {
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            var ms = new MemoryStream();
            var alg = TripleDES.Create();
            alg.Key = Key;
            alg.IV = IV;
            var cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            var encryptedData = ms.ToArray();
            return encryptedData;
        }

        internal static string Encrypt(string clearText, string password)
        {
            var clearBytes =
              Encoding.Unicode.GetBytes(clearText);

            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            var encryptedData = Encrypt(clearBytes,
                     pdb.GetBytes(24), pdb.GetBytes(8));

            return Convert.ToBase64String(encryptedData);

        }

        internal static byte[] Encrypt(byte[] clearData, string password)
        {
            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            return Encrypt(clearData, pdb.GetBytes(24), pdb.GetBytes(8));

        }

        internal static void Encrypt(string fileIn,
                    string fileOut, string password)
        {
            var fsIn = new FileStream(fileIn,
                FileMode.Open, FileAccess.Read);
            var fsOut = new FileStream(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);
            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            var alg = TripleDES.Create();
            alg.Key = pdb.GetBytes(24);
            alg.IV = pdb.GetBytes(8);
            var cs = new CryptoStream(fsOut,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            var bufferLen = 4096;
            var buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
            cs.Close();
            fsIn.Close();
        }

        private static byte[] Decrypt(byte[] cipherData,
                                      byte[] Key, byte[] IV)
        {
            var ms = new MemoryStream();
            var alg = TripleDES.Create();

            alg.Key = Key;
            alg.IV = IV;

            var cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherData, 0, cipherData.Length);

            cs.Close();

            var decryptedData = ms.ToArray();

            return decryptedData;
        }

        internal static string Decrypt(string cipherText, string password)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            var decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(24), pdb.GetBytes(8));
            return Encoding.Unicode.GetString(decryptedData);
        }

        internal static byte[] Decrypt(byte[] cipherData, string password)
        {
            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            return Decrypt(cipherData, pdb.GetBytes(24), pdb.GetBytes(8));
        }

        internal static void Decrypt(string fileIn,
                    string fileOut, string password)
        {
            var fsIn = new FileStream(fileIn,
                        FileMode.Open, FileAccess.Read);
            var fsOut = new FileStream(fileOut,
                        FileMode.OpenOrCreate, FileAccess.Write);

            var pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            var alg = TripleDES.Create();
            alg.Key = pdb.GetBytes(24);
            alg.IV = pdb.GetBytes(8);
            var cs = new CryptoStream(fsOut,
                alg.CreateDecryptor(), CryptoStreamMode.Write);
            var bufferLen = 4096;
            var buffer = new byte[bufferLen];
            int bytesRead;
            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                cs.Write(buffer, 0, bytesRead);

            } while (bytesRead != 0);
            cs.Close();
            fsIn.Close();
        }
    }
}
