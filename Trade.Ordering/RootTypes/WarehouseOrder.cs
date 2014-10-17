/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : WarehouseOrder                                   Pattern  : Empiria Object Type               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a warehouse product order in the Supply Management System.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data.Convertion;

using Empiria.Trade.Data;

namespace Empiria.Trade.Ordering {

  /// <summary>Represents a warehouse product order in the Supply Management System.</summary>
  public class WarehouseOrder : BaseObject {

    #region Fields

    static private readonly bool LegacyAppInstalled = ConfigurationData.GetBoolean("LegacyAppInstalled");

    private const string newOrderNumber = "Nueva orden";

    private WarehousingOperation operation = WarehousingOperation.Empty;
    private string number = newOrderNumber;
    private string concept = String.Empty;
    private Contact supplyPoint = Person.Empty;
    private StorageUnit storageUnit = StorageUnit.Empty;
    private Contact requestedBy = Person.Empty;
    private Contact responsible = Person.Empty;
    private Contact supervisor = Person.Empty;
    private int orderAuthorizationId = -1;
    private DateTime orderingTime = DateTime.Now;
    private DateTime closingTime = ExecutionServer.DateMaxValue;
    private string keywords = String.Empty;
    private int baseSupplyOrderId = -1;
    private WarehouseOrder baseSupplyOrder = null;
    private int parentWarehouseOrderId = -1;
    private WarehouseOrder parentWarehouseOrder = null;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private GeneralActivityStatus status = GeneralActivityStatus.Inactive;

    private FixedList<WarehouseOrderItem> items = null;

    #endregion Fields

    #region Constructors and parsers

    private WarehouseOrder() {
      // Required by Empiria Framework.
    }

    static public WarehouseOrder Parse(int id) {
      return BaseObject.ParseId<WarehouseOrder>(id);
    }

    static public WarehouseOrder Empty {
      get { return BaseObject.ParseEmpty<WarehouseOrder>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public WarehousingOperation Operation {
      get { return operation; }
      set { operation = value; }
    }

    public string Number {
      get { return number; }
      set { number = value; }
    }

    public string Concept {
      get { return concept; }
      set { concept = value; }
    }

    public Contact SupplyPoint {
      get { return supplyPoint; }
      set { supplyPoint = value; }
    }

    public StorageUnit StorageUnit {
      get { return storageUnit; }
      set { storageUnit = value; }
    }

    public Contact RequestedBy {
      get { return requestedBy; }
      set { requestedBy = value; }
    }

    public Contact Responsible {
      get { return responsible; }
      set { responsible = value; }
    }

    public Contact Supervisor {
      get { return supervisor; }
      set { supervisor = value; }
    }

    public int OrderAuthorizationId {
      get { return orderAuthorizationId; }
      set { orderAuthorizationId = value; }
    }

    public DateTime OrderingTime {
      get { return orderingTime; }
      set { orderingTime = value; }
    }

    public DateTime ClosingTime {
      get { return closingTime; }
      set { closingTime = value; }
    }

    public string Keywords {
      get { return keywords; }
      set { keywords = value; }
    }

    public WarehouseOrder BaseSupplyOrder {
      get {
        if (baseSupplyOrder == null) {
          baseSupplyOrder = WarehouseOrder.Parse(baseSupplyOrderId);
        }
        return baseSupplyOrder;
      }
      set {
        baseSupplyOrder = value;
        baseSupplyOrderId = baseSupplyOrder.Id;
      }
    }

    public WarehouseOrder ParentWarehouseOrder {
      get {
        if (parentWarehouseOrder == null) {
          parentWarehouseOrder = WarehouseOrder.Parse(parentWarehouseOrderId);
        }
        return parentWarehouseOrder;
      }
      set {
        parentWarehouseOrder = value;
        parentWarehouseOrderId = parentWarehouseOrder.Id;
      }
    }

    public DateTime PostingTime {
      get { return postingTime; }
      set { postingTime = value; }
    }

    public Contact PostedBy {
      get { return postedBy; }
      set { postedBy = value; }
    }

    public GeneralActivityStatus Status {
      get { return status; }
      set { status = value; }
    }

    public FixedList<WarehouseOrderItem> Items {
      get {
        if (items == null) {
          items = WarehouseData.GetWarehouseOrdersItems(this);
        }
        return items;
      }
    }

    public void Reset() {
      items = null;
    }

    #endregion Public properties

    #region Public methods

    public void CloseCounting() {
      if (this.Operation.Id != 851 && this.Operation.Id != 852) {
        return;
      }
      if (this.Operation.Id == 851) {
        GetColdFusionProducts();
        UpdateExpectedQuantities();
        UpdateInventory();
        UpdateColdFusionStock();
      } else {
        UpdateExpectedQuantities();
      }
      this.Status = GeneralActivityStatus.Completed;
      this.closingTime = DateTime.Now;
      this.Save();
    }

    public void CloseCountingForRevision() {
      if (this.Operation.Id != 851 && this.Operation.Id != 852) {
        return;
      }
      if ((this.Status == GeneralActivityStatus.Deleted) || (this.Status == GeneralActivityStatus.Completed)) {
        return;
      }
      if (this.Operation.Id == 851) {
        GetColdFusionProducts();
      }
      UpdateExpectedQuantities();
      this.Status = GeneralActivityStatus.Deleted;
      this.closingTime = DateTime.Now;
      this.Save();
    }

    private void UpdateExpectedQuantities() {
      WarehouseData.UpdateExpectedQuantitiesWithARPStock(this);
    }

    private void UpdateInventory() {

    }

    private void GetColdFusionProducts() {
      if (!LegacyAppInstalled) {
        return;
      }
      DataConvertionEngine converter = DataConvertionEngine.GetInstance();
      converter.Initalize("Autopartes.MySQL", "Empiria");

      converter.ReplaceDataSetSync("Productos");
    }

    private void UpdateColdFusionStock() {
      if (!LegacyAppInstalled) {
        return;
      }

      DataConvertionEngine converter = DataConvertionEngine.GetInstance();
      converter.Initalize("Empiria", "Autopartes.MySQL");

      DataView view = WarehouseData.GetWarehouseOrdersItemsARPCrossed(this);
      if (view.Count == 0) {
        return;
      }
      string[] sqlArray = new string[view.Count];
      for (int i = 0; i < view.Count; i++) {
        string temp = "UPDATE Articulos SET Existencia = " + Convert.ToInt32((decimal) view[i]["Quantity"]) + " " +
                      "WHERE (cveArticulo = '" + (string) view[i]["cveArticulo"] + "') AND " +
                      "(cveMarcaArticulo = " + ((int) view[i]["cveMarcaArticulo"]).ToString() + ")";

        sqlArray[i] = temp;
      }
      converter.Execute(sqlArray);
    }

    public WarehouseOrderItem CreateItem() {
      return new WarehouseOrderItem(this);
    }

    public WarehouseOrder GenerateSample() {
      return new WarehouseOrder();
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.operation = WarehousingOperation.Parse((int) row["WarehousingOperationId"]);
      this.number = (string) row["WarehouseOrderNumber"];
      this.concept = (string) row["WarehouseOrderConcept"];
      this.supplyPoint = Contact.Parse((int) row["SupplyPointId"]);
      this.storageUnit = StorageUnit.Parse((int) row["StorageUnitId"]);
      this.requestedBy = Contact.Parse((int) row["RequestedById"]);
      this.responsible = Contact.Parse((int) row["ResponsibleId"]);
      this.supervisor = Contact.Parse((int) row["SupervisorId"]);
      this.orderAuthorizationId = (int) row["OrderAuthorizationId"];
      this.orderingTime = (DateTime) row["OrderingTime"];
      this.closingTime = (DateTime) row["ClosingTime"];
      this.keywords = (string) row["WarehouseOrderKeywords"];
      this.baseSupplyOrderId = (int) row["BaseSupplyOrderId"];
      this.parentWarehouseOrderId = (int) row["ParentWarehouseOrderId"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.postingTime = (DateTime) row["PostingTime"];
      this.status = (GeneralActivityStatus) Convert.ToChar(row["WarehouseOrderStatus"]);
    }

    protected override void OnSave() {
      this.keywords = EmpiriaString.BuildKeywords(number, concept, operation.Name, 
                                                  storageUnit.FullCode, storageUnit.Description);
      WarehouseData.WriteWarehouseOrder(this);
      this.Reset();
    }

    #endregion Public methods

  } // class WarehouseOrder

} // namespace Empiria.Trade.Ordering
