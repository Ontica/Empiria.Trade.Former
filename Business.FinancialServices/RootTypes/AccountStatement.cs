/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : AccountStatement                                 Pattern  : Business Services Class           *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a debit o credit financial account statement.                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;


using Empiria.FinancialServices.Data;

namespace Empiria.FinancialServices {

  public class AccountStatement {

    #region Fields

    private DateTime fromDate = DateTime.Today;
    private DateTime toDate = DateTime.Today;

    private decimal initialBalance = 0m;
    private decimal buys = 0m;
    private decimal rewards = 0m;
    private decimal otherConcepts = 0m;
    private decimal onRevision = 0m;
    private decimal payments = 0m;
    private decimal balance = 0m;
    private decimal minimalPayment = 0m;
    private decimal creditLimit = 0m;

    private FinancialAccount account = FinancialAccount.Empty;
    private DataTable transactions = null;

    #endregion Fields

    #region Constructors and Parsers

    public AccountStatement(FinancialAccount account, DateTime fromDate, DateTime toDate) {
      this.account = account;
      this.fromDate = fromDate;
      this.toDate = toDate;
      if (!account.IsEmptyInstance) {
        LoadData();
      }
    }

    #endregion Constructors and Parsers

    #region Public properties

    public FinancialAccount Account {
      get { return account; }
    }

    public DateTime FromDate {
      get { return fromDate; }
      set { fromDate = value; }
    }

    public DateTime ToDate {
      get { return toDate; }
      set { toDate = value; }
    }

    public decimal InitialBalance {
      get { return initialBalance; }
    }

    public decimal Buys {
      get { return buys; }
    }

    public decimal Rewards {
      get { return rewards; }
    }

    public decimal OtherConcepts {
      get { return otherConcepts; }
    }

    public decimal OnRevision {
      get { return onRevision; }
    }

    public decimal Payments {
      get { return payments; }
    }

    public decimal Balance {
      get { return balance; }
    }

    public decimal AvailableCredit {
      get { return Math.Max(0m, creditLimit - balance); }
    }

    public decimal CreditLimit {
      get { return creditLimit; }
    }

    public decimal MinimalPayment {
      get { return minimalPayment; }
    }

    #endregion Public properties

    #region Public methods

    public DataTable Transactions() {
      return transactions;
    }

    public DataView OnRevisionTransactions() {
      DataTable table = FinancialAccountData.GetAllTransactions(account.Id, DateTime.Parse("01/01/1970"), toDate);

      return new DataView(table, "TransactionStatus = 'S'", String.Empty, DataViewRowState.CurrentRows);
    }

    #endregion Public methods

    #region Private methods

    private void LoadData() {
      DataRow data = AccountBalancesData.GetBalance(account.Id, fromDate, toDate);
      transactions = FinancialAccountData.GetTransactions(account.Id, fromDate, toDate);

      initialBalance = (decimal) data["InitialBalance"];
      buys = CalculateBuys();
      payments = CalculatePayments();
      creditLimit = (decimal) data["AuthorizedLimit"];
      rewards = CalculateRewardsTotal();
      otherConcepts = CalculateOtherConceptsTotal();
      onRevision = CalculateOnRevisionTotal();
      balance = (decimal) data["Balance"] + CalculateOnRevisionTotal();
      minimalPayment = (decimal) data["Balance"] / 2m;
    }

    private decimal CalculateBuys() {
      DataView view = new DataView(transactions, "[FinancialConceptCode] LIKE 'FSB%'", String.Empty, DataViewRowState.CurrentRows);
      decimal buys = 0m;
      for (int i = 0; i < view.Count; i++) {
        buys += (decimal) view[i]["Credit"];
      }
      return buys;
    }

    private decimal CalculatePayments() {
      DataView view = new DataView(transactions, "[FinancialConceptCode] LIKE 'FSP%'", String.Empty, DataViewRowState.CurrentRows);
      decimal payments = 0m;

      for (int i = 0; i < view.Count; i++) {
        payments += (decimal) view[i]["Debit"];
      }
      return payments;
    }


    private decimal CalculateOnRevisionTotal() {
      DataView view = OnRevisionTransactions();
      decimal total = 0m;

      for (int i = 0; i < view.Count; i++) {
        total += (decimal) view[i]["Credit"] - (decimal) view[i]["Debit"];
      }
      return total;
    }

    private decimal CalculateRewardsTotal() {
      DataView view = new DataView(transactions, "[FinancialConceptId] = 28 OR [FinancialConceptId] = 29", String.Empty, DataViewRowState.CurrentRows);
      decimal rewards = 0m;
      for (int i = 0; i < view.Count; i++) {
        rewards += (decimal) view[i]["Amount"];
      }
      return rewards;
    }

    private decimal CalculateOtherConceptsTotal() {
      DataView view = new DataView(transactions, "([FinancialConceptId] <> 28 AND [FinancialConceptId] <> 29) AND ([FinancialConceptCode] LIKE 'FSC%' OR [FinancialConceptCode] LIKE 'FSD%')", String.Empty, DataViewRowState.CurrentRows);

      decimal total = 0m;
      for (int i = 0; i < view.Count; i++) {
        total += (decimal) view[i]["Credit"] - (decimal) view[i]["Debit"];
      }
      return total;
    }

    #endregion Private methods
  } // class AccountStatement

} // namespace Empiria.FinancialServices