using System.IO;
using Dapperism.Extensions.Cryptography;

namespace Dapperism.Extensions.Extensions
{
    public static class CryptoExt
    {

        public static string EncryptToRijndael(this string str, string key)
        {
            return RijndaelCrypto.Encrypt(str, key);
        }

        public static string EncryptToTripleDes(this string str, string key)
        {
            return TripleDesCrypto.Encrypt(str, key);
        }

        public static string DecryptToRijndael(this string str, string key)
        {
            return RijndaelCrypto.Decrypt(str, key);
        }

        public static string DecryptToTripleDes(this string str, string key)
        {
            return TripleDesCrypto.Decrypt(str, key);
        }

        public static byte[] EncryptToRijndael(this byte[] bytes, string key)
        {
            return RijndaelCrypto.Encrypt(bytes, key);
        }

        public static byte[] EncryptToTripleDes(this byte[] bytes, string key)
        {
            return TripleDesCrypto.Encrypt(bytes, key);
        }

        public static byte[] DecryptToRijndael(this byte[] bytes, string key)
        {
            return RijndaelCrypto.Decrypt(bytes, key);
        }

        public static byte[] DecryptToTripleDes(this byte[] bytes, string key)
        {
            return TripleDesCrypto.Decrypt(bytes, key);
        }

        public static void EncryptToRijndael(this string inputFile, string outputFile, string key)
        {
            RijndaelCrypto.Encrypt(inputFile, outputFile, key);
        }

        public static void EncryptToTripleDes(this string inputFile, string outputFile, string key)
        {
            TripleDesCrypto.Encrypt(inputFile, outputFile, key);
        }

        public static void DecryptToRijndael(this string inputFile, string outputFile, string key)
        {
            RijndaelCrypto.Decrypt(inputFile, outputFile, key);
        }

        public static void DecryptToTripleDes(this string inputFile, string outputFile, string key)
        {
            TripleDesCrypto.Decrypt(inputFile, outputFile, key);
        }



        public static string GetMd5Hash(this string str)
        {
            return Hash.ComputeMD5Checksum(str);
        }
        public static string GetMd5Hash(this byte[] bytes)
        {
            return Hash.ComputeMD5Checksum(bytes);
        }
        public static string GetMd5Hash(this Stream stream)
        {
            return Hash.ComputeMD5Checksum(stream);
        }

        public static string GetCrc32Hash(this string str)
        {
            return Hash.ComputeCRC32Checksum(str);
        }
        public static string GetCrc32Hash(this byte[] bytes)
        {
            return Hash.ComputeCRC32Checksum(bytes);
        }
        public static string GetCrc32Hash(this Stream stream)
        {
            return Hash.ComputeCRC32Checksum(stream);
        }

        public static string GetSha1Hash(this string str)
        {
            return Hash.ComputeSHA1Checksum(str);
        }
        public static string GetSha1Hash(this byte[] bytes)
        {
            return Hash.ComputeSHA1Checksum(bytes);
        }
        public static string GetSha1Hash(this Stream stream)
        {
            return Hash.ComputeSHA1Checksum(stream);
        }

        public static string GetSha256Hash(this string str)
        {
            return Hash.ComputeSHA256Checksum(str);
        }
        public static string GetSha256Hash(this byte[] bytes)
        {
            return Hash.ComputeSHA256Checksum(bytes);
        }
        public static string GetSha256Hash(this Stream stream)
        {
            return Hash.ComputeSHA256Checksum(stream);
        }

        public static string GetSha384Hash(this string str)
        {
            return Hash.ComputeSHA384Checksum(str);
        }
        public static string GetSha384Hash(this byte[] bytes)
        {
            return Hash.ComputeSHA384Checksum(bytes);
        }
        public static string GetSha384Hash(this Stream stream)
        {
            return Hash.ComputeSHA384Checksum(stream);
        }

        public static string GetSha512Hash(this string str)
        {
            return Hash.ComputeSHA512Checksum(str);
        }
        public static string GetSha512Hash(this byte[] bytes)
        {
            return Hash.ComputeSHA512Checksum(bytes);
        }
        public static string GetSha512Hash(this Stream stream)
        {
            return Hash.ComputeSHA512Checksum(stream);
        }

    }
}
