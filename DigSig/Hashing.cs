using System;
using System.IO;
using System.Security.Cryptography;

namespace XLAutoDeploy.Manifests.DigSig
{
    public static class Hashing
    {
        public static HashAlgorithm CreateHashAlgorithm(SecureHashAlgorithm algorithm)
        {
            var hashName = Enum.GetName(typeof(SecureHashAlgorithm), algorithm);

            return HashAlgorithm.Create(hashName);
        }

        public static byte[] ComputeHash(HashAlgorithm algorithm, string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return algorithm.ComputeHash(stream);
            }
        }

        public static byte[] ComputeHash(HashAlgorithm algorithm, byte[] bytes)
        {
            return algorithm.ComputeHash(bytes);
        }
    }
}
