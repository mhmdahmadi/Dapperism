using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dapperism.Extensions.Cryptography
{
    internal static class Hash
    {
        private static FileStream GetFileStream(string pathName)
        {
            return (new FileStream(pathName, FileMode.Open,
                      FileAccess.Read, FileShare.ReadWrite));
        }

        internal static string ComputeCRC32Checksum(object data)
        {
            string crc32Str = null;
            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    crc32Str = Crc32Algorithm.GetCrc32(bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    crc32Str = Crc32Algorithm.GetCrc32(fileStream);
            }

            if (data is string)
                crc32Str = Crc32Algorithm.GetCrc32((string)data);

            return crc32Str;
        }

        internal static string ComputeMD5Checksum(object data)
        {
            var md5Str = new StringBuilder();
            var md5Bytes = new byte[] { };
            var md5Crypto = new MD5CryptoServiceProvider();

            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    md5Bytes = md5Crypto.ComputeHash(bytes);
            }

            if (data is string)
            {
                md5Bytes = Encoding.UTF8.GetBytes((string)data);
                md5Bytes = md5Crypto.ComputeHash(md5Bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    md5Bytes = md5Crypto.ComputeHash(fileStream);
            }
            foreach (var b in md5Bytes)
                md5Str.Append(b.ToString("x2").ToLower());
            return md5Str.ToString();
        }

        internal static string ComputeSHA1Checksum(object data)
        {
            var sha1Str = new StringBuilder();
            var sha1Bytes = new byte[] { };
            var sha1Crypto = new SHA1CryptoServiceProvider();

            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    sha1Bytes = sha1Crypto.ComputeHash(bytes);
            }

            if (data is string)
            {
                sha1Bytes = Encoding.UTF8.GetBytes((string)data);
                sha1Bytes = sha1Crypto.ComputeHash(sha1Bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    sha1Bytes = sha1Crypto.ComputeHash(fileStream);
            }
            foreach (var b in sha1Bytes)
                sha1Str.Append(b.ToString("x2").ToLower());
            return sha1Str.ToString();
        }
        internal static string ComputeSHA256Checksum(object data)
        {
            var sha256Str = new StringBuilder();
            var sha256Bytes = new byte[] { };
            var sha256Crypto = new SHA256CryptoServiceProvider();

            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    sha256Bytes = sha256Crypto.ComputeHash(bytes);
            }

            if (data is string)
            {
                sha256Bytes = Encoding.UTF8.GetBytes((string)data);
                sha256Bytes = sha256Crypto.ComputeHash(sha256Bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    sha256Bytes = sha256Crypto.ComputeHash(fileStream);
            }
            foreach (var b in sha256Bytes)
                sha256Str.Append(b.ToString("x2").ToLower());
            return sha256Str.ToString();
        }
        internal static string ComputeSHA384Checksum(object data)
        {
            var sha384Str = new StringBuilder();
            var sha384Bytes = new byte[] { };
            var sha384Crypto = new SHA384CryptoServiceProvider();

            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    sha384Bytes = sha384Crypto.ComputeHash(bytes);
            }

            if (data is string)
            {
                sha384Bytes = Encoding.UTF8.GetBytes((string)data);
                sha384Bytes = sha384Crypto.ComputeHash(sha384Bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    sha384Bytes = sha384Crypto.ComputeHash(fileStream);
            }
            foreach (var b in sha384Bytes)
                sha384Str.Append(b.ToString("x2").ToLower());
            return sha384Str.ToString();
        }
        internal static string ComputeSHA512Checksum(object data)
        {
            var sha512Str = new StringBuilder();
            var sha512Bytes = new byte[] { };
            var sha512Crypto = new SHA512CryptoServiceProvider();

            if (data is byte[])
            {
                var bytes = data as byte[];
                if (bytes != null)
                    sha512Bytes = sha512Crypto.ComputeHash(bytes);
            }

            if (data is string)
            {
                sha512Bytes = Encoding.UTF8.GetBytes((string)data);
                sha512Bytes = sha512Crypto.ComputeHash(sha512Bytes);
            }

            if (data is FileStream)
            {
                var fileStream = data as FileStream;
                if (fileStream != null)
                    sha512Bytes = sha512Crypto.ComputeHash(fileStream);
            }
            foreach (var b in sha512Bytes)
                sha512Str.Append(b.ToString("x2").ToLower());
            return sha512Str.ToString();
        }
    }
}
