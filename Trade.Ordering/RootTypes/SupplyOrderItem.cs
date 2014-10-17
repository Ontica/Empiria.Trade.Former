/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyOrderItem                                  Pattern  : Empiria Object Type               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a supply product order item in the Supply Management System.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Products;
using Empiria.Trade.Data;

namespace Empiria.Trade.Ordering {

  public enum PriceType {
    A = 'A',
    B = 'B',
    C = 'C',
    D = 'D',
    E = 'E',
    F = 'F',
    G = 'G',
    H = 'H',
  }

  /// <summary>Represents a supply product order item in the Supply Management System.</summary>
  public class SupplyOrderItem : BaseObject {

    #region Fields

    private SupplyOrder order = SupplyOrder.Empty;
    private int orderItemTypeId = 2040;
    private Contact supplyPoint = Organization.Empty;
    private string concept = String.Empty;
    private int applicationItemTypeId = -1;
    private int applicationItemId = -1;
    private Person commissioner = Person.Empty;
    private DateTime requestedDate = ExecutionServer.DateMaxValue;
    private DateTime promisedDate = ExecutionServer.DateMaxValue;
    private DateTime deliveryTime = ExecutionServer.DateMaxValue;
    private Product product = Product.Empty;
    private decimal quantity = decimal.Zero;
    private PresentationUnit presentationUnit = PresentationUnit.Empty;
    private string identificationTag = String.Empty;
    private string dutyEntryTag = String.Empty;
    private DateTime expirationDate = ExecutionServer.DateMaxValue;
    private int priceRuleId = -1;
    private PriceType priceType = PriceType.A;
    private int discountRuleId = -1;
    private PriceType discountType = PriceType.A;
    private decimal unitRepositionValue = decimal.Zero;
    private decimal productUnitPrice = decimal.Zero;
    private Currency currency = Currency.Default;
    private decimal productSubTotalInBaseCurrency = decimal.Zero;
    private decimal productSubTotal = decimal.Zero;
    private decimal productDiscount = decimal.Zero;
    private decimal productTaxes = decimal.Zero;
    private decimal productTotal = decimal.Zero;
    private decimal shippingSubTotal = decimal.Zero;
    private decimal shippingDiscount = decimal.Zero;
    private decimal shippingTaxes = decimal.Zero;
    private decimal shippingTotal = decimal.Zero;
    private int priceAuthorizationId = -1;
    private string keywords = String.Empty;

    private int parentItemId = -1;
    private SupplyOrderItem parentItem = null;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private OrderStatus status = OrderStatus.Opened;

    #endregion Fields

    #region Constructors and parsers

    private SupplyOrderItem() {
      // Required by Empiria Framework.
    }

    internal SupplyOrderItem(SupplyOrder order) {
      this.order = order;
    }

    static public SupplyOrderItem Parse(int id) {
      return BaseObject.ParseId<SupplyOrderItem>(id);
    }

    static public SupplyOrderItem Empty {
      get { return BaseObject.ParseEmpty<SupplyOrderItem>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public SupplyOrder Order {
      get { return order; }
    }

    public int OrderItemTypeId {
      get { return orderItemTypeId; }
      set { orderItemTypeId = value; }
    }

    public Contact SupplyPoint {
      get { return supplyPoint; }
      set { supplyPoint = value; }
    }

    public string Concept {
      get { return concept; }
      set { concept = value; }
    }

    public int ApplicationItemTypeId {
      get { return applicationItemTypeId; }
      set { applicationItemTypeId = value; }
    }

    public int ApplicationItemId {
      get { return applicationItemId; }
      set { applicationItemId = value; }
    }

    public Person Commissioner {
      get { return commissioner; }
      set { commissioner = value; }
    }

    public DateTime RequestedDate {
      get { return requestedDate; }
      set { requestedDate = value; }
    }

    public DateTime PromisedDate {
      get { return promisedDate; }
      set { promisedDate = value; }
    }

    public DateTime DeliveryTime {
      get { return deliveryTime; }
      set { deliveryTime = value; }
    }

    public Product Product {
      get { return product; }
      set { product = value; }
    }

    public decimal Quantity {
      get { return quantity; }
      set { quantity = value; }
    }

    public PresentationUnit PresentationUnit {
      get { return presentationUnit; }
      set { presentationUnit = value; }
    }

    public string IdentificationTag {
      get { return identificationTag; }
      set { identificationTag = value; }
    }

    public string DutyEntryTag {
      get { return dutyEntryTag; }
      set { dutyEntryTag = value; }
    }

    public DateTime ExpirationDate {
      get { return expirationDate; }
      set { expirationDate = value; }
    }

    public int PriceRuleId {
      get { return priceRuleId; }
      set { priceRuleId = value; }
    }

    public PriceType PriceType {
      get { return priceType; }
      set { priceType = value; }
    }

    public int DiscountRuleId {
      get { return discountRuleId; }
      set { discountRuleId = value; }
    }

    public PriceType DiscountType {
      get { return discountType; }
      set { discountType = value; }
    }

    public decimal UnitRepositionUnitValue {
      get { return unitRepositionValue; }
      set { unitRepositionValue = value; }
    }

    public decimal RepositionValue {
      get { return unitRepositionValue * quantity; }
    }

    public decimal ProductUnitPrice {
      get { return productUnitPrice; }
      set { productUnitPrice = value; }
    }

    public Currency Currency {
      get { return currency; }
      set { currency = value; }
    }

    public decimal ProductSubTotalInBaseCurrency {
      get { return productSubTotalInBaseCurrency; }
      set { productSubTotalInBaseCurrency = value; }
    }

    public decimal ProductSubTotal {
      get { return productSubTotal; }
      set { productSubTotal = value; }
    }

    public decimal ProductDiscount {
      get { return productDiscount; }
      set { productDiscount = value; }
    }

    public decimal ProductDiscountUnitPrice {
      get { return ProductSubTotalBeforeTaxes / quantity; }
    }

    public decimal ProductSubTotalBeforeTaxes {
      get { return productSubTotal - productDiscount; }
    }

    public decimal ProductTaxes {
      get { return productTaxes; }
      set { productTaxes = value; }
    }

    public decimal ProductTotal {
      get { return productTotal; }
      set { productTotal = value; }
    }

    public decimal ShippingSubTotal {
      get { return shippingSubTotal; }
      set { shippingSubTotal = value; }
    }

    public decimal ShippingDiscount {
      get { return shippingDiscount; }
      set { shippingDiscount = value; }
    }

    public decimal ShippingTaxes {
      get { return shippingTaxes; }
      set { shippingTaxes = value; }
    }

    public decimal ShippingTotal {
      get { return shippingTotal; }
      set { shippingTotal = value; }
    }

    public int PriceAuthorizationId {
      get { return priceAuthorizationId; }
      set { priceAuthorizationId = value; }
    }

    public decimal Taxes {
      get { return productTaxes + shippingTaxes; }
    }

    public decimal Total {
      get { return productTotal + shippingTotal; }
    }

    public string Keywords {
      get { return keywords; }
    }

    public SupplyOrderItem ParentItem {
      get {
        if (parentItem == null) {
          parentItem = SupplyOrderItem.Parse(parentItemId);
        }
        return parentItem;
      }
      set {
        parentItem = value;
        parentItemId = parentItem.Id;
      }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public DateTime PostingTime {
      get { return postingTime; }
    }

    public OrderStatus Status {
      get { return status; }
      set { status = value; }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnLoadObjectData(DataRow row) {
      this.order = SupplyOrder.Parse((int) row["SupplyOrderId"]);
      this.orderItemTypeId = (int) row["SupplyOrderItemTypeId"];
      this.supplyPoint = Contact.Parse((int) row["ItemSupplyPointId"]);
      this.concept = (string) row["SupplyOrderItemConcept"];
      this.applicationItemTypeId = (int) row["ApplicationItemTypeId"];
      this.applicationItemId = (int) row["ApplicationItemId"];
      this.commissioner = Person.Parse((int) row["CommissionerId"]);
      this.requestedDate = (DateTime) row["RequestedDate"];
      this.promisedDate = (DateTime) row["PromisedDate"];
      this.deliveryTime = (DateTime) row["DeliveryTime"];
      if (this.order.Status == OrderStatus.Opened) {
        this.product = BaseObject.ParseFull<Product>((int) row["ProductId"]);
      } else {
        this.product = Product.Parse((int) row["ProductId"]);
      }
      this.quantity = (decimal) row["Quantity"];
      this.presentationUnit = PresentationUnit.Parse((int) row["PresentationId"]);
      this.identificationTag = (string) row["IdentificationTag"];
      this.dutyEntryTag = (string) row["DutyEntryTag"];
      this.expirationDate = (DateTime) row["ExpirationDate"];
      this.priceRuleId = (int) row["PriceRuleId"];
      this.priceType = (PriceType) Convert.ToChar(row["PriceType"]);
      this.discountRuleId = (int) row["DiscountRuleId"];
      this.discountType = (PriceType) Convert.ToChar(row["DiscountType"]);
      this.unitRepositionValue = (decimal) row["RepositionValue"];
      this.productUnitPrice = (decimal) row["ProductUnitPrice"];
      this.currency = Currency.Parse((int) row["BaseCurrencyId"]);
      this.productSubTotalInBaseCurrency = (decimal) row["ProductSubtotalBaseCurrency"];
      this.productSubTotal = (decimal) row["ProductSubtotal"];
      this.productDiscount = (decimal) row["ProductDiscount"];
      this.productTaxes = (decimal) row["ProductTaxes"];
      this.productTotal = (decimal) row["ProductTotal"];
      this.shippingSubTotal = (decimal) row["ShippingSubtotal"];
      this.shippingDiscount = (decimal) row["ShippingDiscount"];
      this.shippingTaxes = (decimal) row["ShippingTaxes"];
      this.shippingTotal = (decimal) row["ShippingTotal"];
      this.priceAuthorizationId = (int) row["PriceAuthorizationId"];
      this.keywords = (string) row["SupplyOrderItemKeywords"];
      this.parentItemId = (int) row["ParentSupplyOrderItemId"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.postingTime = (DateTime) row["PostingTime"];
      this.status = (OrderStatus) Convert.ToChar(row["SupplyOrderItemStatus"]);
    }

    protected override void OnSave() {
      if (this.IsNew) {
        postedBy = Contact.Parse(ExecutionServer.CurrentUserId);
        postingTime = DateTime.Now;
      }
      SupplyOrdersData.WriteSupplyOrderItem(this);
      this.Order.Reset();
    }

    #endregion Public methods

  } // class SupplyOrderItem

} // namespace Empiria.Trade.Ordering
