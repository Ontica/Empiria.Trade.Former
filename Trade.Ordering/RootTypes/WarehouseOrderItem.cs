/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : WarehouseOrderItem                               Pattern  : Empiria Object Type               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a warehouse product order item in the Supply Management System.                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Products;

using Empiria.Trade.Data;

namespace Empiria.Trade.Ordering {

  /// <summary>Represents a warehouse product order item in the Supply Management System.</summary>
  public class WarehouseOrderItem : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.Trade.OrderItem.WarehouseOrderItem";

    private WarehouseOrder order = WarehouseOrder.Empty;
    private int orderItemTypeId = -1;
    private string concept = String.Empty;
    private int applicationItemTypeId = -1;
    private int applicationItemId = -1;
    private DateTime requestedDate = ExecutionServer.DateMaxValue;
    private DateTime promisedDate = ExecutionServer.DateMaxValue;
    private DateTime deliveryTime = ExecutionServer.DateMaxValue;
    private Product product = Product.Empty;
    private decimal quantity = decimal.Zero;
    private decimal expectedQuantity = decimal.Zero;
    private decimal inputQuantity = decimal.Zero;
    private decimal outputQuantity = decimal.Zero;
    private int errorsCount = 0;
    private PresentationUnit presentationUnit = PresentationUnit.Empty;
    private decimal contentsQty = decimal.Zero;
    private PresentationUnit contentsUnit = PresentationUnit.Empty;
    private string identificationTag = String.Empty;
    private string dutyEntryTag = String.Empty;
    private DateTime expirationDate = ExecutionServer.DateMaxValue;
    private decimal repositionValue = decimal.Zero;
    private string binCube = String.Empty;
    private Contact responsible = Person.Empty;
    private int authorizationId = -1;
    private string keywords = String.Empty;
    private int parentWarehouseOrderItemId = -1;
    private WarehouseOrderItem parentWarehouseOrderItem = null;
    private int supplyOrderItemId = -1;
    private WarehouseOrderItem supplyOrderItem = null;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private GeneralObjectStatus status = GeneralObjectStatus.Inactive;

    #endregion Fields

    #region Constuctors and parsers

    protected WarehouseOrderItem()
      : base(thisTypeName) {

    }

    protected WarehouseOrderItem(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    internal WarehouseOrderItem(WarehouseOrder order)
      : base(thisTypeName) {
      this.order = order;
    }

    static public WarehouseOrderItem Parse(int id) {
      return BaseObject.Parse<WarehouseOrderItem>(thisTypeName, id);
    }

    static internal WarehouseOrderItem Parse(DataRow dataRow) {
      return BaseObject.Parse<WarehouseOrderItem>(thisTypeName, dataRow);
    }

    static public WarehouseOrderItem Empty {
      get { return BaseObject.ParseEmpty<WarehouseOrderItem>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public int OrderItemTypeId {
      get { return orderItemTypeId; }
      set { orderItemTypeId = value; }
    }

    public WarehouseOrder Order {
      get { return order; }
    }

    public string Concept {
      get { return concept; }
      set { concept = value; }
    }

    public int ApplicationItemId {
      get { return applicationItemId; }
      set { applicationItemId = value; }
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

    public decimal ExpectedQuantity {
      get { return expectedQuantity; }
      set { expectedQuantity = value; }
    }

    public decimal InputQuantity {
      get { return inputQuantity; }
      set { inputQuantity = value; }
    }

    public decimal OutputQuantity {
      get { return outputQuantity; }
      set { outputQuantity = value; }
    }

    public int ErrorsCount {
      get { return errorsCount; }
      set { errorsCount = value; }
    }

    public PresentationUnit PresentationUnit {
      get { return presentationUnit; }
      set { presentationUnit = value; }
    }

    public decimal ContentsQty {
      get { return contentsQty; }
      set { contentsQty = value; }
    }

    public PresentationUnit ContentsUnit {
      get { return contentsUnit; }
      set { contentsUnit = value; }
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

    public decimal RepositionValue {
      get { return repositionValue; }
      set { repositionValue = value; }
    }

    public string BinCube {
      get { return binCube; }
      set { binCube = value; }
    }

    public Contact Responsible {
      get { return responsible; }
      set { responsible = value; }
    }

    public int AuthorizationId {
      get { return authorizationId; }
      set { authorizationId = value; }
    }

    public string Keywords {
      get { return keywords; }
    }

    public WarehouseOrderItem ParentWarehouseOrderItem {
      get {
        if (parentWarehouseOrderItem == null) {
          parentWarehouseOrderItem = WarehouseOrderItem.Parse(parentWarehouseOrderItemId);
        }
        return parentWarehouseOrderItem;
      }
      set {
        parentWarehouseOrderItem = value;
        parentWarehouseOrderItemId = parentWarehouseOrderItem.Id;
      }
    }

    public WarehouseOrderItem SupplyOrderItem {
      get {
        if (supplyOrderItem == null) {
          supplyOrderItem = WarehouseOrderItem.Parse(supplyOrderItemId);
        }
        return supplyOrderItem;
      }
      set {
        supplyOrderItem = value;
        supplyOrderItemId = supplyOrderItem.Id;
      }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public DateTime PostingTime {
      get { return postingTime; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    #endregion Public properties

    #region Public methods

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.order = WarehouseOrder.Parse((int) row["WarehouseOrderId"]);
      this.orderItemTypeId = (int) row["WarehouseOrderItemTypeId"];
      this.concept = (string) row["WarehouseOrderItemConcept"];
      this.applicationItemTypeId = (int) row["ApplicationItemTypeId"];
      this.applicationItemId = (int) row["ApplicationItemId"];
      this.requestedDate = (DateTime) row["RequestedDate"];
      this.promisedDate = (DateTime) row["PromisedDate"];
      this.deliveryTime = (DateTime) row["DeliveryTime"];
      this.product = Product.Parse((int) row["ProductId"]);
      this.quantity = (decimal) row["Quantity"];
      this.expectedQuantity = (decimal) row["ExpectedQuantity"];
      this.inputQuantity = (decimal) row["InputQuantity"];
      this.outputQuantity = (decimal) row["OutputQuantity"];
      this.errorsCount = (int) row["ErrorsCount"];
      this.presentationUnit = PresentationUnit.Parse((int) row["PresentationId"]);
      this.contentsQty = (decimal) row["ContentsQty"];
      this.contentsUnit = PresentationUnit.Parse((int) row["ContentsUnitId"]);
      this.identificationTag = (string) row["IdentificationTag"];
      this.dutyEntryTag = (string) row["DutyEntryTag"];
      this.expirationDate = (DateTime) row["ExpirationDate"];
      this.repositionValue = (decimal) row["RepositionValue"];
      this.binCube = (string) row["BinCube"];
      this.responsible = Contact.Parse((int) row["ResponsibleId"]);
      this.authorizationId = (int) row["AuthorizationId"];
      this.keywords = (string) row["WarehouseOrderItemKeywords"];
      this.parentWarehouseOrderItemId = (int) row["ParentWarehouseOrderItemId"];
      this.supplyOrderItemId = (int) row["SupplyOrderItemId"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.postingTime = (DateTime) row["PostingTime"];
      this.status = (GeneralObjectStatus) Convert.ToChar(row["WarehouseOrderItemStatus"]);
    }

    protected override void ImplementsSave() {
      postedBy = Contact.Parse(ExecutionServer.CurrentUserId);
      postingTime = DateTime.Now;

      WarehouseData.WriteWarehouseOrderItem(this);

      if (this.IdentificationTag.Length != 0 && this.Order.ObjectTypeInfo.Id == 2061) {
        product.BarCodeID = this.IdentificationTag;
        product.Save();
      }
      this.Order.Reset();
    }

    #endregion Public methods

  } // class WarehouseOrderItem

} // namespace Empiria.Trade.Ordering