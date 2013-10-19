/* Empiria® Trade 2013 ***************************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Trade                                   System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyOrder                                      Pattern  : Empiria Object Type               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a supplier-customer order in Empiria® Trade Ordering System.                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.Data.Convertion;
using Empiria.DataTypes;
using Empiria.Documents.Printing;
using Empiria.FinancialServices;
using Empiria.Treasury;

using Empiria.Products;
using Empiria.Trade.Data;
using Empiria.Trade.Billing;

namespace Empiria.Trade.Ordering {

  public enum OrderStatus {
    Opened = 'O',
    Closed = 'C',
    Canceled = 'L',
    OnDelivery = 'D',
    OnTransit = 'T',
    Deleted = 'X'
  }

  /// <summary>Represents a supplier-customer order in Empiria® Trade Ordering System.</summary>
  public class SupplyOrder : BaseObject {

    #region Fields

    private static readonly bool LegacyAppInstalled = ConfigurationData.GetBoolean("LegacyAppInstalled");

    static public string TicketPrinterName = ConfigurationData.GetString("Ticket.PrinterName");
    static public string TicketDefaultFontName = ConfigurationData.GetString("Ticket.DefaultFontName");
    static public int TicketDefaultFontSize = ConfigurationData.GetInteger("Ticket.DefaultFontSize");
    static public string ReportsTemplatesPath = ConfigurationData.GetString("Reports.TemplatesPath");

    private const string thisTypeName = "ObjectType.Trade.Order.SupplyOrder";
    private const string newOrderNumber = "Nuevo pedido";

    private string number = newOrderNumber;
    private string customerOrderNumber = String.Empty;
    private string dutyEntryTag = String.Empty;
    private string concept = String.Empty;
    private SupplyChannel supplyChannel = SupplyChannel.Empty;
    private Contact supplyPoint = Organization.Empty;
    private Contact supplier = Organization.Empty;
    private Person supplierContact = Person.Empty;
    private Contact customer = Organization.Empty;
    private Person customerContact = Person.Empty;
    private DeliveryMode deliveryMode = DeliveryMode.Empty;
    private Contact deliveryTo = Organization.Empty;
    private Contact deliveryPoint = Organization.Empty;
    private Person deliveryContact = Person.Empty;
    private DateTime deliveryTime = Empiria.ExecutionServer.DateMaxValue;
    private string deliveryNotes = String.Empty;
    private int authorizationId = -1;
    private Currency currency = Currency.Default;
    private DateTime orderingTime = DateTime.Now;
    private Person closedBy = Person.Empty;
    private DateTime closingTime = Empiria.ExecutionServer.DateMaxValue;
    private Person canceledBy = Person.Empty;
    private DateTime cancelationTime = Empiria.ExecutionServer.DateMaxValue;
    private string keywords = String.Empty;
    private CRTransaction payment = CRTransaction.Empty;
    private Bill bill = Bill.Empty;
    private int externalOrderId = -1;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private OrderStatus status = OrderStatus.Opened;

    private int parentSupplyOrderId = -1;
    private SupplyOrder parentSupplyOrder = null;
    private SupplyOrderItemList items = null;
    private FinancialAccount customerFinancialAccount = null;

    #endregion Fields

    #region Constuctors and parsers

    protected SupplyOrder()
      : base(thisTypeName) {

    }

    protected SupplyOrder(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public SupplyOrder Parse(int id) {
      return BaseObject.Parse<SupplyOrder>(thisTypeName, id);
    }

    static internal SupplyOrder Parse(DataRow dataRow) {
      return BaseObject.Parse<SupplyOrder>(thisTypeName, dataRow);
    }

    static public SupplyOrder Empty {
      get { return BaseObject.ParseEmpty<SupplyOrder>(thisTypeName); }
    }

    static public SupplyOrder CreateFromCF(int cfOrderID) {
      int supplyOrderId = SupplyOrdersData.CreateSupplyOrderFromColdFusion(cfOrderID);

      return BaseObject.ParseFromBelow<SupplyOrder>(thisTypeName, supplyOrderId);
    }

    static public SupplyOrder MyCurrentOrder() {
      return SupplyOrder.Parse(1001);
    }

    static public ObjectList<SupplyOrder> MyOrdersList(string filter) {
      return null;
    }

    internal void Reset() {
      items = null;
    }

    #endregion Constructors and parsers

    #region Public properties

    public SupplyOrderItemList Items {
      get {
        if (items == null) {
          items = SupplyOrdersData.GetSupplierOrderItems(this);
        }
        return items;
      }
    }

    public string Number {
      get { return number; }
      set { number = value; }
    }

    public string CustomerOrderNumber {
      get { return customerOrderNumber; }
      set { customerOrderNumber = value; }
    }

    public string DutyEntryTag {
      get { return dutyEntryTag; }
      set { dutyEntryTag = value; }
    }


    public string Concept {
      get { return concept; }
      set { concept = value; }
    }

    public SupplyChannel SupplyChannel {
      get { return supplyChannel; }
      set { supplyChannel = value; }
    }

    public Contact SupplyPoint {
      get { return supplyPoint; }
      set { supplyPoint = value; }
    }

    public Contact Supplier {
      get { return supplier; }
      set { supplier = value; }
    }

    public Person SupplierContact {
      get { return supplierContact; }
      set { supplierContact = value; }
    }

    public Contact Customer {
      get { return customer; }
      set { customer = value; }
    }

    public FinancialAccount CustomerFinancialAccount {
      get {
        if (customerFinancialAccount == null) {
          customerFinancialAccount = FinancialAccount.GetForCustomer(this.Customer);
        }
        return customerFinancialAccount;
      }
    }

    public string StatusName {
      get {
        switch (this.Status) {
          case OrderStatus.Opened:
            return "Abierto";
          case OrderStatus.Closed:
            return "Pedido cerrado";
          case OrderStatus.Canceled:
            return "Cancelado";
          case OrderStatus.OnDelivery:
            return "Pendiente de entrega";
          case OrderStatus.OnTransit:
            return "En tránsito";
          case OrderStatus.Deleted:
            return "Eliminado";
          default:
            return "Estado desconocido";
        }
      }
    }

    public AccountStatement CustomerCreditAccountStatement {
      get {
        return new AccountStatement(this.CustomerFinancialAccount, DateTime.Today.AddDays(-30), DateTime.Now);
      }
    }

    public Person CustomerContact {
      get { return customerContact; }
      set { customerContact = value; }
    }

    public DeliveryMode DeliveryMode {
      get { return deliveryMode; }
      set { deliveryMode = value; }
    }

    public Contact DeliveryTo {
      get { return deliveryTo; }
      set { deliveryTo = value; }
    }

    public Contact DeliveryPoint {
      get { return deliveryPoint; }
      set { deliveryPoint = value; }
    }

    public Person DeliveryContact {
      get { return deliveryContact; }
      set { deliveryContact = value; }
    }

    public DateTime DeliveryTime {
      get { return deliveryTime; }
    }

    public string DeliveryNotes {
      get { return deliveryNotes; }
      set { deliveryNotes = value; }
    }

    public int AuthorizationId {
      get { return authorizationId; }
    }

    public Currency Currency {
      get { return currency; }
    }

    public DateTime OrderingTime {
      get { return orderingTime; }
      set { orderingTime = value; }
    }

    public Person ClosedBy {
      get { return closedBy; }
    }

    public DateTime ClosingTime {
      get { return closingTime; }
    }

    public Person CanceledBy {
      get { return canceledBy; }
      set { canceledBy = value; }
    }

    public DateTime CancelationTime {
      get { return cancelationTime; }
      set { cancelationTime = value; }
    }

    public string Keywords {
      get { return keywords; }
    }

    public CRTransaction Payment {
      get { return payment; }
      set { payment = value; }
    }

    public Bill Bill {
      get { return bill; }
      set { bill = value; }
    }

    public int ExternalOrderId {
      get { return externalOrderId; }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public DateTime PostingTime {
      get { return postingTime; }
      set { postingTime = value; }
    }

    public OrderStatus Status {
      get { return status; }
    }

    public SupplyOrder Parent {
      get {
        if (parentSupplyOrder == null) {
          parentSupplyOrder = SupplyOrder.Parse(parentSupplyOrderId);
        }
        return parentSupplyOrder;
      }
      set {
        parentSupplyOrder = value;
        parentSupplyOrderId = parentSupplyOrder.Id;
      }
    }

    #endregion Public properties

    #region Public methods

    public SupplyOrderItem AddOrderItem(Product product, decimal quantity) {
      var orderItem = new SupplyOrderItem(this);
      orderItem.Product = product;
      orderItem.Quantity = quantity;
      orderItem.Save();
      this.Items.Add(orderItem);
      
      return orderItem;
    }

    public void RemoveOrderItem(SupplyOrderItem orderItem) {
      orderItem.Status = OrderStatus.Deleted;
      orderItem.Save();
    }

    public void AppendPayment(CRTransaction crTransaction) {
      this.Payment = crTransaction;
    }

    public CRTransaction CreatePayment(decimal amount, string notes) {
      CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Input.OrderPayment"), this,
                                            DateTime.Now, DateTime.Now, amount, 0m,
                                            "Pedido: " + this.Number, notes);

      //CRPosting posting = crt.CreatePosting();
      //posting.InstrumentType = instrumentType;
      //posting.InstrumentAmount = crt.Amount;
      //posting.InputAmount = crt.Amount;

      //crt.Status = TreasuryItemStatus.Closed;

      return crt;
    }

    private CRTransaction CreateCancelPayment(string notes) {
      CRTransaction crt = new CRTransaction(CRTransactionType.Parse("Output.OrderPayment"), this,
                                            DateTime.Now, DateTime.Now, 0m, this.Items.Total,
                                            "Cancelación del pedido: " + this.Number, notes);

      //CRPosting posting = crt.CreatePosting();
      //posting.InstrumentType = instrumentType;
      //posting.InstrumentAmount = crt.Amount;
      //posting.InputAmount = crt.Amount;

      //crt.Status = TreasuryItemStatus.Closed;

      return crt;
    }

    public void DoDelivery(DateTime deliveryTime) {
      this.deliveryTime = deliveryTime;
      status = OrderStatus.Closed;
      Save();
    }

    public void SetNotDelivery(DeliveryMode deliveryMode) {
      this.deliveryMode = deliveryMode;
      this.deliveryTime = ExecutionServer.DateMaxValue;
      this.deliveryContact = Person.Empty;
      status = OrderStatus.OnDelivery;
      Save();
    }

    public void Close() {
      closedBy = Person.Parse(ExecutionServer.CurrentUserId);
      closingTime = DateTime.Now;
      if (status == OrderStatus.Opened) {
        UpdateARPStock(true);
        if (this.deliveryMode.IsNoDeliveryMode) {
          this.deliveryTime = DateTime.Now;
          status = OrderStatus.Closed;
        } else {
          status = OrderStatus.OnDelivery;
        }
      }
      Save();
      if (!this.Payment.IsEmptyInstance) {
        this.ClosePayment();
        Save();
      }
    }

    private void ClosePayment() {
      string summary = String.Empty;

      if (this.Payment.IsEmptyInstance) {
        throw new NotImplementedException("Payment cannot be empty instance.");
      }

      using (DataWriterContext context = DataWriter.CreateContext("OnCreateFSMTransaction")) {
        ITransaction transaction = context.BeginTransaction();
        List<CRPosting> postings = this.Payment.Postings.FindAll((x) => (x.InstrumentId > 0));
        for (int i = 0; i < postings.Count; i++) {
          FinancialAccount account = FinancialAccount.Parse(postings[i].InstrumentId);
          account.CreateConcept(FinancialConcept.Parse("FSB002"), postings[i].Transaction.CashRegister, DateTime.Now,
                                postings[i].InputAmount, "Pedido: " + this.Number, String.Empty, this.Id);
        }
        this.Payment.Close();
        //OOJJOO
        context.Update();
        transaction.Commit();
      }
    }

    public void Cancel(InstrumentType instrumentType, string notes) {
      if (this.Payment.IsEmptyInstance) {
        throw new NotImplementedException("Cannot cancel this supply order with this method because the payment is empty. Use Cancel() method instead.");
      }
      UpdateARPStock(false);
      cancelationTime = DateTime.Now;
      canceledBy = Person.Parse(ExecutionServer.CurrentUserId);
      status = OrderStatus.Canceled;
      Save();
      CRTransaction tr = this.CreateCancelPayment(notes);

      List<CRPosting> postings = this.Payment.Postings.FindAll((x) => (x.InstrumentId > 0));
      decimal canceledAmount = decimal.Zero;
      for (int i = 0; i < postings.Count; i++) {
        FinancialAccount account = FinancialAccount.Parse(postings[i].InstrumentId);
        account.CreateConcept(FinancialConcept.Parse("FSD005"), tr.CashRegister, DateTime.Now,
                              postings[i].InputAmount, "Pedido: " + this.Number, String.Empty, this.Id);
        tr.AppendPosting(postings[i].InstrumentType, account, postings[i].InputAmount);
        canceledAmount += postings[i].InputAmount;
      }
      if (items.Total != canceledAmount) {
        tr.AppendPosting(instrumentType, CRDocument.Empty, items.Total - canceledAmount);
      }
      tr.Close();
    }

    public void Cancel() {
      if (!this.Payment.IsEmptyInstance) {
        throw new NotImplementedException("Cannot cancel this supply order with this method because the payment is not empty. " + 
                                          "Use Cancel(InstrumentType, string) method instead.");
      }
      UpdateARPStock(false);
      cancelationTime = DateTime.Now;
      canceledBy = Person.Parse(ExecutionServer.CurrentUserId);
      status = OrderStatus.Canceled;
      Save();
    }

    private void UpdateARPStock(bool bToClose) {
      if (!LegacyAppInstalled) {
        return;
      }

      DataConvertionEngine engine = DataConvertionEngine.GetInstance();
      engine.Initalize("Empiria", "Autopartes.MySQL");

      DataView view = SupplyOrdersData.GetSupplyOrdersItemsARPCrossed(this);
      if (view.Count == 0) {
        return;
      }
      string[] sqlArray = new string[view.Count];
      for (int i = 0; i < view.Count; i++) {
        int cantidad = Convert.ToInt32((decimal) view[i]["Quantity"]);
        string temp = String.Empty;
        if (bToClose) {
          temp = "UPDATE Articulos SET Existencia = (Existencia - " + cantidad.ToString() + ") " +
                 "WHERE (cveArticulo = '" + (string) view[i]["cveArticulo"] + "') AND " +
                 "(cveMarcaArticulo = " + ((int) view[i]["cveMarcaArticulo"]).ToString() + ")";
        } else {
          temp = "UPDATE Articulos SET Existencia = (Existencia + " + cantidad.ToString() + ") " +
                 "WHERE (cveArticulo = '" + (string) view[i]["cveArticulo"] + "') AND " +
                 "(cveMarcaArticulo = " + ((int) view[i]["cveMarcaArticulo"]).ToString() + ")";
        }
        sqlArray[i] = temp;
      }
      engine.Execute(sqlArray);
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.number = (string) row["SupplierOrderNumber"];
      this.customerOrderNumber = (string) row["CustomerOrderNumber"];
      this.dutyEntryTag = (string) row["OrderDutyEntryTag"];
      this.concept = (string) row["SupplyOrderConcept"];
      this.supplyChannel = SupplyChannel.Parse((int) row["SupplyChannelId"]);
      this.supplyPoint = Contact.Parse((int) row["SupplyPointId"]);
      this.supplier = Contact.Parse((int) row["SupplierId"]);
      this.supplierContact = Person.Parse((int) row["SupplierContactId"]);

      this.customerContact = Person.Parse((int) row["CustomerContactId"]);
      this.deliveryMode = DeliveryMode.Parse((int) row["DeliveryModeId"]);
      this.deliveryTo = Contact.Parse((int) row["DeliveryToId"]);
      this.deliveryPoint = Contact.Parse((int) row["DeliveryPointId"]);
      this.deliveryContact = Person.Parse((int) row["DeliveryContactId"]);

      this.authorizationId = (int) row["OrderAuthorizationId"];
      this.currency = Currency.Parse((int) row["SupplyOrderCurrencyId"]);
      this.orderingTime = (DateTime) row["OrderingTime"];
      this.closedBy = Person.Parse((int) row["ClosedById"]);
      this.closingTime = (DateTime) row["ClosingTime"];
      this.canceledBy = Person.Parse((int) row["CanceledById"]);
      this.cancelationTime = (DateTime) row["CancelationTime"];
      this.keywords = (string) row["SupplyOrderKeywords"];
      this.payment = CRTransaction.Parse((int) row["CRTransactionId"]);
      this.bill = Bill.Parse((int) row["BillId"]);
      this.externalOrderId = (int) row["ExternalOrderId"];
      this.parentSupplyOrderId = (int) row["ParentSupplyOrderId"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.postingTime = (DateTime) row["PostingTime"];

      this.status = (OrderStatus) Convert.ToChar(row["SupplyOrderStatus"]);

      if (this.Status == OrderStatus.Opened) {
        this.customer = Contact.ParseFromBelow((int) row["CustomerId"]);
      } else {
        this.customer = Contact.Parse((int) row["CustomerId"]);
      }
    }

    protected override void ImplementsSave() {
      this.keywords = EmpiriaString.BuildKeywords(this.number, this.concept, this.customer.Keywords);
      if (IsNew) {
        postedBy = Contact.Parse(ExecutionServer.CurrentUserId);
        postingTime = DateTime.Now;
      }
      SupplyOrdersData.WriteSupplyOrder(this);

      this.Reset();
    }

    #endregion Public methods

    #region Credit Voucher

    private void FillCreditVoucherDocument(Empiria.Documents.Printing.Document document) {
      string temp = this.Customer.FullName + " (" + this.Customer.Nickname.ToString() + ")";
      string[] vector = EmpiriaString.DivideLongString(temp, 29, 2);
      document.Replace("<@CUSTOMER_NAME_1@>", vector[0]);
      document.Replace("<@CUSTOMER_NAME_2@>", vector.Length > 1 ? vector[1] : String.Empty);
      document.Replace("<@CUSTOMER_NAME_3@>", vector.Length > 2 ? vector[2] : String.Empty);
      document.Replace("<@CUSTOMER_NAME_4@>", vector.Length > 3 ? vector[3] : String.Empty);
      document.Replace("<@CUSTOMER_NAME_5@>", vector.Length > 4 ? vector[4] : String.Empty);
      document.Replace("<@CUSTOMER_NUMBER@>", this.Customer.Nickname.ToString());
      document.Replace("<@CUSTOMER_RFC@>", this.Customer.FormattedTaxTag);
      document.Replace("<@SELLER_NAME@>", this.SupplierContact.FullName);
      document.Replace("<@ORDER_NUMBER@>", this.ExternalOrderId.ToString("00000000"));
      document.Replace("<@ACCOUNT@>", this.Customer.Nickname);
      document.Replace("<@DATE@>", this.ClosingTime.ToString("dd/MMM/yyyy"));
      document.Replace("<@TOTAL@>", this.Items.Total.ToString("C2"));
      temp = EmpiriaString.SpeechMoney(this.Items.Total);
      vector = EmpiriaString.DivideLongString(temp, 40, 2);
      document.Replace("<@TOTAL_STRING_1@>", vector[0]);
      document.Replace("<@TOTAL_STRING_2@>", vector.Length > 1 ? vector[1] : String.Empty);
      document.Replace("<@TOTAL_STRING_3@>", vector.Length > 2 ? vector[2] : String.Empty);
      document.Replace("<@TOTAL_STRING_4@>", vector.Length > 3 ? vector[3] : String.Empty);
    }

    public void PrintCreditVoucher() {
      Empiria.Documents.Printing.Document document = new 
                    Empiria.Documents.Printing.Document(TicketDefaultFontName, TicketDefaultFontSize);

      document.LoadTemplate(ReportsTemplatesPath + "credit.voucher.ert");

      FillCreditVoucherDocument(document);

      Ticket ticket = new Ticket();
      ticket.Load(document);
      ticket.Print(TicketPrinterName);

      //Customer voucher copy
      document.LoadTemplate(ReportsTemplatesPath + "credit.voucher.customer.ert");

      FillCreditVoucherDocument(document);

      ticket = new Ticket();
      ticket.Load(document);
      ticket.Print(TicketPrinterName);
    }


    #endregion Credit Voucher

  } // class SupplyOrder

} // namespace Empiria.Trade.Ordering