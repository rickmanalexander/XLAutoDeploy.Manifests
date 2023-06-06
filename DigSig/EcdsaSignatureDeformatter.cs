using System.Security.Cryptography;

namespace XLAutoDeploy.Manifests.DigSig
{
    public sealed class EcdsaSignatureDeformatter : AsymmetricSignatureDeformatter
    {
        private ECDsa _key;

        public EcdsaSignatureDeformatter(ECDsa key) => _key = key;

        public override void SetKey(AsymmetricAlgorithm key) => _key = key as ECDsa;
        public override void SetHashAlgorithm(string strName) { }
        public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature) => _key.VerifyHash(rgbHash, rgbSignature);
    }
}
