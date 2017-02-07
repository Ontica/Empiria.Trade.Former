/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Billing System                    *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : BillingData                                      Pattern  : Data Services Static Class        *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Database read and write methods for billing services.                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Trade.Billing;

namespace Empiria.Trade.Data {

  /// <summary>Database read and write methods for billing services.</summary>
  static internal class BillingDS {

    #region Public methods

    static internal DataView GetBills(DateTime fromDate, DateTime toDate, string filter) {
      try {
        DataOperation dataOperation = DataOperation.Parse("qryCRMBills", fromDate, toDate);
        dataOperation.ExecutionTimeout = 500;
        return DataReader.GetDataView(dataOperation, filter);
      } catch {
        EmpiriaLog.Error("BillingData.GetBills. Filter Error: filter = " + filter);
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

  } // class BillingDS

} // namespace Empiria.Trade.Data
