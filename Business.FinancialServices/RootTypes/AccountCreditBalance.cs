/* Empiria® Business Framework 2014 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : AccountCreditBalance                             Pattern  : Business Services Class           *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Performs credit by credit balance calculation.                                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;
using System.Data;

using Empiria.Data;

using Empiria.FinancialServices.Data;

namespace Empiria.FinancialServices {

  internal class AccountCreditBalance {

    #region Fields

    private int accountId = 0;
    private DataTable credits = null;
    private DataTable debits = null;

    private int debitsBookmark = -1;
    private int creditsBookmark = -1;

    private DataOperationList operations = new DataOperationList("UpdateFSMAccountCreditBalance");

    #endregion Fields

    #region Constructors and Parsers

    internal AccountCreditBalance(int accountId) {
      this.accountId = accountId;
    }

    #endregion Constructors and Parsers

    #region Internal methods

    internal bool NeedsRebuild() {
      DataRow row = AccountBalancesData.GetAccountCurrentBalance(this.accountId);
      if (row == null) {
        AccountBalancesData.DeleteAccountPaymentDistribution(this.accountId);
        return false;
      }
      if ((decimal) row["balance"] <= ((decimal) row["creditBalanceBalance"] - 2m) ||
          (decimal) row["balance"] >= ((decimal) row["creditBalanceBalance"] + 2m)) {
        return true;
      } else {
        return false;
      }
    }

    internal void Rebuild() {
      LoadData();
      AccountBalancesData.DeleteAccountPaymentDistribution(accountId);
      SetCreditsBalances();
    }

    #endregion Internal methods

    #region Private methods

    private void SetCreditsBalances() {
      decimal creditAcumulator = ReadNextCredit();
      decimal debitAcumulator = ReadNextDebit();

      while (true) {
        if (CreditsEOF() && DebitsEOF()) {
          break;
        } else if (!CreditsEOF() && DebitsEOF()) {
          if ((creditAcumulator - debitAcumulator) > 0) {
            SetCreditBalance(creditAcumulator - debitAcumulator);
          }
          break;
        } else if (CreditsEOF() && !DebitsEOF()) {
          AppendPaymentDistribution(debitAcumulator);
          debitAcumulator = ReadNextDebit();
        } else if (!CreditsEOF() && !DebitsEOF()) {
          if (creditAcumulator == debitAcumulator) {
            SetCreditBalance(0m);
            AppendPaymentDistribution(debitAcumulator);
            creditAcumulator = ReadNextCredit();
            debitAcumulator = ReadNextDebit();
          } else if (creditAcumulator < debitAcumulator) {
            SetCreditBalance(0m);
            AppendPaymentDistribution(creditAcumulator);
            debitAcumulator = debitAcumulator - creditAcumulator;
            creditAcumulator = ReadNextCredit();
          } else if (creditAcumulator > debitAcumulator) {
            AppendPaymentDistribution(debitAcumulator);
            creditAcumulator = creditAcumulator - debitAcumulator;
            debitAcumulator = ReadNextDebit();
          } // if
        } // if
      } // while

      SaveOperations();
    }

    private void SetCreditBalance(decimal creditBalance) {
      int transactionId = (int) credits.Rows[creditsBookmark]["TransactionId"];

      operations.Add(AccountBalancesData.SetCreditBalance(transactionId, creditBalance));
    }

    private void AppendPaymentDistribution(decimal paymentAmount) {
      int creditTransactionId = 0;
      int debitTransactionId = 0;
      int organizationId = 0;
      DateTime date = DateTime.Now;

      if (!CreditsEOF()) {
        creditTransactionId = (int) credits.Rows[creditsBookmark]["TransactionId"];
        organizationId = (int) credits.Rows[creditsBookmark]["OrganizationId"];
      } else {
        creditTransactionId = 0;
        organizationId = -1;
      }
      if (!DebitsEOF()) {
        debitTransactionId = (int) debits.Rows[debitsBookmark]["TransactionId"];
        date = (DateTime) debits.Rows[debitsBookmark]["TransactionDate"];
      } else {
        throw new FinancialServicesException(FinancialServicesException.Msg.PaymentDistributionError);
      }
      operations.Add(AccountBalancesData.AppendPaymentDistribution(creditTransactionId, debitTransactionId, accountId,
                                                                   organizationId, date, paymentAmount));
    }

    #endregion Private methods

    #region Private utility methods

    private bool CreditsEOF() {
      return ((creditsBookmark >= credits.Rows.Count) || (credits.Rows.Count == 0));
    }

    private bool DebitsEOF() {
      return ((debitsBookmark >= debits.Rows.Count) || (debits.Rows.Count == 0));
    }

    private void LoadData() {
      credits = AccountBalancesData.GetAccountCredits(accountId);
      debits = AccountBalancesData.GetAccountDebits(accountId);
    }

    private decimal ReadNextCredit() {
      if (CreditsEOF()) {
        return 0m;
      }
      creditsBookmark++;
      if (!CreditsEOF()) {
        return (decimal) credits.Rows[creditsBookmark]["Credit"];
      } else {
        return 0m;
      }
    }

    private decimal ReadNextDebit() {
      if (DebitsEOF()) {
        return 0m;
      }
      debitsBookmark++;
      if (!DebitsEOF()) {
        return (decimal) debits.Rows[debitsBookmark]["Debit"];
      } else {
        return 0m;
      }
    }

    private void SaveOperations() {
      if (operations.Count == 0) {
        return;
      }
      using (DataWriterContext context = DataWriter.CreateContext("SaveAccountCreditBalance")) {
        ITransaction transaction = context.BeginTransaction();
        context.Add(operations);
        context.Update();
        transaction.Commit();
      }
    }

    #endregion Private utility methods

  } // class AccountCreditBalance

} // namespace Empiria.FinancialServices