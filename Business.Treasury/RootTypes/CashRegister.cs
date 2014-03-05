/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : Cashier                                          Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a cashier or cashier supervisor.                                                   *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Contacts;
using Empiria.Security;

namespace Empiria.Treasury {

  public class CashRegister {

    private readonly Organization organization = null;
    private readonly Contact cashier = null;


    private CashRegister(Organization organization, Contact cashier) {
      this.organization = organization;
      this.cashier = cashier;
    }

    static public bool IAmCashier {
      get { 
        return EmpiriaUser.Current.CanExecute("CashRegister");
      }
    }

    static public CashRegister MyCashRegister() {
      return new CashRegister(Organization.Parse(1),
                              Contact.Parse(ExecutionServer.CurrentUserId));
    }

    static internal CashRegister Parse(Organization organization, Contact cashier) {
      Assertion.RequireObject(organization, "organization");
      Assertion.RequireObject(cashier, "cashier");

      return new CashRegister(organization, cashier);
    }

    #region Public properties

    public Contact Cashier {
      get { return cashier; }
    }

    public Organization Organization {
      get { return organization; }
    }

    #endregion Public properties

  } // class Cashier

} // namespace Empiria.Treasury



//public int DoBankDeposit(CRTransactionType transactionType, int accountId, DateTime transactionDate,
//                         DateTime dueDate, string bankSlipTag, decimal totalAmount,
//                         decimal cashAmount, Document[] documents, string notes) {

//  CRTransaction crt = new CRTransaction(transactionType, this);

//  crt.TransactionDate = transactionDate;
//  crt.FinancialAccountId = accountId;
//  crt.ReferenceTag = bankSlipTag;
//  crt.DueDate = dueDate;
//  if (documents.Length != 0) {
//    crt.BaseInstrumentType = InstrumentType.Parse("multiple");
//  } else {
//    crt.BaseInstrumentType = InstrumentType.Parse("cash");      
//  }
//  crt.BaseDocument = Document.NullInstance;
//  crt.CurrencyAmount = totalAmount;
//  crt.Amount = totalAmount;
//  crt.Notes = notes;

//  CRPosting posting = null;

//  if (cashAmount != 0m) {
//    posting  = crt.CreatePosting();
//    posting.InstrumentTypeId = 480;
//    posting.InstrumentAmount = cashAmount;
//    posting.OutputAmount = cashAmount;
//    crt.AppendPosting(posting);
//  }
//  for (int i = 0; i < documents.Length; i++) {
//    posting = crt.CreatePosting();
//    posting.InstrumentTypeId = documents[i].InstrumentTypeId;
//    posting.InstrumentAmount = documents[i].Amount;
//    posting.OutputAmount = documents[i].Amount;

//    documents[i].Status = "D";
//    posting.Document = documents[i];
//    crt.AppendPosting(posting);
//  }
//  crt.Status = "C";
//  crt.Save();

//  return crt.Id;
//}

//public Document CreateCashRegisterSlip(int issuedById, decimal slipTotal,
//                                       DateTime transactionDate, string notes) {
//  Document document = new Document();

//  document.ControlFSMAccountId = -1;
//  document.InstrumentTypeId = InstrumentType.Parse("cashregister.slip").Id;
//  document.InstitutionId = 1;
//  document.Amount = slipTotal;
//  document.DocumentNumber = "";
//  document.IssuedById = issuedById;
//  document.IssueDate = transactionDate;
//  document.ResponsibleId = this.Cashier.Id;
//  document.DueDate = transactionDate.AddDays(4);
//  document.Notes = notes;

//  return document;
//}

//public CRTransaction DoOrderPayment(int gasSaleTotalId, int gasSaleId, int collectorId, int creditAccountId, DateTime transactionDate, 
//                                    decimal total, string summary, string notes) {
//  CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.OrderPayment"), this);

//  crt.FinancialAccountId = creditAccountId;
//  crt.ReferenceParentId = gasSaleTotalId;
//  crt.ReferenceId = gasSaleId;
//  crt.CollectorId = collectorId;      
//  crt.TransactionDate = transactionDate;
//  crt.DueDate = transactionDate;
//  crt.BaseInstrumentType = InstrumentType.Parse("credit");
//  crt.BaseDocument = Document.NullInstance;
//  crt.Amount = total;
//  crt.CurrencyAmount = total;
//  crt.Description = summary;
//  crt.Notes = notes;

//  CRPosting posting = crt.CreatePosting();
//  posting.InstrumentTypeId = InstrumentType.Parse("credit").Id;
//  posting.InstrumentId = creditAccountId;
//  posting.InstrumentAmount = total;
//  posting.InputAmount = total;

//  crt.AppendPosting(posting);

//  crt.Status = "C";

//  //crt.Save();

//  return crt;
//}

//public CRTransaction DoOrderPayment(InstrumentType instrumentType, int gasSaleTotalId, int gasSaleId, int collectorId, DateTime transactionDate,
//                                      decimal total, string summary, string notes, decimal cashPayment) {
//  CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.OrderPayment"), this);

//  crt.ReferenceParentId = gasSaleTotalId;
//  crt.ReferenceId = gasSaleId;
//  crt.CollectorId = collectorId;      
//  crt.TransactionDate = transactionDate;
//  crt.DueDate = transactionDate;
//  if (cashPayment <= decimal.Zero) {
//    crt.BaseInstrumentType = instrumentType;
//  } else {
//    crt.BaseInstrumentType = InstrumentType.Parse("multiple");
//  }

//  crt.BaseDocument = Document.NullInstance;      
//  crt.Amount = total + cashPayment;
//  crt.CurrencyAmount = total + cashPayment;
//  crt.Description = summary;
//  crt.Notes = notes;

//  CRPosting posting = crt.CreatePosting();
//  posting.InstrumentTypeId = instrumentType.Id;
//  posting.InstrumentAmount = total - cashPayment;
//  posting.InputAmount = total - cashPayment;

//  crt.AppendPosting(posting);

//  if (cashPayment < 0m) {
//    CRPosting cashPosting = crt.CreatePosting();
//    cashPosting.InstrumentTypeId = 480;
//    cashPosting.InstrumentAmount = Math.Abs(cashPayment);
//    cashPosting.OutputAmount = Math.Abs(cashPayment);

//    crt.AppendPosting(cashPosting);
//  } else if (cashPayment > 0m) {
//    CRPosting cashPosting = crt.CreatePosting();
//    cashPosting.InstrumentTypeId = 480;
//    cashPosting.InstrumentAmount = cashPayment;
//    cashPosting.InputAmount = cashPayment;

//    crt.AppendPosting(cashPosting);
//  }

//  crt.Status = "C";

//  //crt.Save();

//  return crt;
//}

//public CRTransaction DoOrderPayment(Document document, decimal cashPayment, int gasSaleTotalId, int gasSaleId, 
//                                      int collectorId, DateTime transactionDate, string summary,  string notes) {
//  CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.OrderPayment"), this);

//  crt.ReferenceId = gasSaleId;
//  crt.ReferenceParentId = gasSaleTotalId;
//  crt.CollectorId = collectorId;
//  crt.TransactionDate = transactionDate;
//  crt.DueDate = transactionDate;
//  if (cashPayment <= decimal.Zero) {
//    crt.BaseInstrumentType = InstrumentType.Parse(document.InstrumentTypeId);
//  } else {
//    crt.BaseInstrumentType = InstrumentType.Parse("multiple");
//  }
//  crt.BaseDocument = document;
//  crt.Amount = document.Amount + cashPayment;
//  crt.CurrencyAmount = document.Amount + cashPayment;
//  crt.Description = summary;
//  crt.Notes = notes;

//  CRPosting posting = crt.CreatePosting();
//  posting.InstrumentTypeId = document.InstrumentTypeId;
//  posting.InstrumentAmount = document.Amount;
//  posting.InputAmount = document.Amount;

//  document.Status = "A";
//  posting.Document = document;
//  crt.AppendPosting(posting);

//  if (cashPayment < decimal.Zero) {
//    CRPosting cashPosting = crt.CreatePosting();
//    cashPosting.InstrumentTypeId = 480;
//    cashPosting.InstrumentAmount = Math.Abs(cashPayment);
//    cashPosting.OutputAmount = Math.Abs(cashPayment);

//    crt.AppendPosting(cashPosting);
//  } else if (cashPayment > 0m) {
//    CRPosting cashPosting = crt.CreatePosting();
//    cashPosting.InstrumentTypeId = 480;
//    cashPosting.InstrumentAmount = cashPayment;
//    cashPosting.InputAmount = cashPayment;

//    crt.AppendPosting(cashPosting);
//  }

//  crt.Status = "C";

//  //crt.Save();

//  return crt;
//}

//public int DoSlipPayment(CRTransactionType exchangeTransactionType, int referenceId, int documentId, 
//                         DateTime transactionDate, string notes) {
//  Document document = Document.Parse(documentId);

//  CRTransaction crt = new CRTransaction(exchangeTransactionType, this);
//  crt.TransactionDate = transactionDate;
//  crt.ReferenceId = referenceId;
//  crt.DueDate = transactionDate;
//  crt.BaseInstrumentType = InstrumentType.Parse(480);
//  crt.BaseDocument = document;
//  crt.CurrencyAmount = document.Amount;
//  crt.Amount = document.Amount;
//  crt.Notes = notes;

//  CRPosting posting = crt.CreatePosting();
//  posting.InstrumentTypeId = 480;
//  posting.InstrumentAmount = crt.Amount;
//  posting.InputAmount = crt.Amount;

//  crt.AppendPosting(posting);

//  posting = crt.CreatePosting();
//  posting.InstrumentTypeId = document.InstrumentTypeId;
//  posting.DocumentId = document.Id;
//  posting.InstrumentAmount = crt.CurrencyAmount;
//  posting.OutputAmount = crt.Amount;

//  document.CancelationTime = transactionDate;
//  document.CanceledById = crt.CashRegister.Cashier.Id;
//  document.Status = "C";

//  posting.Document = document;

//  crt.AppendPosting(posting);

//  crt.Status = "C";

//  crt.Save();

//  return crt.Id;
//}

//public int DoCheckReturn(int documentId, decimal commission, DateTime transactionDate, string notes) {
//  Document returnedCheck = Document.Parse(documentId);

//  CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.CheckReturn"), this);
//  crt.TransactionDate = transactionDate;
//  crt.DueDate = transactionDate;
//  crt.BaseInstrumentType = InstrumentType.Parse(488);
//  crt.BaseDocument = returnedCheck;
//  crt.CurrencyAmount = returnedCheck.Amount + commission;
//  crt.Amount = returnedCheck.Amount + commission;
//  crt.Notes = notes;

//  CRPosting posting = crt.CreatePosting();
//  posting.InstrumentTypeId = 488;
//  posting.InstrumentAmount = crt.Amount;
//  posting.InputAmount = crt.Amount;

//  Document slip = CreateCashRegisterSlip(returnedCheck.IssuedById, crt.Amount, transactionDate, notes);

//  slip.AccountNumber = returnedCheck.AccountNumber;
//  slip.DocumentNumber = returnedCheck.DocumentNumber;

//  posting.Document = slip;

//  returnedCheck.CancelationTime = transactionDate;
//  returnedCheck.CanceledById = crt.CashRegister.Cashier.Id;
//  returnedCheck.Status = "R";

//  crt.AppendAdditionalOperation(returnedCheck.SaveOperation());

//  crt.AppendPosting(posting);

//  crt.Status = "C";
//  crt.Save();

//  return crt.Id;
//}