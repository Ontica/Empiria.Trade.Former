/* Empiria Customers Components 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : Recording                                        Pattern  : Empiria Object Type               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a bill.                                                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

using Empiria.Contacts;

using Empiria.Trade.Data;
using Empiria.Trade.Ordering;

namespace Empiria.Trade.Billing {

  public enum BillStatus {
    Active = 'A',
    Canceled = 'C',
    Deleted = 'X',
  }

  public enum BillType {
    Bill = 'B',
    CreditNote = 'C',
    DebitNote = 'D',
    Transfer = 'T',
    GlobalBill = 'G',
    GlobalCreditNote = 'L',
  }

  internal class BillNoOrderData {
    internal decimal SubTotal = 0m;
    internal decimal Taxes = 0m;
    internal decimal Total = 0m;
    internal string TicketNumbers = String.Empty;
    internal string PaymentCondition = "No identificado";

    internal static BillNoOrderData Parse(DataView view) {
      BillNoOrderData o = new BillNoOrderData();
      for (int i = 0; i < view.Count; i++) {
        o.Total += (decimal) view[i]["SupplyOrderTotal"];
        o.Taxes += (decimal) view[i]["SupplyOrderTaxes"];
        o.TicketNumbers += ((int) view[i]["ExternalOrderId"]).ToString() + ", ";
      }
      o.SubTotal = o.Total - o.Taxes;
      o.TicketNumbers = o.TicketNumbers.Trim(' ');
      o.TicketNumbers = o.TicketNumbers.Trim(',');
      return o;
    }
  }

  /// <summary>Represents a bill.</summary>
  public class Bill : BaseObject {

    #region Fields

    internal static readonly string BillCertificatesFolder = ConfigurationData.GetString("BillCertificatesFolder");
    public static readonly string BillXmlFilesFolder = ConfigurationData.GetString("BillXmlFilesFolder");
    public static readonly string BillHtmlFilesFolder = ConfigurationData.GetString("BillHtmlFilesFolder");
    public static readonly string BillPDFFilesFolder = ConfigurationData.GetString("BillPDFFilesFolder");
    public static readonly string BillUrlHtmlFilesFolder = ConfigurationData.GetString("BillUrlHtmlFilesFolder");
    public static readonly bool SendBillOnlyToDefaultEmail = ConfigurationData.GetBoolean("SendBillOnlyToDefaultEmail");

    private const string thisTypeName = "ObjectType.Bill";

    private BillType billType = BillType.Bill;
    private int supplyOrderId = -1;
    private SupplyOrder supplyOrder = null;
    private Contact issuedBy = Contact.Parse(ExecutionServer.CurrentUserId);
    private DateTime issuedTime = DateTime.Now;
    private string certificateContent = String.Empty;
    private string certificateNumber = String.Empty;
    private string serialNumber = String.Empty;
    private string number = String.Empty;
    private int approvalYear = 0;
    private string approvalNumber = String.Empty;
    private string digitalString = String.Empty;
    private string digitalSign = String.Empty;
    private Contact canceledBy = Person.Empty;
    private DateTime cancelationTime = ExecutionServer.DateMaxValue;
    private BillStatus status = BillStatus.Active;

    private string paymentAccount = null;
    private string paymentCondition = null;

    private XmlDocument xml = null;
    private BillStamp stamp = null;
    private bool hasStamp = false;

    private BillNoOrderData notOrderData = null;

    #endregion Fields

    #region Constructors and parsers

    protected Bill()
      : base(thisTypeName) {
    }

    protected Bill(string typeName)
      : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    static public Bill Parse(SupplyOrder order) {
      Assertion.AssertObject(order, "order");

      if (order.Bill.IsEmptyInstance) {
        var bill = new Bill();
        bill.supplyOrder = order;
        bill.Create();
        order.Bill = bill;
        order.Save();
      }
      return order.Bill;
    }

    static public Bill Parse(int id) {
      return BaseObject.Parse<Bill>(thisTypeName, id);
    }

    static internal Bill Parse(DataRow dataRow) {
      return BaseObject.Parse<Bill>(thisTypeName, dataRow);
    }

    static public Bill Empty {
      get { return BaseObject.ParseEmpty<Bill>(thisTypeName); }
    }

    static private string CleanDigitalString(string digitalString) {
      string temp = digitalString;

      for (int c = 0; c < temp.Length; c++) {
        if (Char.IsControl(temp[c])) {
          temp = temp.Replace(temp[c].ToString(), String.Empty);
        }
        if (!(temp[c] >= 0x20 && temp[c] <= 127) && !char.IsLetterOrDigit(temp[c])) {
          temp = temp.Replace(temp[c].ToString(), String.Empty);
        }
      }

      return EmpiriaString.TrimAll(temp);
    }

    static private string GetDigitalStringItem(string digitalStringItem) {
      string temp = CleanBillItem(digitalStringItem);

      if (!String.IsNullOrWhiteSpace(temp)) {
        return temp + "|";
      } else {
        return String.Empty;
      }
    }

    static private string CleanBillItem(string digitalStringItem) {
      string temp = digitalStringItem;
      temp = temp.Replace("|", String.Empty);
      temp = EmpiriaString.TrimAll(temp);

      return CleanDigitalString(temp);
    }

    static internal string CleanXmlBillItem(string digitalStringItem) {
      string temp = digitalStringItem;

      temp = CleanBillItem(temp);
      temp = temp.Replace("&", "&amp;");
      temp = temp.Replace("\"", " &quot");
      temp = temp.Replace("<", "&lt;");
      temp = temp.Replace(">", "&gt;");
      temp = temp.Replace("'", "&apos;");

      return CleanDigitalString(temp);
    }

    static public void GenerateDailyBill(Contact supplier, DateTime fromDate, DateTime toDate) {
      if (fromDate.Month != toDate.Month || fromDate.Year != toDate.Year) {
        throw new TradeOrderingException(TradeOrderingException.Msg.InvalidPeriodForDailyBills, fromDate, toDate);
      }
      DateTime dailyBillDate = toDate.AddDays(3) > DateTime.Now ? DateTime.Now : toDate;   // 72 hrs to send to stamp;

      DataView view = BillingData.GetBills(fromDate, toDate, "[BillType] IN ('G', 'L')");
      if (view.Count != 0) {
        Empiria.Messaging.Publisher.Publish("Global bills were already generated for the selected period.");
        return;
      }

      Bill bill = null;

      string filter = "( (BillId = -1 AND SupplyOrderStatus <> 'O') AND ClosingTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
                      "ClosingTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND " +
                      "CancelationTime > '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ) OR " +
                      "( (BillId <> -1 AND SupplyOrderStatus <> 'O') AND ClosingTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
                      "ClosingTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND " +
                      "BillIssuedTime > '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' )";

      view = SupplyOrdersData.GetSupplierOrders(supplier, filter, "ExternalOrderId ASC");

      if (view.Count != 0) {
        bill = new Bill();
        bill.BillType = BillType.GlobalBill;
        bill.issuedBy = supplier;
        bill.supplyOrder = SupplyOrder.Empty;
        bill.issuedTime = dailyBillDate;   // 72 hrs to send to stamp
        bill.NotOrderData = BillNoOrderData.Parse(view);
        bill.Create();
      }
      filter = "(BillId = -1) AND ClosingTime < '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
               "CancelationTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
               "CancelationTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND SupplyOrderStatus IN ('L', 'X')";

      view = SupplyOrdersData.GetSupplierOrders(supplier, filter, "ExternalOrderId ASC");

      if (view.Count != 0) {
        bill = new Bill();
        bill.BillType = BillType.GlobalCreditNote;
        bill.issuedBy = supplier;
        bill.supplyOrder = SupplyOrder.Empty;
        bill.issuedTime = dailyBillDate;   // 72 hrs to send to stamp
        bill.NotOrderData = BillNoOrderData.Parse(view);
        bill.Create();
      }

      filter = "(BillId <> -1) AND ClosingTime < '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
               "BillIssuedTime >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
               "BillIssuedTime <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND SupplyOrderStatus IN ('D','C')";

      view = SupplyOrdersData.GetSupplierOrders(supplier, filter, "ExternalOrderId ASC");

      if (view.Count != 0) {
        bill = new Bill();
        bill.BillType = BillType.GlobalCreditNote;
        bill.issuedBy = supplier;
        bill.supplyOrder = SupplyOrder.Empty;
        bill.issuedTime = dailyBillDate;   // 72 hrs to send to stamp
        bill.NotOrderData = BillNoOrderData.Parse(view);
        bill.Create();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public BillType BillType {
      get { return billType; }
      set { billType = value; }
    }

    public string BillTypeFiscalName {
      get {
        switch (billType) {
          case BillType.Bill:
          case BillType.GlobalBill:
          case BillType.DebitNote:
            return "I";
          case BillType.CreditNote:
          case BillType.GlobalCreditNote:
            return "E";
          case BillType.Transfer:
            return "T";
          default:
            throw new TradeOrderingException(TradeOrderingException.Msg.UnrecognizedBillType, billType);
        }
      }
    }

    internal void Cancel() {
      this.cancelationTime = DateTime.Now;
      this.canceledBy = Contact.Parse(ExecutionServer.CurrentUserId);
      this.Status = BillStatus.Canceled;
      if (this.HasStamp) {
        BillStamper.CancelStamp(this);
      }
      this.Save();
    }

    public string FullNumber {
      get {
        if (this.HasStamp) {
          return this.Stamp.UUID;
        } else {
          return this.FullOldNumber;
        }
      }
    }

    public SupplyOrder Order {
      get {
        if (supplyOrder == null) {
          supplyOrder = SupplyOrder.Parse(supplyOrderId);
        }
        return supplyOrder;
      }
    }

    internal BillNoOrderData NotOrderData {
      get { return notOrderData; }
      set { notOrderData = value; }
    }

    public string PaymentAccount {
      get {
        if (this.paymentAccount == null) {
          if (this.notOrderData != null) {

            this.paymentAccount = String.Empty;

            return this.paymentAccount;
          }

          List<Treasury.CRPosting> postings =
                    this.Order.Payment.Postings.FindAll((x) => x.InputAmount > 0m && !x.Document.IsEmptyInstance);

          string s = String.Empty;
          for (int i = 0; i < postings.Count; i++) {
            if (i != 0) {
              s += ",";
            }
            string account = "0000" + postings[i].Document.AccountNumber;
            s += account.Substring(account.Length - 4);
          }
          this.paymentAccount = s;
        }
        return this.paymentAccount;
      }
    }

    public string PaymentCondition {
      get {
        if (this.paymentCondition == null) {
          if (this.notOrderData != null) {
            this.paymentCondition = "99";

            return this.paymentCondition;
          }

          List<Treasury.CRPosting> postings = this.Order.Payment.Postings.FindAll((x) => x.InputAmount > 0m);

          if (postings.Count != 0) {
            postings.Sort((x, y) => x.InputAmount.CompareTo(y.InputAmount));

            this.paymentCondition = postings[postings.Count - 1].InstrumentType.TaxFormCode;
          } else {
            this.paymentCondition = "99";
          }
        }
        return this.paymentCondition;
      }
    }

    public string PaymentMethod {
      get {
        if (this.PaymentCondition == "99") {
          return "PPD";
        } else {
          return "PUE";
        }
      }
    }

    public Contact IssuedBy {
      get { return issuedBy; }
      set { issuedBy = value; }
    }

    public DateTime IssuedTime {
      get { return issuedTime; }
      set { issuedTime = value; }
    }

    public BillIssuerData IssuerData {
      get {
        if (this.NotOrderData == null) {
          return this.Order.Supplier.GetExtensionData<BillIssuerData>();
        } else {
          return this.IssuedBy.GetExtensionData<BillIssuerData>();
        }
      }
    }

    public Contact CanceledBy {
      get { return canceledBy; }
      set { canceledBy = value; }
    }

    public DateTime CancelationTime {
      get { return cancelationTime; }
      set { cancelationTime = value; }
    }

    public string CertificateContent {
      get { return certificateContent; }
      set { certificateContent = value; }
    }

    public string CertificateNumber {
      get { return certificateNumber; }
      set { certificateNumber = value; }
    }

    public string SerialNumber {
      get { return serialNumber; }
      set { serialNumber = value; }
    }

    public string Number {
      get { return number; }
      set { number = value; }
    }

    public int ApprovalYear {
      get { return approvalYear; }
      set { approvalYear = value; }
    }

    public string ApprovalNumber {
      get { return approvalNumber; }
      set { approvalNumber = value; }
    }

    public string DigitalString {
      get {
        return String.Empty;
      }
    }

    public string DigitalSign {
      get {
        return String.Empty;
      }
    }

    public decimal SubTotal {
      get {
        if (this.notOrderData != null) {
          return this.NotOrderData.SubTotal;
        } else {
          return this.Order.Items.ProductSubTotalBeforeTaxes;
        }
      }
    }

    public decimal Total {
      get {
        if (this.notOrderData != null) {
          return this.NotOrderData.Total;
        } else {
          return this.Order.Items.Total;
        }
      }
    }

    public BillStatus Status {
      get { return status; }
      set { status = value; }
    }

    public string Version {
      get { return "3.3"; }
    }

    public XmlDocument XmlDocument {
      get { return xml; }
    }

    #endregion Public properties

    #region Public methods

    public string GetXmlFileName() {
      string fileName = "P" + this.Order.ExternalOrderId.ToString("0000000");
      fileName += "." + this.FullNumber;
      fileName += "." + this.IssuedTime.ToString("yyyyMMddHHmmss");

      return fileName + ".xml";
    }

    private string GetXmlFileOldName() {
      string fileName = "P" + this.Order.ExternalOrderId.ToString("0000000");
      fileName += "." + this.FullOldNumber;
      fileName += "." + this.IssuedTime.ToString("yyyyMMddHHmmss");

      return fileName + ".xml";
    }

    private string FullOldNumber {
      get {
        return this.SerialNumber + int.Parse(this.Number).ToString("000000");
      }
    }

    public string GetXmlFileNameFull() {
      string fileName = Bill.BillXmlFilesFolder + GetXmlFileOldName();

      if (File.Exists(fileName)) {
        return fileName;    // Always retrive XML files with old naming convention when them exist
      } else {
        return Bill.BillXmlFilesFolder + GetXmlFileName();
      }
    }

    public string GetXmlString() {
      if (xml == null) {
        return String.Empty;
      }
      StringWriter sw = new StringWriter();
      XmlTextWriter xw = new XmlTextWriter(sw);
      xml.WriteTo(xw);

      return sw.ToString();
    }

    public void SendToCustomer(System.IO.FileInfo pdfFile) {
      System.IO.FileInfo xmlfile = new System.IO.FileInfo(GetXmlFileNameFull());
      System.IO.FileInfo[] files = new System.IO.FileInfo[2];
      files[0] = pdfFile;
      files[1] = xmlfile;

      string eMail = this.Order.Customer.EMail;
      if (!SendBillOnlyToDefaultEmail) {
        Empiria.Messaging.EMail.Send(eMail, GetMailSubject(), GetMailBody(), files);
      } else {
        Empiria.Messaging.EMail.Send("jmcota@ontica.org", GetMailSubject(), GetMailBody(), files);
      }
    }

    public void SendToCustomer(string eMail, System.IO.FileInfo pdfFile) {
      System.IO.FileInfo xmlfile = new System.IO.FileInfo(GetXmlFileNameFull());
      System.IO.FileInfo[] files = new System.IO.FileInfo[2];
      files[0] = pdfFile;
      files[1] = xmlfile;

      if (!SendBillOnlyToDefaultEmail) {
        Empiria.Messaging.EMail.Send(eMail, GetMailSubject(), GetMailBody(), files);
      } else {
        Empiria.Messaging.EMail.Send("jmcota@ontica.org", GetMailSubject(), GetMailBody(), files);
      }
    }

    public bool HasStamp {
      get { return hasStamp && stamp != null; }
    }

    public BillStamp Stamp {
      get { return stamp; }
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.billType = (BillType) Convert.ToChar(row["BillType"]);
      this.supplyOrderId = (int) row["OrderId"];
      this.issuedBy = Contact.Parse((int) row["IssuedById"]);
      this.issuedTime = (DateTime) row["IssuedTime"];
      this.certificateNumber = (string) row["CertificateNumber"];
      this.serialNumber = (string) row["BillSerialNumber"];
      this.number = (string) row["BillNumber"];
      this.approvalYear = (int) row["ApprovalYear"];
      this.approvalNumber = (string) row["ApprovalNumber"];
      this.digitalString = (string) row["DigitalString"];
      this.digitalSign = (string) row["DigitalSign"];
      if (((string) row["BillXMLVersion"]).Length != 0) {
        xml = new System.Xml.XmlDocument();
        xml.LoadXml((string) row["BillXMLVersion"]);
      }
      if (((string) row["BillStamp"]).Length != 0) {
        this.stamp = BillStamp.Parse((string) row["BillStamp"]);
        this.hasStamp = true;
      }
      this.canceledBy = Contact.Parse((int) row["CanceledById"]);
      this.cancelationTime = (DateTime) row["CancelationTime"];
      this.status = (BillStatus) Convert.ToChar(row["BillStatus"]);
    }

    protected override void ImplementsSave() {
      if (String.IsNullOrWhiteSpace(this.number)) {
        this.number = this.Id.ToString();
        this.CreateXMLFile();
      }
      BillingData.WriteBill(this);
    }

    #endregion Public methods

    #region Private methods

    private void Create() {
      Assertion.AssertObject(this.Order, "Empiria.Trade.Billing.Bill.Order");

      this.certificateContent = GetCertificateContent();
      this.certificateNumber = GetCertificateSerialNumber();
      this.serialNumber = this.IssuerData.BillSerialNo;
      this.approvalYear = this.IssuerData.BillApprovalYear;
      this.approvalNumber = this.IssuerData.BillApprovalNo; // "859413" / "409777";
      this.status = BillStatus.Active;
      this.Save();
    }

    private string GetCertificateContent() {
      string certificatePath = Bill.BillCertificatesFolder + this.IssuerData.CertificateFileName;

      X509Certificate2 certificate = new X509Certificate2(certificatePath, this.IssuerData.GetCertificatePassword());
      byte[] array = certificate.GetRawCertData();

      return Convert.ToBase64String(array);
    }

    private string GetCertificateSerialNumber() {
      string certificatePath = Bill.BillCertificatesFolder + this.IssuerData.CertificateFileName;

      X509Certificate2 certificate = new X509Certificate2(certificatePath, this.IssuerData.GetCertificatePassword());
      byte[] array = certificate.GetSerialNumber();

      Array.Reverse(array);

      return ASCIIEncoding.ASCII.GetString(array);
    }

    private void CreateXMLFile() {
      XmlBill facturaXml = XmlBill.Parse(this);
      this.xml = facturaXml.CreateDocument();
      this.xml.Save(this.GetXmlFileNameFull());

      if (this.IssuedTime.Year >= 2014) {
        this.stamp = BillStamper.Stamp(this);
        this.hasStamp = true;
        this.xml = this.stamp.GetXmlDocument();
      }
      this.xml.Save(this.GetXmlFileNameFull());
    }

    private string GetMailBody() {
      string body = String.Empty;

      body = "Estimado cliente:" + System.Environment.NewLine + System.Environment.NewLine;

      body += "Nos permitimos enviarles los archivos PDF y XML relativos a la factura electrónica que detallamos a continuación:" +
              System.Environment.NewLine + System.Environment.NewLine;

      body += "Razón social: " + this.Order.Customer.FullName + System.Environment.NewLine;
      body += "RFC: " + this.Order.Customer.FormattedTaxTag + System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Pedido: " + this.Order.ExternalOrderId + System.Environment.NewLine;
      body += "Importe: " + this.Order.Items.Total.ToString("C2") + System.Environment.NewLine;
      body += "Forma de pago: " + this.PaymentCondition + System.Environment.NewLine;
      body += "Forma de entrega: " + this.Order.DeliveryMode.Name + System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Factura electrónica: " + this.FullNumber + System.Environment.NewLine;
      body += "Fecha y hora de expedición: " + this.IssuedTime.ToString("dd/MMM/yyyy HH:mm:ss") + System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Lo atendió: " + ((Contact) this.Order.SupplierContact).FullName + System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Le agradecemos su preferencia y quedamos a sus órdenes para cualquier asunto que requieran.";
      body += System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Atentamente,";
      body += System.Environment.NewLine;
      body += System.Environment.NewLine;
      body += "Auto Refacciones Pineda, S.A. de C.V." + System.Environment.NewLine;
      body += "pineda@masautopartes.com.mx" + System.Environment.NewLine;
      body += "(55) 2973-0249 / 1997-9360" + System.Environment.NewLine + System.Environment.NewLine;
      body += "A partir de 2014, el número de pedido nos servirá como mecanismo para identificar las ventas a crédito " +
              "y de contado, ya no el número serie y factura anterior los cuales han sido descontinuados por el SAT.";
      body += System.Environment.NewLine + System.Environment.NewLine;
      body += "Le sugerimos no responder o reenviar este correo a la cuenta facturas.pineda@masautopartes.com.mx ya que no " +
              "tiene un administrador y sólo la utilizamos para enviar sus facturas. Gracias.";

      return body;
    }

    private string GetMailSubject() {
      return "Enviamos la factura electrónica del pedido " + this.Order.ExternalOrderId.ToString("N0");
    }

    #endregion Private methods

  } // class Bill

} // namespace Empiria.Trade.Billing
