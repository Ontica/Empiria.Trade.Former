/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : CRTransactionData                                Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data read and write methods for cash register transactions.                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Treasury.Data {

  static public class CRTransactionData {

    #region Public methods

    static public DataTable GetMyCashRegisterOrganizations() {
      return DataReader.GetDataTable(DataOperation.Parse("qryMyOrganizations", Empiria.ExecutionServer.CurrentUserId, "Cashier"));
    }

    static public DataTable GetOrganizationBalances(int organizationId,
                                                    DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRBalances", organizationId, fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetOrganizationDocuments(int organizationId,
                                                    DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRDocuments", organizationId, fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetDocumentsBalances(DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRDocumentsBalances", fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetTransactions(DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRTransactions", fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetTransactionsByInstrumentType(DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRTransactionsByInstrType", fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetTransactionsByInstrumentTypeGrouped(DateTime fromDate, DateTime toDate) {
      DataOperation operation = DataOperation.Parse("qryTMSCRTransactionsByInstrTypeGrouped", fromDate, toDate);

      return DataReader.GetDataTable(operation);
    }

    #endregion Public methods

    #region Internal methods

    static internal DataOperation CancelDocument(CRDocument document, int userId, DateTime cancelationTime) {
      return DataOperation.Parse("doTMSCancelDocument", document.Id, userId, cancelationTime, "X");
    }

    static internal DataOperation CancelTransaction(CRTransaction transaction, int userId, DateTime cancelationTime) {
      return DataOperation.Parse("doTMSCancelTransaction", transaction.Id, userId, cancelationTime, "X");
    }

    static internal CRPostingList GetCRTransactionPostings(CRTransaction transaction) {
      DataOperation dataOperation = DataOperation.Parse("qryTMSCRTransactionPostings", transaction.Id);

      DataView view = DataReader.GetDataView(dataOperation);

      return new CRPostingList((x) => CRPosting.Parse(x), view);
    }

    static internal DataRow GetDocument(int documentId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryTMSDocument", documentId));
    }

    static internal DataRow GetCRPosting(int postingId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryTMSCRPosting", postingId));
    }

    static internal DataRow GetCRTransaction(int transactionId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryTMSCRTransaction", transactionId));
    }

    //static internal int GetNextDocumentId() {
    //  return DataWriter.CreateId("TMSDocuments");
    //}

    //static internal int GetNextCRTransactionId() {
    //  return DataWriter.CreateId("TMSCRTransactions");
    //}

    //static internal int GetNextCRPostingId() {
    //  return DataWriter.CreateId("TMSCRPostings");
    //}

    static internal DataOperation SetDocumentStatus(CRDocument document, string newStatus) {
      return DataOperation.Parse("setTMSDocumentStatus", document.Id, newStatus);
    }

    //static internal DataOperation SetPostingStatus(CRPosting posting, string newStatus) {
    //  return DataOperation.Parse("setTMSCRPostingStatus", posting.Id, newStatus);
    //}

    static internal DataOperation SetTransactionStatus(CRTransaction transaction, string newStatus) {
      return DataOperation.Parse("setTMSCRTransactionStatus", transaction.Id, newStatus);
    }

    static internal DataOperation GetWriteCRDocumentOperation(CRDocument o) {
      return DataOperation.Parse("writeTMSDocument", o.Id,
                                 o.AccountId, o.InstrumentType.Id,
                                 o.Institution.Id, o.AccountNumber, o.DocumentNumber,
                                 o.IssuedBy.Id, o.IssuedTo.Id, o.IssueDate, o.DueDate,
                                 o.CancelationTime, o.Notes, o.Keywords,
                                 o.Currency.Id, o.Amount, o.PostedBy.Id, o.CanceledBy.Id, (char) o.Status);
    }

    static internal DataOperation GetWriteCRPostingOperation(CRPosting o) {
      return DataOperation.Parse("writeTMSCRPosting", o.Id,
                                 o.Transaction.Id, o.InstrumentType.Id, o.InstrumentId,
                                 o.Document.Id, o.Currency.Id, o.InstrumentAmount,
                                 o.InputAmount, o.OutputAmount, (char) o.Status);
    }

    static internal DataOperation GetWriteCRTransactionOperation(CRTransaction o) {
      return DataOperation.Parse("writeTMSCRTransaction", o.Id,
                                 o.CashRegister.Organization.Id, o.CashRegister.Cashier.Id, o.TransactionType.Id,
                                 o.Collector.Id, o.ReferenceId, o.ReferenceTag, o.FinancialAccountId,
                                 o.AuthorizationId, o.BaseInstrumentType.Id, o.TransactionDate, o.DueDate,
                                 o.Currency.Id, o.CurrencyAmount, o.InputAmount, o.OutputAmount,
                                 o.Summary, o.Notes, o.Keywords, o.PostedBy.Id, o.PostingTime,
                                 o.CanceledBy.Id, o.CancelationTime, o.Status);
    }

    #endregion Internal methods

  } // class CRTransactionData

} // namespace Empiria.Treasury
