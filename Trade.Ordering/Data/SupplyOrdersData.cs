/* Empiria® Trade 2013 ***************************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Trade                                   System   : Ordering System                   *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyOrderData                                  Pattern  : Data Services Static Class        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Database read and write methods for Empiria Trade® ordering services.                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.Trade.Ordering;

namespace Empiria.Trade.Data {

  /// <summary>Database read and write methods for warehousing management services.</summary>
  static public class SupplyOrdersData {

    #region Public methods

    static public int CreateSupplyOrderFromColdFusion(int coldFusionOrderId) {
      Empiria.Customers.Pineda.PedidoPineda.Update(coldFusionOrderId);

      DataOperation dataOperation = DataOperation.Parse("SELECT SupplyOrderId FROM SNMSupplyOrders WHERE ExternalOrderId = " + 
                                                        coldFusionOrderId.ToString());

      int supplyOrderId = DataReader.GetScalar<int>(dataOperation);
      if (supplyOrderId == 0) {
        supplyOrderId = DataWriter.CreateId("SNMSupplyOrders");
      }
      dataOperation = DataOperation.Parse("doImportCFSalesOrder", supplyOrderId, coldFusionOrderId);

      DataWriter.Execute(dataOperation);

      return supplyOrderId;
    }

    static public DataView GetSupplyOrdersItemsARPCrossed(SupplyOrder order) {
      return DataReader.GetDataView(DataOperation.Parse("qryARPSupplyOrderItemsARPCrossed", order.Id));
    }

    static public DataView GetCustomerOrders(Contact customer, string filter, string sort) {
      string sql = "SELECT * FROM tabSNMCustomerOrders(" + customer.Id + ")";
      if (filter.Length != 0 && sort.Length != 0) {
        sql += " WHERE " + filter + " ORDER BY " + sort;
      } else if (filter.Length != 0 && sort.Length == 0) {
        sql += " WHERE " + filter;
      } else if (filter.Length == 0 && sort.Length != 0) {
        sql += " ORDER BY " + sort;
      } else if (filter.Length == 0 && sort.Length == 0) {
        // no-op
      }
      return DataReader.GetDataView(DataOperation.Parse(sql));
    }

    static public DataView GetCustomerOrders(Contact customer, DateTime fromDate, DateTime toDate, string filter) {
      var dataOperation = DataOperation.Parse("qrySNMCustomerOrders", customer.Id, fromDate, toDate);

      return DataReader.GetDataView(dataOperation, filter);
    }

    static public DataView GetSuplierCustomerOrders(Contact supplier, Contact customer, 
                                                    DateTime fromDate, DateTime toDate, string filter) {
      var dataOperation = DataOperation.Parse("qrySNMSupplierCustomerOrders", 
                                              supplier.Id, customer.Id, fromDate, toDate);

      return DataReader.GetDataView(dataOperation, filter);
    }

    static public DataView GetSupplierOrders(Contact supplier, string filter, string sort) {
      string sql = "SELECT * FROM tabSNMSupplierOrders(" + supplier.Id + ")";
      if (filter.Length != 0 && sort.Length != 0) {
        sql += " WHERE " + filter + " ORDER BY " + sort;
      } else if (filter.Length != 0 && sort.Length == 0) {
        sql += " WHERE " + filter;
      } else if (filter.Length == 0 && sort.Length != 0) {
        sql += " ORDER BY " + sort;
      } else if (filter.Length == 0 && sort.Length == 0) {
        // no-op
      }
      return DataReader.GetDataView(DataOperation.Parse(sql));
    }

    static public DataView GetSupplierOrders(Contact supplier, DateTime fromDate, 
                                             DateTime toDate, string filter) {
      var dataOperation = DataOperation.Parse("qrySNMSupplierOrders", 
                                              supplier.Id, fromDate, toDate);

      return DataReader.GetDataView(dataOperation, filter);
    }

    static public DataView GetSalesInventory() {
      string sql = "SELECT * FROM vwARPArticulosConInventarioBajo " + 
                   "ORDER BY MarcaArticulo, NomArt, cveArticulo";

      return DataReader.GetDataView(DataOperation.Parse(sql));
    }

    #endregion Public methods

    #region Internal methods

    internal static SupplyOrderItemList GetSupplierOrderItems(SupplyOrder supplyOrder) {
      DataOperation dataOperation = DataOperation.Parse("SELECT * FROM SNMSupplyOrderItems WHERE SupplyOrderId = " + supplyOrder.Id.ToString());

      DataView view = DataReader.GetDataView(dataOperation);

      if (supplyOrder.Status == OrderStatus.Opened) {
        return new SupplyOrderItemList((x) => SupplyOrderItem.ParseFromBelow(x), view);
      } else {
        return new SupplyOrderItemList((x) => SupplyOrderItem.Parse(x), view);
      }
    }

    static internal int WriteSupplyOrder(SupplyOrder o) {
      DataOperation dataOperation = DataOperation.Parse("writeSNMSupplyOrder", o.Id, o.ObjectTypeInfo.Id,
                                          o.Number, o.CustomerOrderNumber, o.DutyEntryTag, o.Concept, o.SupplyChannel.Id,
                                          o.SupplyPoint.Id, o.Supplier.Id, o.SupplierContact.Id, o.Customer.Id, o.CustomerContact.Id,
                                          o.DeliveryMode.Id, o.DeliveryTo.Id, o.DeliveryPoint.Id, o.DeliveryContact.Id, o.DeliveryTime, o.DeliveryNotes,
                                          o.AuthorizationId, o.Currency.Id, o.OrderingTime, o.ClosedBy.Id, o.ClosingTime,
                                          o.CanceledBy.Id, o.CancelationTime, o.Keywords, o.Payment.Id, o.Bill.Id, o.ExternalOrderId,
                                          o.Parent.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    static internal int WriteSupplyOrderItem(SupplyOrderItem o) {
      //DataOperation dataOperation = DataOperation.Parse("writeSNMSupplyOrderItem", o.Id, o.Order.Id,
      //                                                  o.OrderItemTypeId, o.Concept, -1, o.ApplicationItemId,
      //                                                  o.RequestedDate, o.PromisedDate, o.DeliveryTime, o.Product.Id,
      //                                                  o.Quantity, o.ExpectedQuantity, o.InputQuantity, o.OutputQuantity,
      //                                                  o.ErrorsCount, o.PresentationUnit.Id, o.ContentsQty, o.ContentsUnit.Id,
      //                                                  o.IdentificationTag, o.DutyEntryTag, o.ExpirationDate, o.RepositionValue,
      //                                                  o.BinCube, o.Responsible.Id, o.AuthorizationId, o.Keywords, o.ParentWarehouseOrderItem.Id,
      //                                                  o.SupplyOrderItem.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);
      //return DataWriter.Execute(dataOperation);
      throw new NotImplementedException();
    }

    #endregion Internal methods

  } // class SupplyOrdersData

} // namespace Empiria.Trade.Data