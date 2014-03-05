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

    static public void CancelStamp(Bill bill) {
      var ws = new WSConecFM.Cancelado();
      var request = BillStamper.GetCancelStampRequest(bill);
      WSConecFM.Resultados stampResult = ws.Cancelar(request, bill.Stamp.UUID);
      if (!stampResult.status) {
        throw CancelBillStamperException(bill, stampResult);
      }
    }

    static internal BillStamp Stamp(Bill bill) {
      var ws = new WSConecFM.Timbrado();
      var request = BillStamper.GetStampRequest(bill);

      WSConecFM.Resultados stampResult = ws.Timbrar(bill.GetXmlFileNameFull(), request); // ws.Timbrar(bill.GetXmlString(), request);
      if (stampResult.status) {
        return new BillStamp(stampResult);
      } else {
        throw CreateBillStamperException(bill, stampResult);
      }
    }

    #endregion Public methods

    #region Private methods

    static private Exception CancelBillStamperException(Bill bill, WSConecFM.Resultados stampResult) {
      var e = new Empiria.Trade.Ordering.TradeOrderingException(Ordering.TradeOrderingException.Msg.CancelBillStamperException,
                                                                bill.Order.Number, bill.Order.Customer.FullName,
                                                                bill.Stamp.UUID, stampResult.code, stampResult.message);
      e.Publish();
      return e;
    }

    static private Exception CreateBillStamperException(Bill bill, WSConecFM.Resultados stampResult) {
      var e = new Empiria.Trade.Ordering.TradeOrderingException(Ordering.TradeOrderingException.Msg.CreateBillStamperException,
                                                                bill.Order.Number, bill.Order.Customer.FullName,
                                                                stampResult.code, stampResult.message);
      e.Publish();
      return e;
    }

    static private WSConecFM.requestCancelarCFDI GetCancelStampRequest(Bill bill) {
      var request = new WSConecFM.requestCancelarCFDI();

      if (bill.NotOrderData == null) {
        request.emisorRFC = bill.Order.Supplier.FormattedTaxTag;
      } else {
        request.emisorRFC = bill.IssuedBy.FormattedTaxTag;
      }
      request.urlCancelado = BillStamper.WSConnectUrl;
      request.UserID = BillStamper.WSConnectUserID;
      request.UserPass = BillStamper.WSConnectUserPass;
      request.uuid = bill.Stamp.UUID;

      return request;
    }

    static private WSConecFM.requestTimbrarCFDI GetStampRequest(Bill bill) {
      var request = new WSConecFM.requestTimbrarCFDI();

      if (bill.NotOrderData == null) {
        request.emisorRFC = bill.Order.Supplier.FormattedTaxTag;
      } else {
        request.emisorRFC = bill.IssuedBy.FormattedTaxTag;
      }
      request.generarCBB = true;
      request.generarPDF = false;
      request.generarTXT = false;
      request.urlTimbrado = BillStamper.WSConnectUrl;
      request.UserID = BillStamper.WSConnectUserID;
      request.UserPass = BillStamper.WSConnectUserPass;

      return request;
    }

    #endregion Private methods

  }   // class BillStamper

}  // namespace Empiria.Trade.Billing
