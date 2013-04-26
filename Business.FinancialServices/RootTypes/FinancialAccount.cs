/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialAccount                                 Pattern  : Business Services Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a debit o credit financial services account.                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.DataTypes;

using Empiria.FinancialServices.Data;
using Empiria.Treasury;

namespace Empiria.FinancialServices {

  public class FinancialAccount : BaseObject, IFinancialAccount {

    #region Fields

    public static readonly string HtmlFilesFolder = ConfigurationData.GetString("HtmlFilesFolder");
    public static readonly string PDFFilesFolder = ConfigurationData.GetString("PDFFilesFolder");
    public static readonly string UrlHtmlFilesFolder = ConfigurationData.GetString("UrlHtmlFilesFolder");
    public static readonly bool SendFilesOnlyToDefaultEmail = ConfigurationData.GetBoolean("SendFilesOnlyToDefaultEmail");
    public static readonly bool UseBalanceCalculation = ConfigurationData.GetBoolean("UsesBalancesCalculation");

    private const string thisTypeName = "ObjectType.FinancialServicesAccount";
    private FinancialProduct financialProduct = FinancialProduct.Default;
    private FinancialInstitution institution = FinancialInstitution.Default;
    private Contact customer = Person.Empty;
    public int SellerId = -1;
    public int SalesChannelId = 881;
    public int AuthorizedById = -1;
    public int ManagerId = -1;
    public int ExecutiveId = -1;
    public int CollectorId = -1;
    public string ContractNumber = String.Empty;
    public DateTime ContractDate = ExecutionServer.DateMaxValue;
    public string InstitutionCustomerNumber = String.Empty;
    public string AccountNumber = String.Empty;
    public string InterBankAccountNumber = String.Empty;
    public string Notes = String.Empty;
    public string Keywords = String.Empty;
    private Currency baseCurrency = Currency.Default;
    public decimal RequestedLimit = 0m;
    public decimal AuthorizedLimit = 0m;
    public short BillingCycle = 8;
    public string BillingMode = "N";
    public short BillingWeekDays = 5;
    public string BillingNotes = String.Empty;
    public short CollectCycle = 8;
    public string CollectMode = "N";
    public short CollectWeekDays = 5;
    public string CollectNotes = String.Empty;
    public string DocumentationVector = String.Empty;
    public short Score = 0;
    public DateTime RequestedDate = DateTime.Today;
    public DateTime OpeningDate = DateTime.Today;
    public DateTime ClosingDate = ExecutionServer.DateMaxValue;
    public int PostedById = ExecutionServer.CurrentUserId;
    public int ReplacedById = 0;
    public string Status = "P";
    public DateTime StartDate = DateTime.Today;
    public DateTime EndDate = ExecutionServer.DateMaxValue;

    #endregion Fields

    #region Constructors and parsers

    public FinancialAccount(Contact customer)
      : base(thisTypeName) {
      this.customer = customer;
    }

    protected FinancialAccount(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public FinancialAccount Parse(int id) {
      return BaseObject.Parse<FinancialAccount>(thisTypeName, id);
    }

    static internal FinancialAccount Parse(DataRow dataRow) {
      return BaseObject.Parse<FinancialAccount>(thisTypeName, dataRow);
    }

    static public FinancialAccount Empty {
      get { return BaseObject.ParseEmpty<FinancialAccount>(thisTypeName); }
    }

    static public FinancialAccount GetForCustomer(Contact contact) {
      DataRow row = FinancialAccountData.GetAccount(contact.Id);
      if (row != null) {
        return FinancialAccount.Parse(row);
      } else {
        return FinancialAccount.Empty;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public FinancialProduct FinancialProduct {
      get { return financialProduct; }
    }

    public FinancialInstitution Institution {
      get { return institution; }
      set { institution = value; }
    }

    public Currency BaseCurrency {
      get { return baseCurrency; }
      set { baseCurrency = value; }
    }


    public Contact Customer {
      get { return customer; }
    }

    public string StatusName {
      get {
        switch (this.Status) {
          case "A":
            return "Cuenta activa";
          case "S":
            return "Cuenta suspendida";
          case "R":
            return "Cuenta en revisión";
          case "C":
            return "Cuenta cancelada";
          case "P":
            return "En trámite";
          case "J":
            return "En jurídico";
          case "F":
            return "Cortesía";
          default:
            return "Estado desconocido";
        } // switch
      } // get
    }

    #endregion Public properties

    #region Public methods

    public AccountTransaction CreateConcept(FinancialConcept concept, CashRegister cashRegister, DateTime transactionDate,
                                            decimal amount, string summary, string notes, int referenceId) {
      AccountTransaction transaction = null;
      if (concept.AppliesToCredit) {
        transaction = AccountTransaction.CreateCredit(this, concept, cashRegister.Organization, transactionDate,
                                                      amount, (amount - (amount / 1.16m)), summary, notes, referenceId);
      } else if (concept.AppliesToDebit) {
        transaction = AccountTransaction.CreateDebit(this, concept, Person.Empty, cashRegister, transactionDate,
                                                     amount, (amount - (amount / 1.16m)), summary, notes, referenceId);
      }
      transaction.Save();

      return transaction;
    }

    public CRTransaction CreatePayment(Contact collector, DateTime transactionDate,
                                       DateTime dueDate, decimal amount, string notes) {
      CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.CreditPayment"), this,
                                            transactionDate, dueDate, amount, 0m,
                                            "Pago a cuenta de crédito: " + this.AccountNumber, notes);
      crt.Collector = collector;
      //CRPosting posting = crt.CreatePosting();
      //posting.InstrumentType = instrumentType;
      //posting.InstrumentAmount = crt.Amount;
      //posting.InputAmount = crt.Amount;

      //crt.Status = TreasuryItemStatus.Closed;

      return crt;
    }

    public AccountTransaction DoCharge(CRTransaction crTransaction) {
      string summary = String.Empty;
      FinancialConcept concept = FinancialConcept.Parse("FSP001");
      AccountTransaction accountTransaction = AccountTransaction.CreateDebit(this, concept, crTransaction.Collector, crTransaction.CashRegister,
                                                                             crTransaction.TransactionDate, crTransaction.InputAmount,
                                                                             0m, crTransaction.Summary, crTransaction.Notes, -1);


      //if (document != null) {
      //  crTransaction = this.DoCreditAccountPayment(accountTransaction, document, summary, notes);
      //} else {
      //  crTransaction = this.DoCreditAccountPayment(accountTransaction, instrumentType, summary, notes);
      //}
      crTransaction.ReferenceId = accountTransaction.Id;
      crTransaction.ReferenceTag = this.AccountNumber;
      using (DataWriterContext context = DataWriter.CreateContext("OnCreateFSMTransaction")) {
        ITransaction transaction = context.BeginTransaction();

        accountTransaction.Save();
        crTransaction.Close();
        accountTransaction.CRTransactionId = crTransaction.Id;
        context.Add(AccountTransactionData.GetWriteAccountTransaction(accountTransaction));
        context.Update();
        transaction.Commit();
      }
      FinancialAccountData.UpdateAccountsTable(this.Id);

      return accountTransaction;
    }

    public void SendStatementToEmail(System.IO.FileInfo pdfFile, DateTime fromDate, DateTime toDate) {
      System.IO.FileInfo[] files = new System.IO.FileInfo[1];
      files[0] = pdfFile;

      string eMail = this.Customer.EMail;
      if (!SendFilesOnlyToDefaultEmail) {
        Empiria.Messaging.EMail.Send(eMail, GetMailSubject(), GetMailBody(fromDate, toDate), files);
      } else {
        Empiria.Messaging.EMail.Send("jmcota@ontica.org", GetMailSubject(), GetMailBody(fromDate, toDate), files);
      }
    }

    private string GetMailSubject() {
      return "Su estado de cuenta de crédito Pineda";
    }

    private string GetMailBody(DateTime fromDate, DateTime toDate) {
      string body = String.Empty;

      body = "Estimado cliente:" + System.Environment.NewLine + System.Environment.NewLine;

      body += "Nos permitimos enviarle el estado de cuenta con los movimientos de su cuenta de crédito:" +
              System.Environment.NewLine + System.Environment.NewLine;

      body += "Razón social: " + this.Customer.FullName + System.Environment.NewLine;
      body += "Cuenta de crédito: " + this.AccountNumber + System.Environment.NewLine;
      body += "Movimientos del día " + fromDate.ToString("dd/MMM/yyyy") + " al día " +
              toDate.ToString("dd/MMM/yyyy") + System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Le agradecemos su preferencia y quedamos a sus órdenes para cualquier asunto que requieran.";
      body += System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Atentamente,";
      body += System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Auto Refacciones Pineda, S.A. de C.V." + System.Environment.NewLine;
      body += "pineda@masautopartes.com.mx" + System.Environment.NewLine;
      body += "(55) 2973-0249 / 1997-9360" + System.Environment.NewLine;

      return body;
    }

    public AccountTransaction DoPayment(CRTransaction crTransaction) {
      string summary = String.Empty;
      FinancialConcept concept = FinancialConcept.Parse("FSP001");
      AccountTransaction accountTransaction = AccountTransaction.CreateDebit(this, concept, crTransaction.Collector, crTransaction.CashRegister,
                                                                             crTransaction.TransactionDate, crTransaction.InputAmount,
                                                                             0m, crTransaction.Summary, crTransaction.Notes, -1);


      //if (document != null) {
      //  crTransaction = this.DoCreditAccountPayment(accountTransaction, document, summary, notes);
      //} else {
      //  crTransaction = this.DoCreditAccountPayment(accountTransaction, instrumentType, summary, notes);
      //}
      crTransaction.ReferenceId = accountTransaction.Id;
      crTransaction.ReferenceTag = this.AccountNumber;
      using (DataWriterContext context = DataWriter.CreateContext("OnCreateFSMTransaction")) {
        ITransaction transaction = context.BeginTransaction();

        accountTransaction.Save();
        crTransaction.Close();
        accountTransaction.CRTransactionId = crTransaction.Id;
        context.Add(AccountTransactionData.GetWriteAccountTransaction(accountTransaction));
        context.Update();
        transaction.Commit();
      }
      FinancialAccountData.UpdateAccountsTable(this.Id);

      return accountTransaction;
    }

    //private CRTransaction DoCreditAccountPayment(AccountTransaction transaction, CRDocument document,
    //                                             string summary, string notes) {
    //  CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.CreditPayment"), 
    //                                        CashRegister.MyCashRegister(), transaction);

    //  crt.DueDate = document.DueDate;
    //  crt.BaseInstrumentType = document.InstrumentType;
    //  CRPosting posting = crt.CreatePosting();
    //  posting.InstrumentType = document.InstrumentType;
    //  posting.InstrumentAmount = crt.Amount;
    //  posting.InputAmount = crt.Amount;

    //  posting.Document = document;

    //  //crt.Status = TreasuryItemStatus.Closed;

    //  return crt;
    //}

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.financialProduct = FinancialProduct.Parse((int) row["FinancialProductId"]);
      this.institution = FinancialInstitution.Parse((int) row["InstitutionId"]);
      this.customer = Contact.Parse((int) row["CustomerId"]);
      this.SellerId = (int) row["SellerId"];
      this.SalesChannelId = (int) row["SalesChannelId"];
      this.AuthorizedById = (int) row["AuthorizedById"];
      this.ManagerId = (int) row["ManagerId"];
      this.ExecutiveId = (int) row["ExecutiveId"];
      this.CollectorId = (int) row["CollectorId"];
      this.ContractNumber = (string) row["ContractNumber"];
      this.ContractDate = (DateTime) row["ContractDate"];
      this.InstitutionCustomerNumber = (string) row["InstitutionCustomerNumber"];
      this.AccountNumber = (string) row["AccountNumber"];
      this.InterBankAccountNumber = (string) row["InterBankAccountNumber"];
      this.Notes = (string) row["AccountNotes"];
      this.Keywords = (string) row["Keywords"];
      this.baseCurrency = Currency.Parse((int) row["BaseCurrencyId"]);
      this.RequestedLimit = (decimal) row["RequestedLimit"];
      this.AuthorizedLimit = (decimal) row["AuthorizedLimit"];
      this.BillingCycle = (short) row["BillingCycle"];
      this.BillingMode = (string) row["BillingMode"];
      this.BillingWeekDays = (short) row["BillingWeekDays"];
      this.BillingNotes = (string) row["BillingNotes"];
      this.CollectCycle = (short) row["CollectCycle"];
      this.CollectMode = (string) row["CollectMode"];
      this.CollectWeekDays = (short) row["CollectWeekDays"];
      this.CollectNotes = (string) row["CollectNotes"];
      this.DocumentationVector = (string) row["DocumentationVector"];
      this.Score = (short) row["Score"];
      this.RequestedDate = (DateTime) row["RequestedDate"];
      this.OpeningDate = (DateTime) row["OpeningDate"];
      this.ClosingDate = (DateTime) row["ClosingDate"];
      this.PostedById = (int) row["PostedById"];
      this.ReplacedById = (int) row["ReplacedById"];
      this.Status = (string) row["FinancialAccountStatus"];
      this.StartDate = (DateTime) row["StartDate"];
      this.EndDate = (DateTime) row["EndDate"];
    }

    protected override void ImplementsSave() {
      this.Keywords = Customer.Keywords;
      FinancialAccountData.WriteFinancialAccount(this);
    }

    public void ChangeStatus(string newStatus) {
      FinancialAccountData.ChangeAccountStatus(this, newStatus);
    }

    public DateTime FirstUnpaidCreditDate() {
      return AccountBalancesData.GetFirstUnpaidCreditDate(this.Id);
    }

    public void RebuildCreditBalances() {
      try {
        if (!UseBalanceCalculation) {
          return;
        }
        AccountCreditBalance balance = new AccountCreditBalance(this.Id);
        if (balance.NeedsRebuild()) {
          balance.Rebuild();
        }
        FinancialAccountData.UpdateAccountsTable(this.Id);
      } catch (Exception innerException) {
        FinancialServicesException exception =
              new FinancialServicesException(FinancialServicesException.Msg.CreditBalanceRebuildFails, innerException, this.Id);
        exception.Publish();
        throw exception;
      }
    }

    public void RebuildCreditBalancesForced() {
      try {
        if (!UseBalanceCalculation) {
          return;
        }
        AccountCreditBalance balance = new AccountCreditBalance(this.Id);
        balance.Rebuild();
        FinancialAccountData.UpdateAccountsTable(this.Id);
      } catch (Exception innerException) {
        FinancialServicesException exception =
              new FinancialServicesException(FinancialServicesException.Msg.CreditBalanceRebuildFails, innerException, this.Id);
        exception.Publish();
        throw exception;
      }
    }

    static public void RebuildAllCreditBalances() {
      if (!UseBalanceCalculation) {
        return;
      }
      int accountId = 0;
      try {
        DataTable accounts = AccountBalancesData.GetDirtyBalancesAccounts();

        for (int i = 0; i < accounts.Rows.Count; i++) {
          accountId = (int) accounts.Rows[i]["FinancialAccountId"];
          AccountCreditBalance balance = new AccountCreditBalance(accountId);
          balance.Rebuild();
        }
        FinancialAccountData.ResetAccountsTable();
        DataReader.Optimize();
      } catch (Exception innerException) {
        FinancialServicesException exception =
              new FinancialServicesException(FinancialServicesException.Msg.CreditBalanceRebuildFails, innerException, accountId);
        exception.Publish();
        throw exception;
      }
      //ThreadStart job = new ThreadStart(RebuildAllCreditBalancesJob);
      //Thread thread = new Thread(job);
      //thread.Start(); 
    }

    #endregion Public methods

  } // class FinancialAccount

} // namespace Empiria.FinancialServices