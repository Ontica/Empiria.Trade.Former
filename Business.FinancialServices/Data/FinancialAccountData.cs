/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialAccountData                             Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data methods for Financial Services Accounts.                                        *
*                                                                                                            *
********************************* Copyright (c) 2003-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  static public class FinancialAccountData {

    #region Static fields

    #endregion Static fields

    #region Public methods

    static public DataTable GetOrganizations() {
      return DataReader.GetDataTable(DataOperation.Parse("qryOrganizationsAndParents"));
    }

    static public DataTable GetMyOrganizations() {
      if (ExecutionServer.CurrentPrincipal.IsInRole("FinancialServicesManager") || ExecutionServer.CurrentPrincipal.IsInRole("Accountant")) {
        return GetOrganizations();
      }
      return DataReader.GetDataTable(DataOperation.Parse("qryMyOrganizationsAndParents", Empiria.ExecutionServer.CurrentUserId, "CreditAnalyzer"));
    }

    static public DataTable GetCreditSellers(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditSeller", pointOfSaleId));
    }

    static public DataTable GetCreditManagers(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditManager", pointOfSaleId));
    }

    static public DataTable GetCreditExecutives(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditExecutive", pointOfSaleId));
    }

    static public DataTable GetCreditCollectors(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditCollector", pointOfSaleId));
    }

    static public DataTable GetCreditSupervisors(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditSupervisor", pointOfSaleId));
    }

    static public DataTable GetCreditAuthorizators(int pointOfSaleId) {
      return DataReader.GetDataTable(DataOperation.Parse("qryContactsInTargetRoleWithKey",
                                                         "CreditAuthorizator", pointOfSaleId));
    }

    #endregion Public methods

    #region Internal methods

    static internal int WriteFinancialAccount(FinancialAccount o) {
      DataOperation operation = DataOperation.Parse("writeFSMFinancialAccount", o.Id,
                              o.FinancialProduct.Id, o.Institution.Id, o.Customer.Id, o.SellerId,
                              o.SalesChannelId, o.AuthorizedById, o.ManagerId, o.ExecutiveId,
                              o.CollectorId, o.ContractNumber, o.ContractDate, o.InstitutionCustomerNumber,
                              o.AccountNumber, o.InterBankAccountNumber, o.Notes, o.Keywords,
                              o.BaseCurrency.Id, o.RequestedLimit, o.AuthorizedLimit,
                              o.BillingCycle, o.BillingMode, o.BillingWeekDays, o.BillingNotes,
                              o.CollectCycle, o.CollectMode, o.CollectWeekDays, o.CollectNotes,
                              o.DocumentationVector, o.Score, o.RequestedDate, o.OpeningDate,
                              o.ClosingDate, o.PostedById, o.ReplacedById, o.Status, o.StartDate, o.EndDate);


      int i = DataWriter.Execute(operation);

      //GasFacilityData.ChangeCreditAccountStatus(o.CustomerId, o.Status);

      UpdateAccountsTable(o.Id);

      return i;
    }

    //static internal int GetNextFinancialAccountId() {
    //  return DataWriter.CreateId("FSMFinancialAccounts");
    //}

    static internal int ChangeAccountStatus(FinancialAccount account, string newStatus) {
      if (newStatus == "C") {
        //GasFacilityData.ChangeCreditAccountStatus(account.CustomerId, "N");
      } else {
        //GasFacilityData.ChangeCreditAccountStatus(account.CustomerId, newStatus);
      }  // doChangeFacilityCreditStatus
      DataWriter.Execute(DataOperation.Parse("setFSMFinancialAccountStatus", account.Id, newStatus));

      SetAccountStatus(account.Id, newStatus);

      return 1;
    }

    static public DataRow GetAccount(int financialAccountId) {
      return DataReader.GetDataRow(DataOperation.Parse("qryFSMAccount", financialAccountId));
    }

    static public DataView GetAccounts(string filter, string sort) {
      DataTable accountsTable = LoadAccountsCachedTable();

      if (filter.Length != 0 && sort.Length != 0) {
        return new DataView(accountsTable, filter, sort, DataViewRowState.CurrentRows);
      } else {
        return accountsTable.DefaultView;
      }
    }

    static public DataTable GetTransactions(int accountId, DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAccountTransactions",
                                                          accountId, fromDate, toDate));
    }

    static public DataTable GetAllTransactions(int accountId, DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAccountTransactionsComplete",
                                                          accountId, fromDate, toDate));
    }

    static public DataTable LoadAccountsCachedTable() {
      return DataReader.GetDataTable(DataOperation.Parse("SELECT * FROM vwFSMAccountsBalances"));
    }

    static internal void UpdateAccountsTable(int financialAccountId) {
      DataCache.SetDataTableRow("SELECT * FROM vwFSMAccountsBalances", GetAccountRow(financialAccountId),
                                "[FinancialAccountId] = " + financialAccountId.ToString());
    }

    static public void ResetAccountsTable() {
      DataCache.Remove("SELECT * FROM vwFSMAccountsBalances");
    }

    static internal void SetAccountStatus(int accountId, string newStatus) {
      DataTable accountsTable = LoadAccountsCachedTable();

      DataRow[] dataRow = accountsTable.Select("[FinancialAccountId] = " + accountId.ToString());
      if (dataRow.Length == 1) {
        dataRow[0].BeginEdit();
        dataRow[0]["FinancialAccountStatus"] = newStatus;
        dataRow[0].EndEdit();
      }
    }

    #endregion Internal methods

    #region Private methods

    static private DataRow GetAccountRow(int financialAccountId) {
      return DataReader.GetDataRow(DataOperation.Parse("getFSMAccountWithCurrentBalance", financialAccountId));
    }

    #endregion Private methods

  } // class FinancialAccountData

} // namespace Empiria.FinancialServices.Data
