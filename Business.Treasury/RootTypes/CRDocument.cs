/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : Document                                         Pattern  : Standard Class                    *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a check, credit note, a prepaid slip or IOU (I Owe You) slips.                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;

namespace Empiria.Treasury {

  public class CRDocument : BaseObject {

    #region Fields

    private InstrumentType instrumentType = InstrumentType.Empty;
    private FinancialInstitution institution = FinancialInstitution.Empty;
    private int accountId = -1;
    private string accountNumber = String.Empty;
    private string documentNumber = String.Empty;
    private Contact issuedBy = Organization.Empty;
    private Contact issuedTo = Organization.Empty;
    private DateTime issueDate = DateTime.Today;
    private DateTime dueDate = DateTime.Today;
    private DateTime cancelationTime = ExecutionServer.DateMaxValue;
    private string notes = String.Empty;
    private string keywords = String.Empty;
    private Currency currency = Currency.Default;
    private decimal amount = 0m;
    private Contact postedBy = Person.Empty;
    private Contact canceledBy = Person.Empty;
    private TreasuryItemStatus status = TreasuryItemStatus.Pending;

    #endregion Fields

    #region Constructors and parsers

    private CRDocument() {
      // Required by Empiria Framework.
    }

    internal CRDocument(InstrumentType type) {
      this.instrumentType = type;
    }

    static public CRDocument Parse(int id) {
      return BaseObject.ParseId<CRDocument>(id);
    }

    static public CRDocument Empty {
      get { return BaseObject.ParseEmpty<CRDocument>(); }
    }

    static public CRDocument CreateCheck(FinancialInstitution institution, string checksAccount, string checkNumber,
                                         Contact issuedBy, Contact issuedTo, DateTime issueDate, DateTime dueDate,
                                         decimal amount, string notes) {
      CRDocument document = new CRDocument(InstrumentType.Parse("check"));
      document.Institution = institution;
      document.AccountNumber = checksAccount;
      document.DocumentNumber = checkNumber;
      document.IssuedBy = issuedBy;
      document.IssuedTo = issuedTo;
      document.IssueDate = issueDate;
      document.DueDate = dueDate;
      document.Amount = amount;
      document.Notes = notes;

      return document;
    }

    static public CRDocument CreateBankCardSlip(InstrumentType instrumentType, FinancialInstitution institution, string bankCardAccount,
                                                string voucherNumber, Contact issuedBy, DateTime issueDate, DateTime dueDate,
                                                decimal amount, string notes) {
      CRDocument document = new CRDocument(instrumentType);
      document.Institution = institution;
      document.AccountNumber = bankCardAccount;
      document.DocumentNumber = voucherNumber;
      document.IssuedBy = issuedBy;
      document.IssuedTo = CashRegister.MyCashRegister().Organization;
      document.IssueDate = issueDate;
      document.DueDate = dueDate;
      document.Amount = amount;
      document.Notes = notes;
      document.Status = TreasuryItemStatus.Pending;

      return document;
    }

    static public CRDocument CreateBankDepositSlip(InstrumentType instrumentType, int accountId, string reference, Contact issuedBy,
                                                   DateTime dueDate, decimal amount, string notes) {
      CRDocument document = new CRDocument(instrumentType);
      if (accountId == 1) {
        document.Institution = FinancialInstitution.Parse(51);
        document.AccountNumber = "0482079690";
      } else if (accountId == 2) {
        document.Institution = FinancialInstitution.Parse(57);
        document.AccountNumber = "12381918";
      }
      document.DocumentNumber = reference;
      document.IssuedBy = issuedBy;
      document.IssuedTo = CashRegister.MyCashRegister().Organization;
      document.DueDate = dueDate;
      document.Amount = amount;
      document.Notes = notes;
      document.Status = TreasuryItemStatus.Pending;

      return document;
    }

    #endregion Constructors and parsers

    #region Public properties

    public InstrumentType InstrumentType {
      get { return instrumentType; }
    }

    public FinancialInstitution Institution {
      get { return institution; }
      set { institution = value; }
    }

    public int AccountId {
      get { return accountId; }
      set { accountId = value; }
    }

    public string AccountNumber {
      get { return accountNumber; }
      set { accountNumber = value; }
    }

    public string DocumentNumber {
      get { return documentNumber; }
      set { documentNumber = value; }
    }

    public Contact IssuedBy {
      get { return issuedBy; }
      set { issuedBy = value; }
    }

    public Contact IssuedTo {
      get { return issuedTo; }
      set { issuedTo = value; }
    }

    public DateTime IssueDate {
      get { return issueDate; }
      set { issueDate = value; }
    }

    public DateTime DueDate {
      get { return dueDate; }
      set { dueDate = value; }
    }

    public DateTime CancelationTime {
      get { return cancelationTime; }
    }

    public string Notes {
      get { return notes; }
      set { notes = value; }
    }

    public string Keywords {
      get { return keywords; }
    }

    public Currency Currency {
      get { return currency; }
      set { currency = value; }
    }

    public decimal Amount {
      get { return amount; }
      set { amount = value; }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public Contact CanceledBy {
      get { return canceledBy; }
    }

    public TreasuryItemStatus Status {
      get { return status; }
      internal set { status = value; }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnLoadObjectData(DataRow row) {
      this.instrumentType = InstrumentType.Parse((int) row["InstrumentTypeId"]);
      this.institution = FinancialInstitution.Parse((int) row["InstitutionId"]);
      this.accountId = (int) row["FinancialAccountId"];
      this.accountNumber = (string) row["AccountNumber"];
      this.documentNumber = (string) row["DocumentNumber"];
      this.issuedBy = Contact.Parse((int) row["IssuedById"]);
      this.issuedTo = Contact.Parse((int) row["IssuedToId"]);
      this.issueDate = (DateTime) row["IssueTime"];
      this.dueDate = (DateTime) row["DueDate"];
      this.cancelationTime = (DateTime) row["CancelationTime"];
      this.notes = (string) row["Notes"];
      this.keywords = (string) row["Keywords"];
      this.currency = Currency.Parse((int) row["CurrencyId"]);
      this.amount = (decimal) row["Amount"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.canceledBy = Contact.Parse((int) row["CanceledById"]);
      this.status = (TreasuryItemStatus) Convert.ToChar(row["DocumentStatus"]);
    }

    #endregion Public methods

  } // class Document

} // namespace Empiria.Treasury
