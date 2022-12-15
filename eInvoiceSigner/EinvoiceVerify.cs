using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using eInvoice.Sign;
using System.IO;
namespace eInvoice.Signer
{
    public partial class EinvoiceVerify : Form
    {
        public EinvoiceVerify()
        {
            InitializeComponent();
        }
        private static string hexadecimalToDecimal(String hexVal)
        {
            int len = hexVal.Length;

            // Initializing base1 value  
            // to 1, i.e 16^0 
            int base1 = 1;

            long dec_val = 0;
            string cc = "";
            // Extracting characters as 
            // digits from last character 
            for (int i = len - 1; i >= 0; i--)
            {
                // if character lies in '0'-'9',  
                // converting it to integral 0-9  
                // by subtracting 48 from ASCII value 
                if (hexVal[i] >= '0' &&
                    hexVal[i] <= '9')
                {
                    cc += (hexVal[i] - 48).ToString() + "x" + base1.ToString()+"+";
                    dec_val += (hexVal[i] - 48) * base1;

                    // incrementing base1 by power 
                    base1 = base1 * 16;
                }

                // if character lies in 'A'-'F' ,  
                // converting it to integral  
                // 10 - 15 by subtracting 55  
                // from ASCII value 
                else if (hexVal[i] >= 'A' &&
                         hexVal[i] <= 'F')
                {
                    cc += (hexVal[i] - 55).ToString() + "x" + base1.ToString() + "+";
                    dec_val += (hexVal[i] - 55) * base1;

                    // incrementing base1 by power 
                    base1 = base1 * 16;
                }
            }
            return cc;
        }
        private static long decimalTo10(String hexVal)
        {
            int len = hexVal.Length;

            // Initializing base1 value  
            // to 1, i.e 16^0 
            int base1 = 1;

            long dec_val = 0;

            // Extracting characters as 
            // digits from last character 
            for (int i = len - 1; i >= 0; i--)
            {
               
                    dec_val += (hexVal[i] - 48) * base1;

                    // incrementing base1 by power 
                    base1 = base1 * 10;
               
            }
            return dec_val;
        } 
        //private static X509Certificate2 GetCertificateBySubject(string CertificateSubject)
        //{
        //    // Check the args. 
        //    if (null == CertificateSubject)
        //        throw new ArgumentNullException("CertificateSubject");


        //    // Load the certificate from the certificate store.
        //    X509Certificate2 cert = null;

        //  //  X509Store store = new X509Store("My", StoreLocation.CurrentUser);
        //    X509Store store = new X509Store(StoreLocation.CurrentUser);

        //    try
        //    {
        //        // Open the store.
        //        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        //        // Get the certs from the store.
        //        X509Certificate2Collection CertCol = store.Certificates;

        //        // Find the certificate with the specified subject. 
        //        foreach (X509Certificate2 c in CertCol)
        //        {
        //            if (c.Subject.ToLower() == CertificateSubject.ToLower())
        //            {
        //                cert = c;
        //                break;
        //            }
        //        }

        //        // Throw an exception of the certificate was not found. 
        //        if (cert == null)
        //        {
        //            throw new CryptographicException("The certificate could not be found.");
        //        }
        //    }
        //    finally
        //    {
        //        // Close the store even if an exception was thrown.
        //        store.Close();
        //    }

        //    return cert;
        //}
        private static X509Certificate2 GetCertificateBySerial(string CertificateSerial)
        {
            // Check the args. 
            if (null == CertificateSerial)
                throw new ArgumentNullException("CertificateSerial");


            // Load the certificate from the certificate store.
            X509Certificate2 cert = null;

            //  X509Store store = new X509Store("My", StoreLocation.CurrentUser);
            X509Store store = new X509Store(StoreLocation.CurrentUser);

            //try
            //{
            // Open the store.
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            // Get the certs from the store.
            X509Certificate2Collection CertCol = store.Certificates;

            // Find the certificate with the specified subject. 
            foreach (X509Certificate2 c in CertCol)
            {
                if (c.GetSerialNumberString().ToLower() == CertificateSerial.ToLower())
                {
                    cert = c;
                    break;
                }
            }

            // Throw an exception of the certificate was not found. 
            if (cert == null)
            {
                throw new CryptographicException("The certificate could not be found.");
            }
            //}
            //finally
            //{
            //    // Close the store even if an exception was thrown.
            //    store.Close();
            //}

            return cert;
        }
        // Verify the signature of an XML file against an asymetric  
        // algorithm and return the result. 
        //private Boolean VerifyXmlFileBySerial(String FileName, String CertificateSerial)
        //{
        //    // Check the args. 
        //    if (null == FileName)
        //        throw new ArgumentNullException("FileName");
        //    if (null == CertificateSerial)
        //        throw new ArgumentNullException("CertificateSerial");

            
        //        X509Certificate2 cert = GetCertificateBySerial(CertificateSerial);
           
        
        //    // Create a new XML document.
        //    XmlDocument xmlDocument = new XmlDocument();

        //    // Load the passed XML file into the document. 
        //    xmlDocument.Load(FileName);

        //    // Create a new SignedXml object and pass it 
        //    // the XML document class.
        //    SignedXml signedXml = new SignedXml(xmlDocument);

        //    // Find the "Signature" node and create a new 
        //    // XmlNodeList object.
        //    XmlNodeList nodeList = xmlDocument.GetElementsByTagName(txtPrefix.Text.Trim() + "Signature");

        //    // Load the signature node.
        //    signedXml.LoadXml((XmlElement)nodeList[0]);

        //    // Check the signature and return the result. 
        //    return signedXml.CheckSignature(cert, false);

        //}
        //private Boolean VerifyXmlFileBySubjectname(String FileName, String CertificateSubjectname)
        //{
        //    // Check the args. 
        //    if (null == FileName)
        //        throw new ArgumentNullException("FileName");
        //    if (null == CertificateSubjectname)
        //        throw new ArgumentNullException("CertificateSubjectname");

           
        //        X509Certificate2 cert = GetCertificateBySubject(CertificateSubjectname);
           
        //    // Create a new XML document.
        //    XmlDocument xmlDocument = new XmlDocument();

        //    // Load the passed XML file into the document. 
        //    xmlDocument.Load(FileName);

        //    // Create a new SignedXml object and pass it 
        //    // the XML document class.
        //    SignedXml signedXml = new SignedXml(xmlDocument);

        //    // Find the "Signature" node and create a new 
        //    // XmlNodeList object.
        //    XmlNodeList nodeList = xmlDocument.GetElementsByTagName(txtPrefix.Text.Trim() + "Signature");

        //    // Load the signature node.
        //    signedXml.LoadXml((XmlElement)nodeList[0]);

        //    // Check the signature and return the result. 
        //    return signedXml.CheckSignature(cert, false);

        //}
        //private Boolean VerifyXmlFileBySubjectname(String FileName, String CertificateSubjectname)
        //{
        //    // Check the args. 
        //    if (null == FileName)
        //        throw new ArgumentNullException("FileName");
        //    if (null == CertificateSubjectname)
        //        throw new ArgumentNullException("CertificateSubjectname");


        //    X509Certificate2 cert = GetCertificateBySubject(CertificateSubjectname);

        //    // Create a new XML document.
        //    XmlDocument xmlDocument = new XmlDocument();

        //    // Load the passed XML file into the document. 
        //    xmlDocument.Load(FileName);

        //    // Create a new SignedXml object and pass it 
        //    // the XML document class.
        //    CryptoConfig.AddAlgorithm(typeof(SigKillXmlDsigExcC14NTransform), "http://www.w3.org/2001/10/xml-exc-c14n#");
        //    var signedXml = new SignedXml(xmlDocument);
        //    var isValid = xmlDocument.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")
        //                        .OfType<XmlElement>()
        //                        .ToArray()
        //                        .All(e =>
        //                        {
        //                            //workaround - remove the signature element here.
        //                            e.ParentNode.RemoveChild(e);
        //                            signedXml.LoadXml(e);
        //                            return signedXml.CheckSignature(cert, true);
        //                        });


        //    //SignedXml signedXml = new SignedXml(xmlDocument);

        //    //// Find the "Signature" node and create a new 
        //    //// XmlNodeList object.
        //    //XmlNodeList nodeList = xmlDocument.GetElementsByTagName(txtPrefix.Text.Trim() + "Signature");

        //    //// Load the signature node.
        //    //signedXml.LoadXml((XmlElement)nodeList[0]);

        //    //// Check the signature and return the result. 
        //    //return signedXml.CheckSignature(cert, false);
        //    return isValid;

        //}
        //private bool TryGetValidDocument(string publicKey, XmlDocument doc)
        //{
        //    var rsa = new RSACryptoServiceProvider();
        //    rsa.FromXmlString(publicKey);

        //    var nsMgr = new XmlNamespaceManager(doc.NameTable);
        //    nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

        //    var signedXml = new SignedXml(doc);
        //    var sig = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsMgr);
        //    if (sig == null)
        //    {
        //        Console.WriteLine("Could not find the signature node");
        //        return false;
        //    }
        //    signedXml.LoadXml(sig);

        //    return signedXml.CheckSignature(rsa);
        //}
        //private bool TryGetValidDocument(X509Certificate2 cert, XmlDocument doc)
        //{
        //    var nsMgr = new XmlNamespaceManager(doc.NameTable);
        //    nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
          
        //    var signedXml = new SignedXml(doc);
        //    var sig = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsMgr);
        //    if (sig == null)
        //    {
        //        Logger.WriteError("Could not find the signature node");
        //        return false;
        //    }
        //    signedXml.LoadXml(sig);
            
        //    return signedXml.CheckSignature(cert,true);
        //}
        ///// <summary>
        ///// Verify the integrity of the signed XML document
        ///// </summary>
        ///// <param name="xmlFilePath">Path to the signed xml document</param>
        ///// <param name="certificate">X509Certificate2 Public key certificate</param>
        ///// <returns></returns>
        //private static bool VerifyXMLDocument(X509Certificate2 certificate,XmlDocument xmldoc)
        //{
        //   // var xmlDocument = ReadXMLDocumentFromPath(xmlFilePath);

        //    var signedXml = new SignedXml(xmldoc);

        //    // Load the XML Signature
        //    var nodeList = xmldoc.GetElementsByTagName("Signature");

        //    signedXml.LoadXml((XmlElement)nodeList[0]);

        //    // Verify the integrity of the xml document
        //    using (var rsaKey = certificate.PublicKey.Key)
        //    {
        //        return signedXml.CheckSignature(rsaKey);
        //    }
        //}
        ///// <summary>
        ///// Verify the integrity of the signed XML document
        ///// </summary>
        ///// <param name="xmlFilePath">Path to the signed xml document</param>
        ///// <param name="certificate">X509Certificate2 Public key certificate</param>
        ///// <returns></returns>
        //private static bool VerifyXMLDocument(string xmlFilePath,X509Certificate2 certificate)
        //{
        //    // var xmlDocument = ReadXMLDocumentFromPath(xmlFilePath);

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.PreserveWhitespace = true;
        //    xmlDocument.Load(xmlFilePath);
          
        //    // string xmlcontents = doc.InnerXml;



        //     var signedXml = new SignedXml(xmlDocument);

        //    // Load the XML Signature
        //    var nodeList = xmlDocument.GetElementsByTagName("Signature");

        //    signedXml.LoadXml((XmlElement)nodeList[0]);

        //    // Verify the integrity of the xml document
        //    using (AsymmetricAlgorithm rsaKey = certificate.PublicKey.Key)
        //    {
        //        return signedXml.CheckSignature(rsaKey);
        //    }
        //}
        private static void SignXMLDocument(X509Certificate2 cert, XmlDocument doc, string signedXMLPath)
        {

            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

            var signedXml = new SignedXml(doc);
            var sig = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsMgr);
           

            if (sig == null)
            {
                Logger.WriteError("Could not find the signature node");

            }
            else
            {
                signedXml.LoadXml(sig);

                var xmlDigitalSignature = signedXml.GetXml();
                XmlElement el = (XmlElement)doc.SelectSingleNode("//HDon/DSCKS/NBan");
                el.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

              //  doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

                doc.Save(signedXMLPath);


             
                
            }
        }
        private void btverify_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            try
            {
               
                if(rdo1.Checked)//verify signature only
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(txtfilepath.Text);
               
                     txtResult.Text =VerifyESignature.XMLVerifyOnly(xmlDoc); //true to verify the signature only; false to verify both the signature and certificate.
                   
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(txtfilepath.Text);

                    txtResult.Text = VerifyESignature.VerifyXmlByPublicKey(xmlDoc);
                 
                }
               
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.ToString();
            }
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtfilepath.Text = openFileDialog1.FileName;

            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (txtfilepath.Text != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(txtfilepath.Text);

                string plant = "_VN" + txtfilepath.Text.Substring(txtfilepath.Text.Length - 6, 2);

                string serial = "";
                try
                {
                    serial = ConfigurationManager.AppSettings[plant].ToString();// ConfigurationManager.AppSettings[plant].ToString();//VN82
                }
                catch
                {
                    serial = ConfigurationManager.AppSettings["_VNDF"].ToString();
                }
                X509Certificate2 x509 = GetCertificateBySerial(serial);
                // SignDoc.SignXML(txtfilepath.Text, x509, "D:\\aaaa22.xml");
                // SignDoc.SignXmlwithKey(txtfilepath.Text, x509, "D:\\aaaa22key.xml");
                //  SignXMLDocument(x509, xmlDoc, "D:\\aaaa.xml");
                SignDoc.SignedXMLWithCertificate(xmlDoc, x509);
                string SignedFile = txtfilepath.Text.ToLower().Replace(".xml", "_Signed_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");
                try
                {
                    xmlDoc.Save(SignedFile);
                    MessageBox.Show("The output file is: " + SignedFile, "Signed Success", MessageBoxButtons.OK);
                }
                catch
                {
                    MessageBox.Show("Can not save file: " + SignedFile, "Signed Fail", MessageBoxButtons.OK);
                }

            }
            else
            {
                MessageBox.Show("Please select XML need to sign", "Certificate error", MessageBoxButtons.OK);
            }
        }

        private void btGetInfoX509_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            txtResult.Text = VerifyESignature.GetX509Info(txtX509.Text);
        }

        private void cmdSetToken_Click(object sender, EventArgs e)
        {
            txtTokenName.Text = "";
            txtSerial.Text = "";
            TokenSign.ClearCert();
            X509Certificate2 x509 = TokenSign.SelectCertificate();
            if (x509 != null)
            {
                txtTokenName.Text = x509.GetName();
                txtSerial.Text = x509.GetSerialNumberString();
            }
           
        }

        private void btSignWToken_Click(object sender, EventArgs e)
        {
            if (txtSerial.Text !="")
            {
                if (txtfilepath.Text != "")
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(txtfilepath.Text);
                    X509Certificate2 x509 = GetCertificateBySerial(txtSerial.Text);
                    SignDoc.SignedXMLWithCertificate(xmlDoc, x509);
                    string SignedFile = txtfilepath.Text.ToLower().Replace(".xml", "_Signed_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");
                    try
                    {
                        xmlDoc.Save(SignedFile);
                        MessageBox.Show("The output file is: " + SignedFile, "Signed Success", MessageBoxButtons.OK);
                    }
                    catch
                    {
                        MessageBox.Show("Can not save file: " + SignedFile, "Signed Fail", MessageBoxButtons.OK);
                    }
                }
                else {
                    MessageBox.Show("Please select XML need to sign", "Certificate error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Please select certificate", "Certificate error", MessageBoxButtons.OK);
            }
        }
        private void SignPdfFileWithDate(string fileFullName, DateTime SignDate)
        {
            string sourcePath = fileFullName;
         
            string errmgs = "";
            try
            {
                string foder=Path.GetDirectoryName(sourcePath);
                string signedfolder = foder + @"\Signed";
                if(System.IO.Directory.Exists(signedfolder)==false)
                {
                    System.IO.Directory.CreateDirectory(signedfolder);
                }
                string targetPath = signedfolder + @"\" + Path.GetFileName(sourcePath);
                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    //Move file to temp folder to process
                  //  errmgs = string.Format("{0} ({1})", " sourcePath = MoveTemp(sourcePath);", sourcePath);

                    if (!string.IsNullOrEmpty(sourcePath))
                    {
                  

                        if (!TokenSign.SignHashedWithDate(sourcePath, targetPath, SignDate))
                        {
                            MessageBox.Show("Sign error");
                        }
                        else
                        {
                            errmgs = string.Format("Signed success {0}", targetPath);
                            MessageBox.Show(errmgs);
                        }
                    }
                    else
                    {
                        Logger.WriteError(errmgs);
                    }
                }


            }
            catch (Exception ex)
            {
                errmgs = string.Format("{0} ({1})", ex.Message, sourcePath);
                MessageBox.Show(errmgs);
            }
            
        }
        private void btSignwithdate_Click(object sender, EventArgs e)
        {
            if (txtfilepath.Text == "")
            {
                MessageBox.Show("Please select pdf file to sign");
            }
            else
            {
                if (dtSign.Value == null)
                {
                    MessageBox.Show("Please select sign date");
                }
                else
                {
                    if (txtSerial.Text == "")
                    {
                        MessageBox.Show("Please select certification");
                    }
                    else
                    {
                        SignPdfFileWithDate(txtfilepath.Text, dtSign.Value);
                    }
                }
            }
        }

        private void EinvoiceVerify_Load(object sender, EventArgs e)
        {
            dtSign.Value = DateTime.Now;
        }
        
    }
}
