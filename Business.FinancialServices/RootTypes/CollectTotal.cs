/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : CollectTotal                                     Pattern  : Business Services Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents the total amounts for daily activity of credit collect for a single collector.     *
*                                                                                                            *
********************************* Copyright (c) 2003-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using Empiria.Data;
using Empiria.FinancialServices.Data;
using Empiria.Treasury;

namespace Empiria.FinancialServices {

  /// <summary>Represents the total amounts for daily activity of credit collect for a single collector.</summary>
  public class CollectTotal {

    public int Id;
    public int OrganizationId;
    public string Organization = String.Empty;
    public int CollectorId;
    public string Collector = String.Empty;
    public int CashierId;
    public CashRegister CashRegister = null;
    public DateTime CollectDate = DateTime.Today;
    public string Notes = String.Empty;
    public decimal CashTotal = 0m;
    public decimal ChecksTotal = 0m;
    public decimal PrePaidCardTotal = 0m;
    public decimal EPaymentTotal = 0m;
    public decimal BankDepositTotal = 0m;
    public decimal CreditsRewardsTotal = 0m;
    public DateTime PostingTime = DateTime.Today;
    public int PostedById = 0;
    private string status = "P";

    private DataTable collectTotalItemsTable = null;
    private DataTable collectTotalByInstrument = null;

    private AccountTransaction[] items = null;

    static public CollectTotal Parse(int collectTotalId) {
      DataRow row = CollectData.GetCollectTotal(collectTotalId);

      CollectTotal collect = new CollectTotal();

      collect.Id = collectTotalId;
      collect.OrganizationId = (int) row["OrganizationId"];
      collect.Organization = (string) row["Organization"];
      collect.CollectorId = (int) row["CollectorId"];
      collect.Collector = (string) row["Collector"];
      collect.CashierId = (int) row["CashierId"];
      //collect.Cashier = CashRegister.Parse(collect.CashierId);
      collect.CollectDate = (DateTime) row["CollectDate"];
      collect.Notes = (string) row["Notes"];
      collect.CashTotal = (decimal) row["CashTotal"];
      collect.ChecksTotal = (decimal) row["ChecksTotal"];
      collect.PrePaidCardTotal = (decimal) row["PrePaidCardTotal"];
      collect.EPaymentTotal = (decimal) row["EPaymentTotal"];
      collect.BankDepositTotal = (decimal) row["BankDepositTotal"];
      collect.CreditsRewardsTotal = (decimal) row["CreditsRewardsTotal"];
      collect.PostingTime = DateTime.Today;
      collect.PostedById = 0;
      collect.status = (string) row["CollectTotalStatus"];

      collect.collectTotalItemsTable = CollectData.GetCollectTotalItems(collectTotalId);
      collect.collectTotalByInstrument = CollectData.GetCollectTotalByInstrument(collectTotalId);

      return collect;
    }

    public decimal ClosedTotal() {
      if (collectTotalByInstrument.Rows.Count == 0) {
        return 0m;
      }

      decimal sum = 0m;
      for (int i = 0; i < collectTotalByInstrument.Rows.Count; i++) {
        sum += Convert.ToDecimal(collectTotalByInstrument.Rows[i]["PaymentAmount"]);
      }
      return sum;
    }

    public decimal RewardsClosedTotal() {
      decimal rewardsSum = 0;

      for (int i = 0; i < collectTotalItemsTable.Rows.Count; i++) {
        if ((int) collectTotalItemsTable.Rows[i]["FinancialConceptId"] == 28 || (int) collectTotalItemsTable.Rows[i]["FinancialConceptId"] == 29) {
          rewardsSum += Convert.ToDecimal(collectTotalItemsTable.Rows[i]["Debit"]);
        }
      }
      return rewardsSum;
    }

    public decimal Total {
      get {
        return CashTotal + ChecksTotal + PrePaidCardTotal + EPaymentTotal + BankDepositTotal;
      }
    }

    public string Status {
      get { return status; }
    }

    public decimal ToCloseTotal() {
      return Total - ClosedTotal();
    }
    public decimal ToRewardsCloseTotal() {
      return CreditsRewardsTotal - RewardsClosedTotal();
    }

    public void Save() {
      if (Id == 0) {
        this.Id = CollectData.GetNextCollectTotalId();
      }
      CollectData.WriteCollectTotal(this);
    }

    public bool CanClose() {
      if (!IsInstrumentClosed()) {
        return false;
      }
      if (!IsRewardsClosed()) {
        return false;
      }
      return true;
    }

    public void Close() {
      if (!CanClose()) {
        return;
      }

      using (DataWriterContext context = DataWriter.CreateContext("CloseCollectTotal")) {
        this.status = "C";
        context.Add(CollectData.SetCollectTotalStatus(this.Id, "C"));
        context.Update();
      }
      FinancialAccount.RebuildAllCreditBalances();
    }

    public bool CanDelete() {
      return false;
    }

    public void Delete() {
      if (!CanDelete()) {
        return;
      }

      AccountTransaction[] transactions = new AccountTransaction[collectTotalItemsTable.Rows.Count];
      for (int i = 0; i < collectTotalItemsTable.Rows.Count; i++) {
        transactions[i] = AccountTransaction.Parse(collectTotalItemsTable.Rows[i]);
      }
      using (DataWriterContext context = DataWriter.CreateContext("DeleteCollectTotal")) {
        ITransaction transaction = context.BeginTransaction();
        for (int i = 0; i < transactions.Length; i++) {
          //context.Add(transactions[i].Delete(this));
        }
        this.status = "X";
        context.Add(CollectData.SetCollectTotalStatus(this.Id, "X"));
        context.Update();
        transaction.Commit();
      }
    }

    public AccountTransaction[] Items {
      get {
        if (items == null) {
          items = GetItems(this.Id);
        }
        return items;
      }
    }

    public bool IsInstrumentClosed() {
      if (collectTotalByInstrument.Rows.Count == 0) {
        return false;
      }

      decimal sum = 0m;
      for (int i = 0; i < collectTotalByInstrument.Rows.Count; i++) {
        decimal total = Convert.ToDecimal(collectTotalByInstrument.Rows[i]["CreditPaymentTotalInInstrument"]);
        if ((total - 2.0m) < Convert.ToDecimal(collectTotalByInstrument.Rows[i]["PaymentAmount"]) &&
            Convert.ToDecimal(collectTotalByInstrument.Rows[i]["PaymentAmount"]) < (total + 2.0m)) {
          sum += total;
        } else {
          return false;
        }
      }
      if ((sum - 2.0m) < this.Total && this.Total < (sum + 2.0m)) {
        return true;
      } else {
        return false;
      }
    }

    public bool IsRewardsClosed() {
      decimal rewardsSum = 0;

      for (int i = 0; i < collectTotalItemsTable.Rows.Count; i++) {
        if ((int) collectTotalItemsTable.Rows[i]["FinancialConceptId"] == 28 || (int) collectTotalItemsTable.Rows[i]["FinancialConceptId"] == 29) {
          rewardsSum += Convert.ToDecimal(collectTotalItemsTable.Rows[i]["Debit"]);
        }
      }
      if ((rewardsSum - 2.0m) < this.CreditsRewardsTotal && this.CreditsRewardsTotal < (rewardsSum + 2.0m)) {
        return true;
      } else {
        return false;
      }
    }

    //public AccountTransaction CreatePayment(decimal amount, string summary, string notes, int referenceId) {
    //  AccountTransaction transaction = null;
    //  if ((conceptId <= 27 || conceptId == 35) && conceptId != 222) {
    //    transaction = AccountTransaction.CreateCredit(this, organizationId, conceptId, transactionDate,
    //                                                  amount, summary, notes, referenceId);
    //  } else {
    //    transaction = AccountTransaction.CreateDebit(collectTotal, this, organizationId, conceptId, transactionDate,
    //                                                 amount, summary, notes, referenceId);
    //  }
    //  transaction.Save();

    //  return transaction;
    //}  

    private AccountTransaction[] GetItems(int collectTotalId) {
      AccountTransaction[] transactions = new AccountTransaction[collectTotalItemsTable.Rows.Count];

      for (int i = 0; i < collectTotalItemsTable.Rows.Count; i++) {
        transactions[i] = AccountTransaction.Parse(collectTotalItemsTable.Rows[i]);
      }

      return transactions;
    }


  } // class CollectTotal

} // namespace Empiria.FinancialServices
