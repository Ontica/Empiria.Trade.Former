/* Empiria® Trade 2013 ***************************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Trade                                   System   : Ordering System                   *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : BillingData                                      Pattern  : Data Services Static Class        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Database read and write methods for billing services data.                                    *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.Trade.Billing;

namespace Empiria.Trade.Data {

  /// <summary>Database read and write methods for billing services data.</summary>
  static public class BillingData {

    #region Public methods

    static internal DataView GetBills(DateTime fromDate, DateTime toDate, string filter) {
      DataOperation dataOperation = DataOperation.Parse("qryCRMBills", fromDate, toDate);

      dataOperation.ExecutionTimeout = 500;

      return DataReader.GetDataView(dataOperation, filter);
    }

    #endregion Public methods

    #region Internal methods

    static internal int WriteBill(Bill o) {
      DataOperation dataOperation = DataOperation.Parse("writeCRMBill", o.Id, (char) o.BillType,
                        o.Order.Id, o.Order.ExternalOrderId, o.IssuedBy.Id, o.IssuedTime, o.CertificateNumber,
                        o.SerialNumber, o.Number, o.ApprovalYear, o.ApprovalNumber, o.DigitalString,
                        o.DigitalSign, o.GetXmlString(), o.CanceledBy.Id, o.CancelationTime, (char) o.Status);

      return DataWriter.Execute(dataOperation);
    }

    #endregion Internal methods

  } // class BillingData

} // namespace Empiria.Trade.Data