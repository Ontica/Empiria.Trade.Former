/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : CollectData                                      Pattern  : Data Services Static Class        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Contains data methods for financial services collect activities.                              *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  /// <summary>Contains data methods for financial services collect activities.</summary>
  static public class CollectEventData {

    #region Public methods

    static public DataTable GetCollectEvents() {
      return DataReader.GetDataTable(DataOperation.Parse("SELECT * FROM vwFSMCollectEvents"));
    }

    static public DataRow GetCollectEvent(int collectEventId) {
      return DataReader.GetDataRow(DataOperation.Parse("getFSMCollectEvent", collectEventId));
    }

    #endregion Public methods

    #region Internal methods

    static internal int GetNextCollectEventId() {
      return DataWriter.CreateId("FSMCollectEvents");
    }

    static internal int WriteCollectEvent(CollectEvent o) {
      DataOperation operation = DataOperation.Parse("writeFSMCollectEvent", o.Id, o.EventTypeId,
                                                    o.FinancialAccountId, o.EventTime, o.OnEventBalance,
                                                    o.PromisedAmount, o.PromisedDate, o.EventNotes,
                                                    o.CollectorId, o.ResolutionTypeId, o.ResolutionDate, o.ResolutionNotes,
                                                    o.AccountOldStatus, o.AccountNewStatus, o.ClosedById,
                                                    o.PostingTime, o.PostedById, o.Status);
      return DataWriter.Execute(operation);
    }

    #endregion Internal methods

  } // class CollectData

} // namespace Empiria.FinancialServices.Data