/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : AccountTransaction                               Pattern  : Business Services Class           *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a debit o credit financial services account transaction.                           *
*                                                                                                            *
********************************* Copyright (c) 2003-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.FinancialServices.Data;
using Empiria.Treasury;

namespace Empiria.FinancialServices {

  public class AccountTransaction : BaseObject {

    #region Fields

    private FinancialAccount financialAccount = FinancialAccount.Empty;
    private FinancialConcept financialConcept = FinancialConcept.Empty;
    private Organization organization = Organization.Empty;
    private int appliedToId = -1;
    private Contact collectedBy = Person.Empty;
    private int referenceId = -1;
    private string summary = String.Empty;
    private string notes = String.Empty;
    private int authorizationId = -1;
    private DateTime transactionDate = DateTime.Today;
    private DateTime postingDate = DateTime.Now;
    private DateTime cancelationDate = ExecutionServer.DateMaxValue;
    private int canceledById = -1;
    private int currencyId = 600;
    private decimal amount = 0m;
    private decimal taxes = 0m;
    private decimal credit = 0m;
    private decimal debit = 0m;
    private decimal creditBalance = 0m;
    private int crTransactionId = -1;
    private Contact postedBy = Person.Empty;
    private string status = "A";

    #endregion Fields

    #region Constructors and parsers

    private AccountTransaction() {
      // Required by Empiria Framework.
    }

    protected AccountTransaction(FinancialAccount account, FinancialConcept concept) {
      this.financialAccount = account;
      this.financialConcept = concept;
    }

    static public AccountTransaction Parse(int id) {
      return BaseObject.ParseId<AccountTransaction>(id);
    }

    static public AccountTransaction Empty {
      get { return BaseObject.ParseEmpty<AccountTransaction>(); }
    }

    static internal AccountTransaction CreateCredit(FinancialAccount account, FinancialConcept financialConcept, Organization organization,
                                                    DateTime transactionDate, decimal amount, decimal taxes, string summary, string notes,
                                                    int referenceId) {
      AccountTransaction transaction = new AccountTransaction(account, financialConcept);
      transaction.organization = organization;
      transaction.transactionDate = transactionDate;
      transaction.amount = amount;
      transaction.taxes = taxes;
      transaction.credit = amount;
      transaction.debit = 0m;
      transaction.summary = summary;
      transaction.notes = notes;
      transaction.referenceId = referenceId;

      return transaction;
    }

    static internal AccountTransaction CreateDebit(FinancialAccount account, FinancialConcept financialConcept,
                                                   Contact collector, CashRegister cashRegister, DateTime transactionDate,
                                                   decimal amount, decimal taxes, string summary, string notes, int referenceId) {
      AccountTransaction transaction = new AccountTransaction(account, financialConcept);
      transaction.appliedToId = -1;
      transaction.transactionDate = transactionDate;
      transaction.collectedBy = collector;
      transaction.organization = cashRegister.Organization;

      transaction.amount = amount;
      transaction.taxes = taxes;
      transaction.credit = 0m;
      transaction.debit = amount;
      transaction.notes = notes;
      transaction.summary = summary;
      transaction.referenceId = referenceId;

      return transaction;
    }

    #endregion Constructors and parsers

    #region Properties

    public FinancialAccount Account {
      get { return financialAccount; }
    }

    public FinancialConcept FinancialConcept {
      get { return financialConcept; }
    }

    public Organization Organization {
      get { return organization; }
    }

    public int AppliedToId {
      get { return appliedToId; }
      set { appliedToId = value; }
    }

    public Contact CollectedBy {
      get { return collectedBy; }
    }

    public int CRTransactionId {
      get { return crTransactionId; }
      internal set { crTransactionId = value; }
    }

    public int ReferenceId {
      get { return referenceId; }
    }

    public string Summary {
      get { return summary; }
    }

    public string Notes {
      get { return notes; }
    }

    public int AuthorizationId {
      get { return authorizationId; }
    }

    public DateTime TransactionDate {
      get { return transactionDate; }
    }

    public DateTime PostingDate {
      get { return postingDate; }
    }

    public int CanceledById {
      get { return canceledById; }
    }

    public DateTime CancelationDate {
      get { return cancelationDate; }
    }

    public int CurrencyId {
      get { return currencyId; }
    }

    public decimal Amount {
      get { return amount; }
    }

    public decimal Taxes {
      get { return taxes; }
    }

    public decimal Credit {
      get { return credit; }
    }

    public decimal Debit {
      get { return debit; }
    }

    public decimal CreditBalance {
      get { return creditBalance; }
    }

    public Contact PostedBy {
      get { return postedBy; }
      set { postedBy = value; }
    }

    public string Status {
      get { return status; }
      set { status = value; }
    }

    #endregion Properties

    #region Methods

    public void Delete(CollectTotal collectTotal) {
      using (DataWriterContext context = DataWriter.CreateContext("OnDeleteFSMTransaction")) {
        if (collectTotal.Status == "P") {
          context.Add(AccountTransactionData.CancelAccountTransaction(this, Empiria.ExecutionServer.CurrentUserId, DateTime.Now));
          if (this.FinancialConcept.Id == 722) {
            CRTransaction crTransaction = CRTransaction.Parse(this.CRTransactionId);
            crTransaction.Delete();
            //OOJJOO
          }
          context.Update();
          this.Status = "X";
        }
      }
      FinancialAccountData.UpdateAccountsTable(this.Account.Id);
    }

    static private decimal GetTaxPercentage(FinancialConcept financialConcept) {
      if (financialConcept.Id >= 25) {
        return 0.16m;
      } else {
        return 0m;
      }
    }


    public CRTransaction GetCRTransaction() {
      return CRTransaction.Parse(this.CRTransactionId);
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.financialAccount = FinancialAccount.Parse((int) row["FinancialAccountId"]);
      this.financialConcept = FinancialConcept.Parse((int) row["FinancialConceptId"]);
      this.organization = Organization.Parse((int) row["OrganizationId"]);
      this.appliedToId = (int) row["AppliedToId"];
      this.collectedBy = Contact.Parse((int) row["CollectedById"]);
      this.referenceId = (int) row["ReferenceId"];
      this.summary = (string) row["Summary"];
      this.notes = (string) row["Notes"];
      this.authorizationId = (int) row["AuthorizationId"];
      this.transactionDate = (DateTime) row["TransactionDate"];
      this.postingDate = (DateTime) row["PostingDate"];
      this.cancelationDate = (DateTime) row["CancelationDate"];
      this.canceledById = (int) row["CanceledById"];
      this.currencyId = (int) row["CurrencyId"];
      this.amount = (decimal) row["Amount"];
      this.taxes = (decimal) row["Taxes"];
      this.credit = (decimal) row["Credit"];
      this.debit = (decimal) row["Debit"];
      this.creditBalance = (decimal) row["CreditBalance"];
      this.crTransactionId = (int) row["CRTransactionId"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.status = (string) row["TransactionStatus"];
    }

    protected override void OnSave() {
      if (this.IsNew) {
        this.PostedBy = Contact.Parse(ExecutionServer.CurrentUserId);
      }
      AccountTransactionData.WriteAccountTransaction(this);
    }

    #endregion Methods

  } // class AccountTransaction

} // namespace Empiria.FinancialServices
