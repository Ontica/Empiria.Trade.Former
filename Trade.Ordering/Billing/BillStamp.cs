using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Xml;

namespace Empiria.Trade.Billing {

  public class BillStamp {

    #region Properties

    public string AuthorityCertificateNumber {
      get;
      private set;
    }

    public string AuthorityStamp {
      get;
      private set;
    }

    public Bill Bill {
      get;
      private set;
    }

    public string IssuerStamp {
      get;
      private set;
    }

    public byte[] QRCode { 
      get;
      private set;
    }

    public DateTime Timestamp {
      get;
      private set;
    }

    public string UUID {
      get;
      private set;
    }

    public XmlDocument XmlDocument {
      get;
      private set;
    }

    #endregion Properties


    #region Methods

    internal BillStamp(WSConecFM.Resultados stampResult) {
      Assertion.AssertObject(stampResult, "stampResult");
      Assertion.Assert(stampResult.status,
                        "Bill stamp external web service call return an error status " + 
                        stampResult.code);
      this.QRCode = System.Convert.FromBase64String(stampResult.cbbBase64);
      this.XmlDocument = new System.Xml.XmlDocument();
      byte[] xmlByteArray = System.Convert.FromBase64String(stampResult.xmlBase64);
      this.XmlDocument.LoadXml(System.Text.Encoding.UTF8.GetString(xmlByteArray));
      XmlElement item = (XmlElement) this.XmlDocument.GetElementsByTagName("tfd:TimbreFiscalDigital").Item(0);
      this.UUID = item.GetAttribute("UUID");
      this.AuthorityCertificateNumber = item.GetAttribute("noCertificadoSAT");
      this.AuthorityStamp = item.GetAttribute("selloSAT");
      this.IssuerStamp = item.GetAttribute("selloCFD");
      this.Timestamp = XmlConvert.ToDateTime(item.GetAttribute("FechaTimbrado"), 
                                             XmlDateTimeSerializationMode.Utc);
    }
 
    static internal BillStamp Parse(string json) {
      throw new NotImplementedException();
    }

    public string ToJson() {
      return Empiria.Data.JsonConverter.ToJson(this);
    }
    
    #endregion Methods

  }   //internal class BillStamp

}  // namespace Empiria.Trade.Billing
