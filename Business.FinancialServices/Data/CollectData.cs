/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : CollectData                                      Pattern  : Data Services Static Class        *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Contains data methods for financial services collect activities.                              *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  /// <summary>Contains data methods for financial services collect activities.</summary>
  static public class CollectData {

    #region Public methods

    static public DataTable GetCollectors(int organizationId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditCollector", organizationId));
    }

    static public DataTable GetCollectTotals(int organizationId, DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectTotalsByOrgAndDate", organizationId, fromDate, toDate));
    }

    static public DataTable GetCollectTotalItems(int collectTotalId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectTotalItems", collectTotalId));
    }

    static public DataTable GetCollectTotalByInstrument(int collectTotalId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectTotalByInstrument", collectTotalId));
    }

    #endregion Public methods

    #region Internal methods

    static internal DataRow GetCollectTotal(int collectTotalId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryFSMCollectTotal", collectTotalId));
    }

    static internal int GetNextCollectTotalId() {
      return DataWriter.CreateId("FSMCollectTotals");
    }

    static internal DataOperation SetCollectTotalStatus(int collectTotalId, string newStatus) {
      return DataOperation.Parse("setFSMCollectTotalStatus", collectTotalId, newStatus);
    }

    static internal int WriteCollectTotal(CollectTotal o) {
      DataOperation operation = DataOperation.Parse("writeFSMCollectTotal", o.Id, o.OrganizationId,
                                                    o.CollectorId, o.CashierId, o.CollectDate, o.Notes,
                                                    o.CashTotal, o.ChecksTotal, o.PrePaidCardTotal,
                                                    o.EPaymentTotal, o.BankDepositTotal, o.CreditsRewardsTotal,
                                                    o.PostingTime, o.PostedById, o.Status);

      return DataWriter.Execute(operation);
    }

    #endregion Internal methods

  } // class CollectData

} // namespace Empiria.FinancialServices.Data