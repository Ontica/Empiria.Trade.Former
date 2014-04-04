using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Empiria.Trade.Billing {

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
