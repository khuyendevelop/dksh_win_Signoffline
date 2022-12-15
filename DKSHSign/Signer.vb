Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Org.BouncyCastle.X509
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Pkcs
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms
Imports System.IO.Packaging

Public Class TokenSign

    Private Shared _Certificate As X509Certificate2
    Private Shared _key As Byte()
    Public Shared _timer As Timer = Nothing

    Public Shared Function GetCertificate() As X509Certificate2

        Return _Certificate

    End Function

    Public Shared Function SaveCert(cert As X509Certificate2) As X509Certificate2

        Dim abc As X509Certificate2 = New X509Certificate2(cert.GetRawCertData(), "dong1005", X509KeyStorageFlags.Exportable Or X509KeyStorageFlags.UserKeySet)

        'Dim a = cert.PrivateKey
        If cert.HasPrivateKey Then
            'Dim pKey As System.Security.Cryptography.AsymmetricAlgorithm = cert.PrivateKey
            abc.PrivateKey = cert.PrivateKey

            'Dim privateKey As RSACryptoServiceProvider = abc.PrivateKey
            'privateKey.Decrypt()
            'Dim keyData As Byte() = privateKey.ExportCspBlob(True)
        End If

        Return abc
    End Function

    Public Shared Function SelectCertificate(Optional ByVal serial As String = "") As X509Certificate2
        'If (serial <> "") Then
        '    _Certificate = Nothing

        '    '  Else

        'End If
        If _Certificate Is Nothing Then
            Dim x509Store As X509Store = New X509Store(StoreLocation.CurrentUser)
            x509Store.Open(OpenFlags.[ReadOnly])
            '108F56B4E814F7D66380
            Dim x509CertificateCollection As X509CertificateCollection
            If serial = "" Then
                x509CertificateCollection = X509Certificate2UI.SelectFromCollection(x509Store.Certificates, "Danh sách ch" & ChrW(7919) & " ký s" & ChrW(7889) & ".", "Hãy ch" & ChrW(7885) & "n m" & ChrW(7897) & "t ch" & ChrW(7919) & " ký s" & ChrW(7889) & ".", X509SelectionFlag.SingleSelection)
            Else
                ' Dim cers As X509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySerialNumber, serial, False)
                x509CertificateCollection = x509Store.Certificates.Find(X509FindType.FindBySerialNumber, serial, False)
            End If

            '    If (cers.Count > 0) Then
            '{
            '    cer = cers[0];
            '};
            'store.Close();


            If x509CertificateCollection.Count > 0 Then
                _Certificate = CType(x509CertificateCollection(0), X509Certificate2)
            Else
                Return Nothing
            End If
            'Dim pKey As System.Security.Cryptography.AsymmetricAlgorithm = _Certificate.PrivateKey
            'Dim data As Byte() = _Certificate.Export(X509ContentType.Cert, "*dksh123")
            '_Certificate = new X509Certificate2(data,"",X509KeyStorageFlags.Exportable or X509KeyStorageFlags.PersistKeySet)

            '_Certificate = New X509Certificate2(_Certificate.Export(X509ContentType.Cert, "*dksh123"), "*dksh123", X509KeyStorageFlags.DefaultKeySet)
            '_Certificate = New X509Certificate2("D:\DKSHVN.Pfx", "*dksh123")
            '_Certificate.PrivateKey = pKey
            _Certificate = SaveCert(_Certificate)
        End If

        Return _Certificate

    End Function

    Public Shared Function SignHashed(InputFile As String, OutputFile As String) As Boolean
        Dim card As X509Certificate2 = _Certificate
        If card Is Nothing Then
            Return False
        End If

        Dim hashtable As Hashtable = New Hashtable()
        Dim result As Boolean
        Try
            Dim x509CertificateParser As X509CertificateParser = New X509CertificateParser()
            Dim array As Org.BouncyCastle.X509.X509Certificate() = New Org.BouncyCastle.X509.X509Certificate() {x509CertificateParser.ReadCertificate(card.RawData)}
            Dim fileStream As FileStream = New FileStream(InputFile, FileMode.OpenOrCreate, FileAccess.Read)
            Dim array2 As Byte() = New Byte(fileStream.Length - 1) {}
            fileStream.Read(array2, 0, CInt(fileStream.Length))
            fileStream.Close()
            Dim reader As PdfReader = New PdfReader(array2)
            Dim pdfStamper As PdfStamper = pdfStamper.CreateSignature(reader, New FileStream(OutputFile, FileMode.OpenOrCreate), vbNullChar)
            Dim signatureAppearance As PdfSignatureAppearance = pdfStamper.SignatureAppearance
            signatureAppearance.SignDate = DateTime.Now
            Dim bf As BaseFont = BaseFont.CreateFont(Application.StartupPath + "\times.ttf", "Identity-H", False)
            Dim font As iTextSharp.text.Font = New iTextSharp.text.Font(bf)
            font.SetColor(255, 0, 0)
            signatureAppearance.Layer2Font = font
            signatureAppearance.Layer2Text = "Ký b" & ChrW(7903) & "i: " + PdfPKCS7.GetSubjectFields(array(0)).GetField("CN") + vbLf & "Ký ngày: " + String.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now)
            signatureAppearance.SetVisibleSignature(New iTextSharp.text.Rectangle(0.0F, 0.0F, 200.0F, 30.0F), 1, Nothing)
            signatureAppearance.SetCrypto(Nothing, array, Nothing, PdfSignatureAppearance.WINCER_SIGNED)
            Dim pdfSignature As PdfSignature = New PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1)
            pdfSignature.[Date] = New PdfDate(signatureAppearance.SignDate)
            pdfSignature.Name = ""
            If signatureAppearance.Reason IsNot Nothing Then
                pdfSignature.Reason = signatureAppearance.Reason
            End If
            If signatureAppearance.Location IsNot Nothing Then
                pdfSignature.Location = signatureAppearance.Location
            End If

            signatureAppearance.CryptoDictionary = pdfSignature
            Dim num As Integer = 4000
            hashtable(PdfName.CONTENTS) = num * 2 + 2
            signatureAppearance.PreClose(hashtable)
            Dim hashAlgorithm As HashAlgorithm = New SHA1CryptoServiceProvider()
            Dim rangeStream As Stream = signatureAppearance.RangeStream
            Dim array3 As Byte() = New Byte(8191) {}
            While True
                Dim expr_1FC As Integer = rangeStream.Read(array3, 0, 8192)
                Dim inputCount As Integer = expr_1FC
                If expr_1FC <= 0 Then
                    Exit While
                End If
                hashAlgorithm.TransformBlock(array3, 0, inputCount, array3, 0)
            End While

            rangeStream.Close()
            hashAlgorithm.TransformFinalBlock(array3, 0, 0)
            Dim array4 As Byte() = SignMsg(hashAlgorithm.Hash, False)
            If array4 IsNot Nothing Then
                Dim array5 As Byte() = New Byte(num - 1) {}
                Dim pdfDictionary As PdfDictionary = New PdfDictionary()
                System.Array.Copy(array4, 0, array5, 0, array4.Length)
                pdfDictionary.Put(PdfName.CONTENTS, New PdfString(array5).SetHexWriting(True))
                signatureAppearance.Close(pdfDictionary)
                result = True
            Else
                result = False
            End If
        Catch ex_27A As Exception
            'result = False
            Throw ex_27A
        End Try
        Return result
    End Function

    Public Shared Function SignHashedWithDate(InputFile As String, OutputFile As String, SignDate As DateTime) As Boolean
        Dim card As X509Certificate2 = _Certificate
        If card Is Nothing Then
            Return False
        End If

        Dim hashtable As Hashtable = New Hashtable()
        Dim result As Boolean
        Try
            Dim x509CertificateParser As X509CertificateParser = New X509CertificateParser()
            Dim array As Org.BouncyCastle.X509.X509Certificate() = New Org.BouncyCastle.X509.X509Certificate() {x509CertificateParser.ReadCertificate(card.RawData)}
            Dim fileStream As FileStream = New FileStream(InputFile, FileMode.OpenOrCreate, FileAccess.Read)
            Dim array2 As Byte() = New Byte(fileStream.Length - 1) {}
            fileStream.Read(array2, 0, CInt(fileStream.Length))
            fileStream.Close()
            Dim reader As PdfReader = New PdfReader(array2)
            Dim pdfStamper As PdfStamper = pdfStamper.CreateSignature(reader, New FileStream(OutputFile, FileMode.OpenOrCreate), vbNullChar)
            Dim signatureAppearance As PdfSignatureAppearance = pdfStamper.SignatureAppearance
            signatureAppearance.SignDate = SignDate
            Dim bf As BaseFont = BaseFont.CreateFont(Application.StartupPath + "\times.ttf", "Identity-H", False)
            Dim font As iTextSharp.text.Font = New iTextSharp.text.Font(bf)
            font.SetColor(255, 0, 0)
            signatureAppearance.Layer2Font = font
            signatureAppearance.Layer2Text = "Ký b" & ChrW(7903) & "i: " + PdfPKCS7.GetSubjectFields(array(0)).GetField("CN") + vbLf & "Ký ngày: " + String.Format("{0:dd/MM/yyyy HH:mm:ss}", SignDate)
            signatureAppearance.SetVisibleSignature(New iTextSharp.text.Rectangle(0.0F, 0.0F, 200.0F, 30.0F), 1, Nothing)
            signatureAppearance.SetCrypto(Nothing, array, Nothing, PdfSignatureAppearance.WINCER_SIGNED)
            Dim pdfSignature As PdfSignature = New PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1)
            pdfSignature.[Date] = New PdfDate(signatureAppearance.SignDate)
            pdfSignature.Name = ""
            If signatureAppearance.Reason IsNot Nothing Then
                pdfSignature.Reason = signatureAppearance.Reason
            End If
            If signatureAppearance.Location IsNot Nothing Then
                pdfSignature.Location = signatureAppearance.Location
            End If

            signatureAppearance.CryptoDictionary = pdfSignature
            Dim num As Integer = 4000
            hashtable(PdfName.CONTENTS) = num * 2 + 2
            signatureAppearance.PreClose(hashtable)
            Dim hashAlgorithm As HashAlgorithm = New SHA1CryptoServiceProvider()
            Dim rangeStream As Stream = signatureAppearance.RangeStream
            Dim array3 As Byte() = New Byte(8191) {}
            While True
                Dim expr_1FC As Integer = rangeStream.Read(array3, 0, 8192)
                Dim inputCount As Integer = expr_1FC
                If expr_1FC <= 0 Then
                    Exit While
                End If
                hashAlgorithm.TransformBlock(array3, 0, inputCount, array3, 0)
            End While

            rangeStream.Close()
            hashAlgorithm.TransformFinalBlock(array3, 0, 0)
            Dim array4 As Byte() = SignMsg(hashAlgorithm.Hash, False)
            If array4 IsNot Nothing Then
                Dim array5 As Byte() = New Byte(num - 1) {}
                Dim pdfDictionary As PdfDictionary = New PdfDictionary()
                System.Array.Copy(array4, 0, array5, 0, array4.Length)
                pdfDictionary.Put(PdfName.CONTENTS, New PdfString(array5).SetHexWriting(True))
                signatureAppearance.Close(pdfDictionary)
                result = True
            Else
                result = False
            End If
        Catch ex_27A As Exception
            'result = False
            Throw ex_27A
        End Try
        Return result
    End Function


    Public Shared Function SignMsg(msg As Byte(), detached As Boolean) As Byte()
        Dim signerCert As X509Certificate2 = _Certificate
        If signerCert Is Nothing Then
            Return Nothing
        End If

        'If (_key IsNot Nothing AndAlso _key.Length > 0) Then
        '    Return _key
        'End If

        Dim contentInfo As ContentInfo = New ContentInfo(msg)
        Dim signedCms As SignedCms = New SignedCms(contentInfo, detached)
        Dim cmsSigner As CmsSigner = New CmsSigner(signerCert)

        cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly
        'Dim result As Byte()
        Try
            'cmsSigner.
            If _timer IsNot Nothing Then
                _timer.Enabled = True
            End If
            signedCms.ComputeSignature(cmsSigner, False)
            'signedCms.CheckSignature(True)
            _key = signedCms.Encode()
        Catch ex_2E As Exception
            Throw New Exception("Can not get Private Key to sign!")
        End Try
        Return _key
    End Function

    Private Shared Sub AddSignableItems(relationship As PackageRelationship, partsToSign As List(Of Uri), relationshipsToSign As List(Of PackageRelationshipSelector))
        Dim item As PackageRelationshipSelector = New PackageRelationshipSelector(relationship.SourceUri, PackageRelationshipSelectorType.Id, relationship.Id)
        relationshipsToSign.Add(item)
        If relationship.TargetMode = TargetMode.Internal Then
            Dim part As PackagePart = relationship.Package.GetPart(PackUriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri))
            If Not partsToSign.Contains(part.Uri) Then
                partsToSign.Add(part.Uri)
                Dim enumerator As IEnumerator(Of PackageRelationship) = part.GetRelationships().GetEnumerator()
                Try
                    While enumerator.MoveNext()
                        Dim current As PackageRelationship = enumerator.Current
                        AddSignableItems(current, partsToSign, relationshipsToSign)
                    End While
                Finally
                    If enumerator IsNot Nothing Then
                        enumerator.Dispose()
                    End If
                End Try
            End If
        End If
    End Sub

    Public Shared Function AddSignature(PathFileName As String, PathOutput As String) As Boolean
        Dim certificate As X509Certificate2 = _Certificate
        If certificate Is Nothing Then
            Return False
        End If

        FileSystem.FileCopy(PathFileName, PathOutput)
        Dim package As Package = package.Open(PathOutput, FileMode.Open, FileAccess.ReadWrite)
        Try
            Dim list As List(Of Uri) = New List(Of Uri)()
            Dim relationshipsToSign As List(Of PackageRelationshipSelector) = New List(Of PackageRelationshipSelector)()
            Dim list2 As List(Of Uri) = New List(Of Uri)()

            Dim enumerator As IEnumerator(Of PackageRelationship) = package.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument").GetEnumerator()
            While enumerator.MoveNext()
                Dim current As PackageRelationship = enumerator.Current
                AddSignableItems(current, list, relationshipsToSign)
            End While
            If enumerator IsNot Nothing Then
                enumerator.Dispose()
            End If
            Dim packageDigitalSignature As PackageDigitalSignature = New PackageDigitalSignatureManager(package) With {.CertificateOption = CertificateEmbeddingOption.InSignaturePart}.Sign(list, certificate)
        Catch ex_2E As Exception
            Throw ex_2E
        Finally
            package.Close()
        End Try

        Return True
    End Function

    Public Shared Sub ClearCert()
        _Certificate = Nothing
    End Sub

End Class
