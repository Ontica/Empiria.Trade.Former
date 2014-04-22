/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : CRTransaction                                    Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a Cash Register Transaction.                                                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.DataTypes;

using Empiria.Treasury.Data;

namespace Empiria.Treasury {

  public enum TreasuryItemStatus {
    Pending = 'P',
    Closed = 'C',
    Deleted = 'X'
  }

  public class CRTransaction : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.CashRegisterTransaction";

    private CashRegister cashRegister = null;
    private CRTransactionType transactionType = null;
    private Contact collector = Person.Empty;
    private int referenceId = -1;
    private string referenceTag = String.Empty;
    private int financialAccountId = -1;
    private int authorizationId = -1;
    private InstrumentType baseInstrumentType = InstrumentType.Empty;
    private DateTime transactionDate = DateTime.Today;
    private DateTime dueDate = DateTime.Today;
    private Currency currency = Currency.Default;
    private decimal currencyAmount = 0m;
    private decimal inputAmount = 0m;
    private decimal outputAmount = 0m;
    private string summary = String.Empty;
    private string notes = String.Empty;
    private string keywords = String.Empty;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private Contact canceledBy = Person.Empty;
    private DateTime cancelationTime = ExecutionServer.DateMaxValue;
    private TreasuryItemStatus status = TreasuryItemStatus.Pending;

    private CRPostingList postings = null;

    #endregion Fields

    #region Constructors and parsers

    protected CRTransaction()
      : base(thisTypeName) {

    }

    protected CRTransaction(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    public CRTransaction(CRTransactionType transactionType, CashRegister cashier)
      : base(thisTypeName) {
      this.transactionType = transactionType;
      this.cashRegister = cashier;
    }

    public CRTransaction(CRTransactionType transactionType,
                         BaseObject reference, DateTime transactionDate,
                         DateTime dueDate, decimal inputAmount, decimal outputAmount, string summary,
                         string notes)
      : base(thisTypeName) {
      this.transactionType = transactionType;
      this.baseInstrumentType = InstrumentType.Empty;
      this.cashRegister = CashRegister.MyCashRegister();
      this.referenceId = reference.Id;
      this.transactionDate = transactionDate;
      this.dueDate = dueDate;
      this.currencyAmount = inputAmount + outputAmount;
      this.inputAmount = inputAmount;
      this.outputAmount = outputAmount;
      this.summary = summary;
      this.notes = notes;
    }

    static public CRTransaction Parse(int id) {
      return BaseObject.Parse<CRTransaction>(thisTypeName, id);
    }

    static internal CRTransaction Parse(DataRow dataRow) {
      return BaseObject.Parse<CRTransaction>(thisTypeName, dataRow);
    }

    static public CRTransaction Empty {
      get { return BaseObject.ParseEmpty<CRTransaction>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public CashRegister CashRegister {
      get { return cashRegister; }
    }

    public CRTransactionType TransactionType {
      get { return transactionType; }
    }

    public Contact Collector {
      get { return collector; }
      set { collector = value; }
    }

    public int ReferenceId {
      get { return referenceId; }
      set { referenceId = value; }
    }

    public string ReferenceTag {
      get { return referenceTag; }
      set { referenceTag = value; }
    }

    public int FinancialAccountId {
      get { return financialAccountId; }
      set { financialAccountId = value; }
    }

    public int AuthorizationId {
      get { return authorizationId; }
      set { authorizationId = value; }
    }

    public InstrumentType BaseInstrumentType {
      get { return baseInstrumentType; }
      set { baseInstrumentType = value; }
    }

    public DateTime TransactionDate {
      get { return transactionDate; }
      set { transactionDate = value; }
    }

    public DateTime DueDate {
      get { return dueDate; }
      set { dueDate = value; }
    }

    public Currency Currency {
      get { return currency; }
      set { currency = value; }
    }

    public decimal CurrencyAmount {
      get { return currencyAmount; }
      set { currencyAmount = value; }
    }

    public decimal Amount {
      get { return inputAmount + outputAmount; }
    }

    public decimal InputAmount {
      get { return inputAmount; }
    }

    public decimal OutputAmount {
      get { return outputAmount; }
    }

    public string Summary {
      get { return summary; }
      set { summary = value; }
    }

    public string Notes {
      get { return notes; }
      set { notes = value; }
    }

    public string Keywords {
      get { return keywords; }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public DateTime PostingTime {
      get { return postingTime; }
    }

    public Contact CanceledBy {
      get { return canceledBy; }
    }

    public DateTime CancelationTime {
      get { return cancelationTime; }
    }

    public TreasuryItemStatus Status {
      get { return status; }
      internal set { status = value; }
    }

    public CRPostingList Postings {
      get {
        if (postings == null) {
          postings = CRTransactionData.GetCRTransactionPostings(this);
        }
        return postings;
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.cashRegister = CashRegister.Parse(Organization.Parse((int) row["OrganizationId"]),
                                             Contact.Parse((int) row["CashierId"]));
      this.transactionType = CRTransactionType.Parse((int) row["TransactionTypeId"]);
      this.collector = Contact.Parse((int) row["CollectorId"]);
      this.referenceId = (int) row["ReferenceId"];
      this.referenceTag = (string) row["ReferenceTag"];
      this.financialAccountId = (int) row["FinancialAccountId"];
      this.authorizationId = (int) row["AuthorizationId"];
      this.baseInstrumentType = InstrumentType.Parse((int) row["BaseInstrumentTypeId"]);
      this.transactionDate = (DateTime) row["TransactionDate"];
      this.dueDate = (DateTime) row["DueDate"];
      this.currency = Currency.Parse((int) row["CurrencyId"]);
      this.currencyAmount = (decimal) row["CurrencyAmount"];
      this.inputAmount = (decimal) row["InputAmount"];
      this.outputAmount = (decimal) row["OutputAmount"];
      this.summary = (string) row["Summary"];
      this.notes = (string) row["Notes"];
      this.keywords = (string) row["TransactionKeywords"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.postingTime = (DateTime) row["PostingTime"];
      this.canceledBy = Contact.Parse((int) row["CanceledById"]);
      this.cancelationTime = (DateTime) row["CancelationTime"];
      this.status = (TreasuryItemStatus) Convert.ToChar(row["TransactionStatus"]);
    }

    protected override void ImplementsSave() {
      if (postings.Count == 0) {
        throw new NotImplementedException();
      }
      using (DataWriterContext context = DataWriter.CreateContext("SaveCRTransaction")) {
        ITransaction transaction = context.BeginTransaction();
        context.Add(CRTransactionData.GetWriteCRTransactionOperation(this));
        foreach (CRPosting posting in this.Postings) {
          posting.Save();
          if (!posting.Document.IsEmptyInstance) {
            posting.Document.Save();
            context.Add(CRTransactionData.GetWriteCRDocumentOperation(posting.Document));
          }
          context.Add(CRTransactionData.GetWriteCRPostingOperation(posting));
        }
        context.Update();
        transaction.Commit();
      }
    }

    #endregion Public methods

    public string GetDigitalSign() {
      string s = GetDigitalString();

      return Empiria.Security.Cryptographer.CreateDigitalSign(s);
    }

    public string GetDigitalString() {
      return EmpiriaString.BuildDigitalString(this.Id, transactionType.Id, Collector.Id, ReferenceId,
                                              ReferenceTag, FinancialAccountId, AuthorizationId,
                                              BaseInstrumentType.Id, TransactionDate, DueDate,
                                              Currency.Id, CurrencyAmount, Amount, InputAmount, OutputAmount,
                                              Summary, Notes, Keywords, PostedBy.Id, PostingTime);
    }

    public CRPosting AppendPosting(InstrumentType instrumentType, CRDocument document, decimal amount) {
      CRPosting posting = new CRPosting(this, instrumentType, document, amount);
      this.Postings.Add(posting);

      return posting;
    }

    public CRPosting AppendPosting(InstrumentType instrumentType, IFinancialAccount account, decimal amount) {
      CRPosting posting = new CRPosting(this, instrumentType, account, amount);
      this.Postings.Add(posting);

      return posting;
    }

    public bool RemovePosting(CRPosting posting) {
      return this.Postings.Remove(posting);
    }

    public void Close() {
      foreach (CRPosting posting in this.Postings) {
        if (!posting.Document.IsEmptyInstance) {
          posting.Document.Status = TreasuryItemStatus.Closed;
          posting.Document.Save();
        }
        posting.Status = TreasuryItemStatus.Closed;
        posting.Save();
      }
      this.Status = TreasuryItemStatus.Closed;
      this.Save();
    }

    public void Delete() {
      foreach (CRPosting posting in this.Postings) {
        if (!posting.Document.IsEmptyInstance) {
          posting.Document.Status = TreasuryItemStatus.Deleted;
          posting.Document.Save();
        }
        posting.Status = TreasuryItemStatus.Deleted;
        posting.Save();
      }
      this.Status = TreasuryItemStatus.Deleted;
      this.Save();
    }

    //public void Save(DataWriterContext context) {
    //  if (postings.Count == 0) {
    //    throw new NotImplementedException();
    //  }
    //  if (Id == 0) {
    //    this.Id = CRTransactionData.GetNextCRTransactionId();
    //  }
    //  context.Add(CRTransactionData.WriteTransaction(this));
    //  for (int i = 0; i < postings.Count; i++) {
    //    context.Add(postings[i].SaveOperations());
    //  }
    //}

    //public DataOperationList SaveOperations() {
    //  if (postings.Count == 0) {
    //    throw new NotImplementedException();
    //  }
    //  DataOperationList operations = new DataOperationList("SaveTMSCRTransaction");

    //  operations.Add(CRTransactionData.WriteTransaction(this));

    //  for (int i = 0; i < postings.Count; i++) {
    //    operations.Add(postings[i].SaveOperations());
    //  }

    //  return operations;
    //}

  } // class CRTransaction

} // namespace Empiria.Treasury