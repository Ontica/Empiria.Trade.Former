/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : CollectEvent                                     Pattern  : Business Services Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a financial account collect event.                                                 *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;


using Empiria.FinancialServices.Data;

namespace Empiria.FinancialServices {

  /// <summary>Represents a financial account collect event.</summary>
  public class CollectEvent {

    public int Id;
    public int EventTypeId;
    public int FinancialAccountId;
    public DateTime EventTime = DateTime.Now;
    public decimal OnEventBalance = 0m;
    public decimal PromisedAmount = 0m;
    public DateTime PromisedDate = DateTime.Today;
    public string EventNotes = String.Empty;
    public int CollectorId = -1;
    public int ResolutionTypeId = -1;
    public DateTime ResolutionDate = ExecutionServer.DateMaxValue;
    public string ResolutionNotes = String.Empty;
    public char AccountOldStatus = 'A';
    public char AccountNewStatus = 'A';
    public int ClosedById = -1;
    public DateTime PostingTime = DateTime.Now;
    public int PostedById;
    public char Status = 'A';

    static public CollectEvent Parse(int collectEventId) {
      DataRow row = CollectEventData.GetCollectEvent(collectEventId);

      CollectEvent collectEvent = new CollectEvent();

      collectEvent.Id = (int) row["CollectEventId"];
      collectEvent.EventTypeId = (int) row["CollectEventTypeId"];
      collectEvent.FinancialAccountId = (int) row["FinancialAccountId"];
      collectEvent.EventTime = (DateTime) row["CollectEventTime"];
      collectEvent.OnEventBalance = (decimal) row["CashTotal"];
      collectEvent.PromisedAmount = (decimal) row["OnEventBalance"];
      collectEvent.PromisedDate = (DateTime) row["PromisedAmount"];
      collectEvent.EventNotes = (string) row["EventNotes"];
      collectEvent.CollectorId = (int) row["CollectorId"];
      collectEvent.ResolutionTypeId = (int) row["ResolutionTypeId"];
      collectEvent.ResolutionDate = (DateTime) row["ResolutionDate"];
      collectEvent.ResolutionNotes = (string) row["ResolutionNotes"];
      collectEvent.AccountOldStatus = (char) row["AccountOldStatus"];
      collectEvent.AccountNewStatus = (char) row["AccountNewStatus"];
      collectEvent.ClosedById = (int) row["ClosedById"];
      collectEvent.PostingTime = (DateTime) row["PostingTime"];
      collectEvent.PostedById = (int) row["PostedById"];
      collectEvent.Status = (char) row["CollectEventStatus"];

      return collectEvent;
    }

    public void Save() {
      if (Id == 0) {
        this.Id = CollectEventData.GetNextCollectEventId();
      }
      CollectEventData.WriteCollectEvent(this);
    }

    public bool CanDelete() {
      return false;
    }

    public void Delete() {
      //if (!CanDelete()) {
      //  return;
      //}

      //AccountTransaction[] transactions = new AccountTransaction[collectTotalItemsTable.Rows.Count];
      //for (int i = 0; i < collectTotalItemsTable.Rows.Count; i++) {
      //  transactions[i] = AccountTransaction.ParseWithRow(collectTotalItemsTable.Rows[i]);
      //}
      //using (DataWriterContext context = DataWriter.CreateContext("DeleteCollectTotal")) {
      //  ITransaction transaction = context.BeginTransaction();
      //  for (int i = 0; i < transactions.Length; i++) {
      //    //context.Add(transactions[i].Delete(this));
      //  }
      //  this.status = "X";
      //  context.Add(CollectData.SetCollectTotalStatus(this.Id, "X"));
      //  context.Update();
      //  transaction.Commit();
      //}
    }


  } // class CollectEvent

} // namespace Empiria.FinancialServices