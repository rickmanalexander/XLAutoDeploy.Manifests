using System.Security.Cryptography;

namespace XLAutoDeploy.Manifests.DigSig
{
    public sealed class Ecdsa256SignatureDescription : SignatureDescription
    {
        public const int RequiredKeySize = 256; 
        public Ecdsa256SignatureDescription()
        {
            KeyAlgorithm = typeof(ECDsa).AssemblyQualifiedName;
        }

        public override HashAlgorithm CreateDigest() => SHA256.Create();

        public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            if (!(key is ECDsa ecdsa) || ecdsa.KeySize != RequiredKeySize)
                throw new CryptographicException($"Requires EC key using P-{RequiredKeySize}");

            return new DSASignatureFormatter(ecdsa);
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            if (!(key is ECDsa ecdsa) || ecdsa.KeySize != RequiredKeySize)
                throw new CryptographicException($"Requires EC key using P-{RequiredKeySize}");

            return new EcdsaSignatureDeformatter(ecdsa);
        }
    }
}
