/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : AccountBalancesData                              Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data methods for financial services accounts balances.                               *
*                                                                                                            *
********************************* Copyright (c) 2003-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  static public class AccountBalancesData {

    #region Static fields

    #endregion Static fields

    #region Public methods

    static public DataRow GetBalance(int accountId, DateTime fromDate, DateTime toDate) {      
      var op = DataOperation.Parse("qryFSMAccountBalance", accountId, fromDate, toDate);

      return DataReader.GetDataRow(op);
    }

    static public DataView GetBalances(DateTime fromDate, DateTime toDate, string filter, string sort) {
      DataOperation dataOperation = DataOperation.Parse("qryFSMAccountsBalances", fromDate, toDate);

      return DataReader.GetDataView(dataOperation, filter, sort);
    }

    static public DataTable GetBalancesByManager(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAccountsBalancesByManager", fromDate, toDate));
    }

    static public DateTime GetFirstUnpaidCreditDate(int accountId) {
      DataOperation op = DataOperation.Parse("getFSMAccountFirstUnpaidCreditDate", accountId);

      return DataReader.GetScalar<DateTime>(op, DateTime.Today);
    }

    static public DataTable GetPaymentDistribution(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMPaymentDistribution", fromDate, toDate));
    }

    static public DataTable GetPaymentDistributionItems(int organizationId, DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMPaymentDistributionItems", organizationId, fromDate, toDate));
    }

    static public DataTable GetPaymentDistributionItemsAppliedOn(int organizationId, DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMPaymentDistributionItemsAppliedOn", organizationId, fromDate, toDate));
    }

    static public DataTable GetUnpaidTransactionsTotalsByOrg(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMUnpaidTransactionsTotalsByOrg", fromDate, toDate));
    }

    #endregion Public methods

    #region Internal methods

    static internal DataOperation AppendPaymentDistribution(int creditTransactionId, int debitTransactionId,
                                                            int accountId, int organizationId, DateTime date,
                                                            decimal paymentAmount) {
      return DataOperation.Parse("apdFSMPaymentDistribution", creditTransactionId, debitTransactionId,
                                  accountId, organizationId, date, paymentAmount);
    }

    static internal DataTable GetAccountCredits(int accountId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAccountCredits", accountId));
    }

    static internal DataTable GetAccountDebits(int accountId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAccountDebits", accountId));
    }

    static internal DataRow GetAccountCurrentBalance(int accountId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryFSMAccountCurrentBalance", accountId));
    }

    static internal DataTable GetDirtyBalancesAccounts() {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMDirtyBalancesAccounts"));
    }


    static internal int DeleteAccountPaymentDistribution(int accountId) {
      return DataWriter.Execute(DataOperation.Parse("delFSMAccountPaymentDistribution", accountId));
    }

    static internal DataOperation SetCreditBalance(int transactionId, decimal creditBalance) {
      return DataOperation.Parse("setFSMTransactionCreditBalance", transactionId, creditBalance);
    }

    #endregion Internal methods

  } // class AccountBalancesData

} // namespace Empiria.FinancialServices.Data