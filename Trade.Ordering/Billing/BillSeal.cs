/* Empiria Customers Components 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : BillSeal                                         Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Generates a bill seal from an xml file using XSLT.                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.IO;

using System.Security.Cryptography;

using System.Text;

using System.Xml;
using System.Xml.Xsl;

namespace Empiria.Trade.Billing {

  /// <summary>Generates a bill seal from an xml file using XSLT.</summary>
  public class BillSeal {

    #region Fields

    private static readonly string BillSealXsltFilePath = ConfigurationData.GetString("BillSealXsltFilePath");

    private XmlDocument xmlDocument = null;
    private string result = null;

    #endregion Fields

    #region Constructors and parsers

    public BillSeal(XmlDocument xml) {
      Assertion.AssertObject(xml, "xml");

      this.xmlDocument = xml;
    }

    #endregion Constructors and parsers

    #region Public methods

    public string Sign(BillIssuerData issuer) {
      string privateKeyFileName = Bill.BillCertificatesFolder + issuer.PrivateKeyFile;

      Byte[] pLlavePrivadaenBytes = System.IO.File.ReadAllBytes(privateKeyFileName);

      RSACryptoServiceProvider rsa = SATSeguridad.DecodeEncryptedPrivateKeyInfo(pLlavePrivadaenBytes,
                                                                                issuer.GetCertificatePassword());
      byte[] array = null;

      string sealAsText = this.ToString();

      SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
      array = rsa.SignData(Encoding.UTF8.GetBytes(sealAsText), hasher);

      return Convert.ToBase64String(array);
    }

    public override string ToString() {
      if (this.result == null) {
        this.result = this.GenerateSeal();
      }
      return this.result;
    }

    #endregion Public methods

    #region Private methods

    private string GenerateSeal() {
      XslCompiledTransform xsltTransformer = new XslCompiledTransform();

      xsltTransformer.Load(BillSealXsltFilePath);

      StringWriter stringWriter = new StringWriter();
      XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

      xsltTransformer.Transform(this.xmlDocument, xmlTextWriter);

      return stringWriter.ToString();
    }

    #endregion Private methods

  } // class BillSeal

} // namespace Empiria.Trade.Billing
