using XLAutoDeploy.Manifests.Utilities;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml; 

namespace XLAutoDeploy.Manifests.DigSig
{
    public static class XmlSignature
    {
        private const string AlgorithmUri = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";

        static XmlSignature()
        {
            // register custom algorithm
            CryptoConfig.AddAlgorithm(typeof(Ecdsa256SignatureDescription), AlgorithmUri);
        }

        public static void SignXmlDocument(string filePath, X509Certificate2 certificate, bool embedSigniture)
        {
            var xmlDoc = new XmlDocument() { PreserveWhitespace = true };
            xmlDoc.Load(filePath);

            SignXmlDocument(xmlDoc, certificate, embedSigniture);

            xmlDoc.Save(filePath); //overwrites original file
        }

        public static void SignXmlDocument(Stream stream, X509Certificate2 certificate, bool embedSigniture)
        {
            var xmlDoc = new XmlDocument() { PreserveWhitespace = true };
            xmlDoc.Load(stream);

            SignXmlDocument(xmlDoc, certificate, embedSigniture);

            xmlDoc.Save(stream);
        }

        private static void SignXmlDocument(XmlDocument xmlDoc, X509Certificate2 certificate, bool embedSigniture)
        {
            RemoveExistingSignatureNodes(xmlDoc);

            var signedXml = SignXml(xmlDoc.DocumentElement, certificate, AlgorithmUri, embedSigniture);

            xmlDoc.DocumentElement?.AppendChild(signedXml);
        }

        public static XmlElement SignXml(XmlElement xml, X509Certificate2 cert, string signatureMethod, bool embedSigniture)
        {
            // X509Certificate2.PrivateKey is being deprecated
            var key = (AsymmetricAlgorithm)cert.GetRSAPrivateKey() ?? cert.GetECDsaPrivateKey();

            // set key, signing algorithm, and canonicalization method
            var signedXml = new SignedXml(xml) { SigningKey = key };
            signedXml.SignedInfo.SignatureMethod = signatureMethod;
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";

            // Sign the entire document (set the Uri property to "") using "SAML style" transforms
            // This means the entire document is considered for the digest computation, protecting it
            // from (middle-man attacks) tampering.
            var reference = new Reference { Uri = String.Empty };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform()); //A transformation allows the verifier to represent the XML data in the identical manner that the signer used
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            // OPTIONAL: embed the public key in the XML.
            // This isn't trustworthy for validation due to "middle-man" attacks
            if (embedSigniture)
            {
                var keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(cert));
                signedXml.KeyInfo = keyInfo;
            }

            // create signature
            signedXml.ComputeSignature();

            // get signature XML element
            return signedXml.GetXml();
        }


        public static bool ValidateXmlDocumentSignature(string filePath, X509Certificate2 certificate)
        {
            var xmlDoc = new XmlDocument() { PreserveWhitespace = true };
            xmlDoc.Load(filePath);

            return ValidateXmlDocumentSignature(xmlDoc, certificate);
        }

        public static bool ValidateXmlDocumentSignature(Stream stream, X509Certificate2 certificate)
        {
            var xmlDoc = new XmlDocument() { PreserveWhitespace = true };
            xmlDoc.Load(stream);

            return ValidateXmlDocumentSignature(xmlDoc, certificate);
        }

        private static bool ValidateXmlDocumentSignature(XmlDocument xmlDoc, X509Certificate2 certificate)
        {
            ValidateForSignatureElement(xmlDoc); 

            var signiture = xmlDoc.GetElementsByTagName("Signature")[0];

            return ValidateXmlSignature((XmlElement)signiture, certificate);
        }

        public static bool ValidateXmlSignature(XmlElement xml, X509Certificate2 certificate)
        {
            var signedXml = new SignedXml(xml);

            ValidateForSignatureElement(xml.OwnerDocument); 

            return signedXml.CheckSignature((AsymmetricAlgorithm)certificate.GetRSAPublicKey() ?? certificate.GetECDsaPublicKey());
        }

        public static void ValidateForSignatureElement(XmlDocument xmlDoc)
        {
            var signatureElement = xmlDoc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (signatureElement?.Count == 0)
            {
                throw new CryptographicException(Errors.GetFormatedErrorMessage("Attempting to verify a digitally signed xml file.",
                    "No <Signiture> was found in the document.",
                    "Documents must contain a single <Signiture> element."));
            }

            // This example only supports one signature for
            // the entire XML document.
            if (signatureElement?.Count > 1)
            {
                throw new CryptographicException(Errors.GetFormatedErrorMessage("Attempting to verify a digitally signed xml file.",
                    "More that one <Signiture> was found in the document.",
                    "Documents may only contain a single <Signiture> element."));
            }
        }

        // See: https://stackoverflow.com/a/22944248/9743237
        public static X509Certificate2 GetCertificateFromXmlDocument(XmlDocument xmlDoc)
        {
            var certificateValue = xmlDoc.SelectSingleNode("X509Data/X509Certificate").InnerText;
            byte[] data = Convert.FromBase64String(certificateValue);
            return new X509Certificate2(data);
        }

        // public static X509Certificate2 CreateSelfSignedCertificate(string subjectName, int // daysAfterCurrent)
        // {
        //     // ECDSA using P-256 and SHA-256
        //     return CreateSelfSignedCertificate(subjectName, DateTimeOffset.UtcNow, // daysAfterCurrent);
        // }
        // 
        // public static X509Certificate2 CreateSelfSignedCertificate(string subjectName, // DateTimeOffset baseDate, int daysAfter)
        // {
        //     return CreateSelfSignedCertificate(subjectName, baseDate, baseDate.AddDays// (daysAfter));
        // }
        // 
        // public static X509Certificate2 CreateSelfSignedCertificate(string subjectName, // DateTimeOffset notBefore, DateTimeOffset notAfter)
        // {
        //     // ECDSA using P-256 and SHA-256
        //     var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        //     return new CertificateRequest(@"CN=" + subjectName.Replace("CN=",  String.Empty), //ecdsa, HashAlgorithmName.SHA256)
        //         .CreateSelfSigned(notBefore, notAfter);
        // }

        public static void RemoveExistingSignatureNodes(XmlDocument xmlDoc)
        {
            // Remove Existing Signature nodes
            var nodeList = xmlDoc.GetElementsByTagName("Signature");

            if (nodeList?.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    node.RemoveAll();
                }
            }
        }

        public static bool TryGetSignatureElement(XmlDocument xmlDoc, out XmlElement sinatureElement)
        {
            try
            {
                sinatureElement = (XmlElement)xmlDoc.GetElementsByTagName("Signature")[0];
                return true;
            }
            catch
            {
                sinatureElement = null; 
                return false;
            }
        }

        public static bool IsSignedDocument(XmlDocument xmlDoc)
        {
            try
            {
                return xmlDoc.GetElementsByTagName("Signature")?.Count > 0;
            }
            catch
            {
                return false; 
            }
        }
    }
}
