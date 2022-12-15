using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace eInvoice.Signer
{
    
    public class SignDoc
    {
       
        public static void SignedXMLWithCertificate(XmlDocument doc, X509Certificate2 cert)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("sig", "");

            SignedXml signedXML = new SignedXml(doc);
            signedXML.SigningKey = cert.PrivateKey;
            Reference reference = new Reference();
            reference.Uri = "";
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXML.AddReference(reference);

            KeyInfo keyinfo = new KeyInfo();
            keyinfo.AddClause(new KeyInfoX509Data(cert));


           // //add PublicKey
           // RSACryptoServiceProvider rsaprovider = (RSACryptoServiceProvider)cert.PublicKey.Key;
           // RSAKeyValue rkv = new RSAKeyValue(rsaprovider);
           // keyinfo.AddClause(rkv);

            signedXML.KeyInfo = keyinfo;

            XmlElement Objects = doc.CreateElement("Object");
            XmlElement SignatureProperties = doc.CreateElement("SignatureProperties");
            XmlElement SignatureProperty = doc.CreateElement("SignatureProperty");
            XmlElement SigningTime = doc.CreateElement("SigningTime");
            SigningTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SignatureProperty.AppendChild(SigningTime);
            SignatureProperties.AppendChild(SignatureProperty);
            Objects.AppendChild(SignatureProperties);
          //  signedXML.AddObject((DataObject)Objects);
         ////   XmlNodeList elementsToSign = doc.SelectNodes(String.Format("/{0}", Objects));

            System.Security.Cryptography.Xml.DataObject dataSignature =  new System.Security.Cryptography.Xml.DataObject();
            dataSignature.Data = Objects.ChildNodes;
         //   dataSignature.Id = doc.DocumentElement.Name;
            signedXML.AddObject(dataSignature);


            signedXML.ComputeSignature();
            XmlElement xmlsig = signedXML.GetXml();

            //doc.DocumentElement.AppendChild(doc.ImportNode(xmlsig, true));
            
           
         
            doc.SelectSingleNode("/HDon/DSCKS/sig:NBan", nsmgr).AppendChild(doc.ImportNode(xmlsig, true));

        }
    }
    public class VerifyESignature
    {
        public static string XMLVerifyOnly(XmlDocument xmlDoc)
        {
            string result = "";
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
           
            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDoc);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            XmlNode NBan = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan");
           
            XmlNode noteSig = NBan.SelectSingleNode("//sig:Signature", nsmgr);
           
            if (noteSig == null)
            {
               result="Verification failed: No Signature was found in the document.";
            }
           // XmlNode certificate = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan/Signature/KeyInfo/X509Data/X509Certificate");
            XmlNode cer = noteSig.SelectSingleNode("//sig:KeyInfo/sig:X509Data/sig:X509Certificate", nsmgr);
       
            // This example only supports one signature for
           
            signedXml.LoadXml((XmlElement)noteSig);

            // Check the signature and return the result.

            X509Certificate2 dcert2 = new X509Certificate2(Convert.FromBase64String(cer.InnerText));

            string issuedby = dcert2.GetNameInfo(X509NameType.SimpleName, false);

            string sresult = string.Format("{0}Subject: {1}{0}", Environment.NewLine, dcert2.Subject);
            sresult+= string.Format("{0}Issuer: {1}{0}", Environment.NewLine, dcert2.Issuer);
           sresult+= string.Format("{0}Version: {1}{0}", Environment.NewLine, dcert2.Version);
            sresult+= string.Format("{0}Valid Date: {1}{0}", Environment.NewLine, dcert2.NotBefore);
            sresult += string.Format("{0}Expiry Date: {1}{0}", Environment.NewLine, dcert2.NotAfter);
            sresult += string.Format("{0}Thumbprint: {1}{0}", Environment.NewLine, dcert2.Thumbprint);
            sresult += string.Format("{0}Serial Number: {1}{0}", Environment.NewLine, dcert2.SerialNumber);
            sresult += string.Format("{0}Friendly Name: {1}{0}", Environment.NewLine, dcert2.PublicKey.Oid.FriendlyName);
            sresult += string.Format("{0}Public Key Format: {1}{0}", Environment.NewLine, dcert2.PublicKey.EncodedKeyValue.Format(true));
            sresult += string.Format("{0}Raw Data Length: {1}{0}", Environment.NewLine, dcert2.RawData.Length);
            sresult += string.Format("{0}Certificate to string: {1}{0}", Environment.NewLine, dcert2.ToString(true));
            sresult += string.Format("{0}Certificate to XML String: {1}{0}", Environment.NewLine, dcert2.PublicKey.Key.ToXmlString(false));
          //  sresult += string.Format("{0}Certificate Private Key to XML String: {1}{0}", Environment.NewLine, dcert2.PrivateKey.ToXmlString(false));
           
           string subject=  dcert2.Subject;
            string Issuer=dcert2.Issuer;
           string version= dcert2.Version.ToString();
           string ValidDate=dcert2.NotBefore.ToShortDateString();
           string ExpiryDate = dcert2.NotAfter.ToShortDateString();
           string Thumbprint =dcert2.Thumbprint;
            string serial=dcert2.SerialNumber;
            string friend=dcert2.PublicKey.Oid.FriendlyName;
           string publickeyformat=dcert2.PublicKey.EncodedKeyValue.Format(true);
           string dataleng=dcert2.RawData.Length.ToString();
            string certificatestring=dcert2.ToString(true);
            string publickeyxml= dcert2.PublicKey.Key.ToXmlString(false);

         //string xmlpublic=dcert2.PublicKey.Key.ToXmlString(false);
          string resultpulic=  VerifyXmlByPublicKey(xmlDoc, noteSig, publickeyxml);
          result += string.Format("{0}{1}", Environment.NewLine, resultpulic);
            if( signedXml.CheckSignature(dcert2,true)==true)
            {
              //  string issuedby = dcert2.GetNameInfo(X509NameType.SimpleName, false);
                result += string.Format("{0}The XML signature is valid. with {1}", Environment.NewLine, issuedby);
               
                result+=string.Format("{0}Subject: {1}{0}", Environment.NewLine, dcert2.Subject);
                result += string.Format("{0}Issuer: {1}{0}", Environment.NewLine, dcert2.Issuer);
                result += string.Format("{0}Version: {1}{0}", Environment.NewLine, dcert2.Version);
                result+=string.Format("{0}Valid Date: {1}{0}", Environment.NewLine, dcert2.NotBefore);
                result+=string.Format("{0}Expiry Date: {1}{0}", Environment.NewLine, dcert2.NotAfter);
               // result+=string.Format("{0}Thumbprint: {1}{0}", Environment.NewLine, dcert2.Thumbprint);
                result+=string.Format("{0}Serial Number: {1}{0}", Environment.NewLine, dcert2.SerialNumber);
              //  result+=string.Format("{0}Friendly Name: {1}{0}", Environment.NewLine, dcert2.PublicKey.Oid.FriendlyName);
              //  result+=string.Format("{0}Public Key Format: {1}{0}", Environment.NewLine, dcert2.PublicKey.EncodedKeyValue.Format(true));
                //result+=string.Format("{0}Raw Data Length: {1}{0}", Environment.NewLine, dcert2.RawData.Length);
               // result+=string.Format("{0}Certificate to string: {1}{0}", Environment.NewLine, dcert2.ToString(true));
              //  result+=string.Format("{0}Certificate to XML String: {1}{0}", Environment.NewLine, dcert2.PublicKey.Key.ToXmlString(false));
               // result += string.Format("{0}Certificate Private to XML String: {1}{0}", Environment.NewLine, dcert2.PrivateKey.ToXmlString(false));
            }
            else
            {
                result = "The XML signature is NOT valid.";
            }
            return result;
        }
        /// <summary>
        /// string subject = dcert2.Subject;
       /// string Issuer = dcert2.Issuer;
       /// string version = dcert2.Version.ToString();
       /// string ValidDate = dcert2.NotBefore.ToShortDateString();
       /// string ExpiryDate = dcert2.NotAfter.ToShortDateString();
       /// string Thumbprint = dcert2.Thumbprint;
       /// string serial = dcert2.SerialNumber;
       /// string friend = dcert2.PublicKey.Oid.FriendlyName;
       /// string publickeyformat = dcert2.PublicKey.EncodedKeyValue.Format(true);
       /// string dataleng = dcert2.RawData.Length.ToString();
       /// string certificatestring = dcert2.ToString(true);
       /// string publickeyxml = dcert2.PublicKey.Key.ToXmlString(false);
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static X509Certificate2 XMLVerifyOnly(string xmlPath)
        {
            
            // Check arguments.
            X509Certificate2 result;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(xmlPath);
               
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDoc);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            XmlNode NBan = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan");

            XmlNode noteSig = NBan.SelectSingleNode("//sig:Signature", nsmgr);

            if (noteSig == null)
            {
                result = null;
            }
            else
            {
                // XmlNode certificate = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan/Signature/KeyInfo/X509Data/X509Certificate");
                XmlNode cer = noteSig.SelectSingleNode("//sig:KeyInfo/sig:X509Data/sig:X509Certificate", nsmgr);

                // This example only supports one signature for

                signedXml.LoadXml((XmlElement)noteSig);

                // Check the signature and return the result.

                X509Certificate2 dcert2 = new X509Certificate2(Convert.FromBase64String(cer.InnerText));


                if (signedXml.CheckSignature(dcert2, true) == true)
                {
                    result = dcert2;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        public static string VerifyXmlByPublicKey(XmlDocument xmlDoc)
        {
            string sresult = "";
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            XmlNode NBan = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan");

            XmlNode noteSig = NBan.SelectSingleNode("//sig:Signature", nsmgr);
            XmlNode keyinfo = noteSig.SelectSingleNode("//sig:KeyInfo/sig:KeyValue", nsmgr);
            //Adding public key to RSACryptoServiceProvider object.
            RSACryptoServiceProvider RSAVerifier = new RSACryptoServiceProvider();
            
            string xml = keyinfo.InnerXml;
            RSAVerifier.FromXmlString(xml);

                    
            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.LoadXml((XmlElement)noteSig);
            bool result = signedXml.CheckSignature(RSAVerifier);
            if (result)
            {
                sresult = "The XML signature and Certificate are valid.";
            }
            else
            {
                sresult = "The XML signature and Certificate are not valid.";
            }
            return sresult;
        }

        public static string VerifyXmlByPublicKey(XmlDocument xmlDoc,XmlNode SigNode,string xmlStringPublicKey)
        {
            string sresult = "";
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            //nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            //XmlNode NBan = xmlDoc.SelectSingleNode("//HDon/DSCKS/NBan");

            //XmlNode noteSig = NBan.SelectSingleNode("//sig:Signature", nsmgr);
           // XmlNode keyinfo = noteSig.SelectSingleNode("//sig:KeyInfo/sig:KeyValue", nsmgr);
            ////Adding public key to RSACryptoServiceProvider object.
            RSACryptoServiceProvider RSAVerifier = new RSACryptoServiceProvider();

         //   string xml = keyinfo.InnerXml;
            RSAVerifier.FromXmlString(xmlStringPublicKey);

            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.LoadXml((XmlElement)SigNode);
            bool result = signedXml.CheckSignature(RSAVerifier);
            if (result)
            {
                sresult = "The XML signature with publickey is valid.";
            }
            else
            {
                sresult = "The XML signature with publickey is not valid.";
            }
            return sresult;
        }

        public static string GetX509Info(string x509string)
        {
            string sresult = "Can not get the X509 info";
            //X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            //X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            //X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.MultiSelection);
            //Console.WriteLine("Number of certificates: {0}{1}", scollection.Count, Environment.NewLine);
            //foreach (X509Certificate2 x509 in scollection)
            //{
            //    try
            //    {
            //        byte[] rawdata = x509.RawData;
            //    }
            //    catch { }
            //}

           X509Certificate2 dcert2 = new X509Certificate2(Convert.FromBase64String(x509string));
         //   X509Certificate dcert2 = new X509Certificate(Convert.FromBase64String(x509string));
            try
            {
                byte[] rawdata = dcert2.RawData;
                Console.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
                sresult = string.Format("{0}Subject: {1}{0}", Environment.NewLine, dcert2.Subject);
                sresult += string.Format("{0}Issuer: {1}{0}", Environment.NewLine, dcert2.Issuer);
                  sresult += string.Format("{0}Valid Date: {1}{0}", Environment.NewLine, dcert2.NotBefore);
             sresult += string.Format("{0}Expiry Date: {1}{0}", Environment.NewLine, dcert2.NotAfter);
                sresult += string.Format("{0}Thumbprint: {1}{0}", Environment.NewLine, dcert2.Thumbprint);
                sresult += string.Format("{0}Serial Number: {1}{0}", Environment.NewLine, dcert2.SerialNumber);
                 sresult += string.Format("{0}Friendly Name: {1}{0}", Environment.NewLine, dcert2.PublicKey.Oid.FriendlyName);
                 sresult += string.Format("{0}Public Key Format: {1}{0}", Environment.NewLine, dcert2.PublicKey.EncodedKeyValue.Format(true));

                //Console.WriteLine("Friendly Name: {0}{1}", dcert2.FriendlyName, Environment.NewLine);
                //Console.WriteLine("Certificate Verified?: {0}{1}", dcert2.Verify(), Environment.NewLine);
                //Console.WriteLine("Simple Name: {0}{1}", dcert2.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
                //Console.WriteLine("Signature Algorithm: {0}{1}", dcert2.SignatureAlgorithm.FriendlyName, Environment.NewLine);
                //Console.WriteLine("Public Key: {0}{1}", dcert2.PublicKey.Key.ToXmlString(false), Environment.NewLine);
                //Console.WriteLine("Certificate Archived?: {0}{1}", dcert2.Archived, Environment.NewLine);
                //Console.WriteLine("Length of Raw Data: {0}{1}", dcert2.RawData.Length, Environment.NewLine);
                X509Certificate2UI.DisplayCertificate(dcert2);
                dcert2.Reset();
            }
            catch (CryptographicException)
            {
               sresult="Information could not be written out for this certificate.";
            }
         //   sresult = string.Format("{0}Subject: {1}{0}", Environment.NewLine, dcert2.Subject);
         //   sresult += string.Format("{0}Issuer: {1}{0}", Environment.NewLine, dcert2.Issuer);
         ////   sresult += string.Format("{0}Version: {1}{0}", Environment.NewLine, dcert2.Version);
         // //  sresult += string.Format("{0}Valid Date: {1}{0}", Environment.NewLine, dcert2.NotBefore);
         //   sresult += string.Format("{0}Valid Date: {1}{0}", Environment.NewLine, dcert2.GetEffectiveDateString());
         //  // sresult += string.Format("{0}Expiry Date: {1}{0}", Environment.NewLine, dcert2.NotAfter);
         //   sresult += string.Format("{0}Expiry Date: {1}{0}", Environment.NewLine, dcert2.GetExpirationDateString());
         // //  sresult += string.Format("{0}Thumbprint: {1}{0}", Environment.NewLine, dcert2.Thumbprint);
         // //  sresult += string.Format("{0}Serial Number: {1}{0}", Environment.NewLine, dcert2.SerialNumber);
         //   sresult += string.Format("{0}Serial Number: {1}{0}", Environment.NewLine, dcert2.GetSerialNumberString());
         // //  sresult += string.Format("{0}Friendly Name: {1}{0}", Environment.NewLine, dcert2.PublicKey.Oid.FriendlyName);
         // //  sresult += string.Format("{0}Public Key Format: {1}{0}", Environment.NewLine, dcert2.PublicKey.EncodedKeyValue.Format(true));
         //   sresult += string.Format("{0}Public Key Format: {1}{0}", Environment.NewLine, dcert2.GetPublicKeyString());
         //   sresult += string.Format("{0}Raw Data Length: {1}{0}", Environment.NewLine, dcert2.GetRawCertDataString().Length);
         //   sresult += string.Format("{0}Certificate to string: {1}{0}", Environment.NewLine, dcert2.ToString(true));
         // //  sresult += string.Format("{0}Certificate to XML String: {1}{0}", Environment.NewLine, dcert2.PublicKey.Key.ToXmlString(false));
         //   //  sresult += string.Format("{0}Certificate Private Key to XML String: {1}{0}", Environment.NewLine, dcert2.PrivateKey.ToXmlString(false));



            return sresult;
        }
    }
}
