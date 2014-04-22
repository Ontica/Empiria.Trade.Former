/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : BillingData                                      Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Database read and write methods for billing services data.                                    *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      try {
        DataOperation dataOperation = DataOperation.Parse("qryCRMBills", fromDate, toDate);
        dataOperation.ExecutionTimeout = 500;
        return DataReader.GetDataView(dataOperation, filter);
      } catch {
        Empiria.Messaging.Publisher.Publish("BillingData.GetBills ERROR : filter = " + filter);
        throw;
      } 
    }

    #endregion Public methods

    #region Internal methods

    static internal int WriteBill(Bill o) {
      DataOperation dataOperation = DataOperation.Parse("writeCRMBill", o.Id, (char) o.BillType,
                        o.Order.Id, o.Order.ExternalOrderId, o.IssuedBy.Id, o.IssuedTime, o.CertificateNumber,
                        o.SerialNumber, o.Number, o.ApprovalYear, o.ApprovalNumber, o.DigitalString,
                        o.DigitalSign, o.GetXmlString(), o.HasStamp ? o.Stamp.ToJson() : String.Empty, 
                        o.CanceledBy.Id, o.CancelationTime, (char) o.Status);

      return DataWriter.Execute(dataOperation);
    }

    #endregion Internal methods

  } // class BillingData

} // namespace Empiria.Trade.Data