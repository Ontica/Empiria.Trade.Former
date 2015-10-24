/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Financial Services                *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : AccountTransactionData                           Pattern  : Data Services Static Class        *
*  Version   : 2.0                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides data methods for financial services accounts debit or credit transactions.           *
*                                                                                                            *
********************************* Copyright (c) 2003-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  /// <summary>Provides data methods for financial services accounts debit or credit transactions.</summary>
  static public class AccountTransactionData {

    #region Public methods

    static public DataRow GetAccountTransaction(int transactionId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryFSMTransaction", transactionId));
    }

    static public DataTable GetGroupedTransactions(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMGroupedTransactions", fromDate, toDate));
    }

    static public DataTable GetTransactionApplications(int transactionId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMTransactionApplications", transactionId));
    }

    static public DataTable GetTransactions(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMTransactions", fromDate, toDate));
    }

    static public DataView GetTransactions(DateTime fromDate, DateTime toDate, string filter, string sort) {
      return DataReader.GetDataView(DataOperation.Parse("qryFSMTransactions", fromDate, toDate), filter, sort);
    }

    static public DataView GetUnpayedTransactions(DateTime dateTime, string filter, string sort) {
      return DataReader.GetDataView(DataOperation.Parse("qryFSMUnpayedTransactions", dateTime), filter, sort);
    }

    #endregion Public methods

    #region Internal methods

    static internal int GetNextAccountTransactionId() {
      return DataWriter.CreateId("FSMTransactions");
    }

    static internal DataOperation CancelAccountTransaction(AccountTransaction transaction, int currentUserId,
                                                           DateTime date) {
      return DataOperation.Parse("setFSMAccountTransactionStatus", transaction.Id, currentUserId, date, 'X');
    }

    static internal DataOperation GetWriteAccountTransaction(AccountTransaction o) {
      return DataOperation.Parse("writeFSMTransaction", o.Id,
                                o.Account.Id, o.FinancialConcept.Id, o.Organization.Id, o.AppliedToId,
                                o.CollectedBy.Id, o.PostedBy.Id, o.ReferenceId, o.Summary, o.Notes, o.AuthorizationId,
                                o.TransactionDate, o.PostingDate, o.CancelationDate, o.CanceledById,
                                o.CurrencyId, o.Amount, 0.16m, o.Taxes, o.Credit, o.Debit,
                                o.CreditBalance, o.CRTransactionId, o.Status);
    }

    static internal int WriteAccountTransaction(AccountTransaction o) {
      DataOperation operation = DataOperation.Parse("writeFSMTransaction", o.Id,
                                o.Account.Id, o.FinancialConcept.Id, o.Organization.Id, o.AppliedToId,
                                o.CollectedBy.Id, o.PostedBy.Id, o.ReferenceId, o.Summary, o.Notes, o.AuthorizationId,
                                o.TransactionDate, o.PostingDate, o.CancelationDate, o.CanceledById,
                                o.CurrencyId, o.Amount, 0.16m, o.Taxes, o.Credit, o.Debit,
                                o.CreditBalance, o.CRTransactionId, o.Status);

      int i = DataWriter.Execute(operation);

      FinancialAccountData.UpdateAccountsTable(o.Account.Id);

      return i;
    }

    #endregion Internal methods

  } // class FinancialAccountData

} // namespace Empiria.FinancialServices.Data
