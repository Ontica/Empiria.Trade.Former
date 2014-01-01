using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Empiria.Trade.Billing {

  static public class BillStamper {

    #region Fields

    static private readonly string WSConnectUrl = ConfigurationData.GetString("Timbrado.WSConnect.Url");
    static private readonly string WSConnectUserID = ConfigurationData.GetString("Timbrado.WSConnect.UserID");
    static private readonly string WSConnectUserPass = ConfigurationData.GetString("Timbrado.WSConnect.UserPassword");

    #endregion Fields

    #region Public methods

    static internal BillStamp Stamp(Bill bill) {
      var ws = new WSConecFM.Timbrado();
      var request = BillStamper.GetStampRequest(bill);
      //var layout = GetLayout();
      WSConecFM.Resultados stampResult = ws.Timbrar(@"D:\facturas\P0663078.M085484.20131230124712.xml", request); // ws.Timbrar(bill.GetXmlString(), request);
      if (stampResult.status) {
        return new BillStamp(stampResult);
      } else {
        throw CreateBillStamperException(bill, stampResult);
      }
    }

    private static string GetLayout() {
      var objReader = new System.IO.StreamReader(@"D:\facturas\P0663078.M085484.20131230124712.xml", Encoding.UTF8);
      var layout = objReader.ReadToEnd();
      objReader.Close();
      Empiria.Messaging.Publisher.Publish("XML:" + layout);
      return layout; //  bill.GetXmlString();
    }

    static internal BillStamp CancelStamp(Bill bill) {
      throw new NotImplementedException();
    }

    #endregion Public methods

    #region Private methods

    static private Exception CreateBillStamperException(Bill bill, WSConecFM.Resultados stampResult) {
      return new Empiria.Trade.Ordering.TradeOrderingException(Ordering.TradeOrderingException.Msg.CreateBillStamperException,
                                                               bill.Order.Number, bill.Order.Customer.FullName,
                                                               stampResult.code, stampResult.message);
    }

    static private WSConecFM.requestTimbrarCFDI GetStampRequest(Bill bill) {
      var request = new WSConecFM.requestTimbrarCFDI();

      request.emisorRFC = bill.IssuedBy.FormattedTaxTag;
      request.generarCBB = true;
      request.generarPDF = false;
      request.generarTXT = true;
      request.urlTimbrado = BillStamper.WSConnectUrl;
      request.UserID = BillStamper.WSConnectUserID;
      request.UserPass = BillStamper.WSConnectUserPass;

      return request;
    }

    #endregion Private methods

  }   // class BillStamper

}  // namespace Empiria.Trade.Billing
