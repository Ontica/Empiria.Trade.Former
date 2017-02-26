/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Billing System                    *
*  Namespace : Empiria.Trade.Billing                            Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : BillIssuerData                                   Pattern  : Data Transfer Object              *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Holds data about a bill issuer.                                                               *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Security;

namespace Empiria.Trade.Billing {

  /// <summary>Holds data about a bill issuer.</summary>
  public class BillIssuerData {

    #region Fields
    public string BillSerialNo { get; set; }
    public string BillApprovalNo { get; set; }
    public int BillApprovalYear { get; set; }
    public string FiscalRegimen { get; set; }
    public string IssuePlace { get; set; }
    public string CertificateFileName { get; set; }
    public string CertificatePwd { get; set; }
    public string PrivateKeyFile { get; set; }

    #endregion Fields

    public SecureString GetCertificatePassword() {
      SecureString password = new SecureString();
      for (int i = 0; i < this.CertificatePwd.Length; i++) {
        password.AppendChar(this.CertificatePwd[i]);
      }

      return password;
    }

  }   //internal class BillIssuerData

}  // namespace Empiria.Trade.Billing
