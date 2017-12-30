using System;
using System.Xml;

namespace Empiria.Trade.Billing {

  public class BillStamp {

    private XmlDocument xmlDocument = new System.Xml.XmlDocument();

    public BillStamp() {

    }

    #region Properties


    public string AuthorityCertificateNumber {
      get;
      set;
    } = String.Empty;

    public string AuthorityDigitalString {
      get {
        if (this.Version == String.Empty || this.Version == "1.0") {
          return "||1.0|" + this.UUID + "|" +
                 this.Timestamp.ToString(@"yyyy-MM-dd\THH:mm:ss") + "|" +
                 this.AuthorityStamp + "|" +
                 this.AuthorityCertificateNumber + "||";
        } else {
          return "||1.1|" + this.UUID + "|" +
                 this.Timestamp.ToString(@"yyyy-MM-dd\THH:mm:ss") + "|" +
                 this.AuthorityTaxID + "|" +
                 this.IssuerStamp + "|" + this.AuthorityCertificateNumber + "||";
        }
      }
    }

    public string AuthorityStamp {
      get;
      set;
    } = String.Empty;


    public string AuthorityTaxID {
      get;
      set;
    } = String.Empty;


    public string IssuerStamp {
      get;
      set;
    } = String.Empty;

    public byte[] QRCode {
      get;
      set;
    }

    public DateTime Timestamp {
      get;
      set;
    }

    public string UUID {
      get;
      set;
    } = String.Empty;


    public string Version {
      get;
      set;
    } = String.Empty;

    #endregion Properties

    public XmlDocument GetXmlDocument() {
      return this.xmlDocument;
    }

    #region Methods

    internal BillStamp(WSConecFM.Resultados stampResult) {
      Assertion.AssertObject(stampResult, "stampResult");
      Assertion.Assert(stampResult.status,
                        "Bill stamp external web service call return an error status " +
                        stampResult.code);

      this.QRCode = System.Convert.FromBase64String(stampResult.cbbBase64);

      this.xmlDocument = new System.Xml.XmlDocument();

      byte[] xmlByteArray = System.Convert.FromBase64String(stampResult.xmlBase64);

      this.xmlDocument.LoadXml(System.Text.Encoding.UTF8.GetString(xmlByteArray));

      XmlElement item = (XmlElement) this.xmlDocument.GetElementsByTagName("tfd:TimbreFiscalDigital").Item(0);

      this.Version = item.GetAttribute("Version");
      this.UUID = item.GetAttribute("UUID");
      this.AuthorityTaxID = item.GetAttribute("RfcProvCertif");
      this.AuthorityCertificateNumber = item.GetAttribute("NoCertificadoSAT");
      this.AuthorityStamp = item.GetAttribute("SelloSAT");
      this.IssuerStamp = item.GetAttribute("SelloCFD");
      this.Timestamp = XmlConvert.ToDateTime(item.GetAttribute("FechaTimbrado"),
                                             XmlDateTimeSerializationMode.Utc);
    }

    static internal BillStamp Parse(string json) {
      return Empiria.Data.JsonConverter.ToObject<BillStamp>(json);
    }

    public string ToJson() {
      return Empiria.Data.JsonConverter.ToJson(this);
    }

    #endregion Methods

  }   //internal class BillStamp

}  // namespace Empiria.Trade.Billing
