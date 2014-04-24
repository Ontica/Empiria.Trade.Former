﻿/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : WarehouseData                                    Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Database read and write methods for warehousing management services.                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Trade.Ordering;

namespace Empiria.Trade.Data {

  /// <summary>Provides database read and write methods for product data management</summary>
  static public class WarehouseData {

    #region Public methods

    static public DataView GetCountingOrders() {
      return DataReader.GetDataView(DataOperation.Parse("SELECT * FROM vwSNMWarehouseOrders WHERE (WarehousingOperationNamedKey LIKE 'Counting.%' AND WarehouseOrderStatus <> 'X')"));
    }

    static public DataView GetInventory(DateTime fromDate, DateTime toDate, string keywords) {
      if (keywords.Length != 0) {
        keywords = SearchExpression.ParseAndLike("ProductKeywords", keywords);
      }
      return DataReader.GetDataView(DataOperation.Parse("qrySNMInventory", 1, fromDate, toDate), keywords);
    }

    static public DataView GetWarehouseOrdersItemsARPCrossed(WarehouseOrder order) {
      return DataReader.GetDataView(DataOperation.Parse("qryARPWarehouseOrdersItemsARPCrossed", order.Id));
    }

    static public DataView GetStorageProducts(string keywords) {
      string sql = "SELECT * FROM vwSNMWarehouseOrderItems";
      if (keywords.Length != 0) {
        sql += " WHERE " + SearchExpression.ParseAndLike("ProductKeywords", keywords);
      }
      return DataReader.GetDataView(DataOperation.Parse(sql));
    }

    static public FixedList<WarehouseOrderItem> GetWarehouseOrdersItems(WarehouseOrder order) {
      DataOperation op = DataOperation.Parse("qrySNMWarehouseOrderItems", order.Id);

      DataTable table = DataReader.GetDataTable(op);

      return new FixedList<WarehouseOrderItem>((x) => WarehouseOrderItem.Parse(x), table);
    }

    //static public FixedList<ProductGroup> GetProductGroups(string keywords) {
    //  string sql = "SELECT * FROM PLMProductGroups";

    //  string filter = SearchExpression.ParseAndLikeWithNoiseWords("ProductGroupKeywords", keywords);
    //  if (filter.Length != 0) {
    //    sql += " WHERE (" + filter + ") AND (ProductGroupStatus <> 'X')";
    //  } else {
    //    sql += " WHERE (ProductGroupStatus <> 'X')";
    //  }
    //  sql += " ORDER BY ProductGroupName";

    //  DataView view = DataReader.GetDataView(DataOperation.Parse(sql));

    //  return new FixedList<ProductGroup>((x) => ProductGroup.Parse(x), view);
    //}

    //static public FixedList<ProductGroup> GetProductGroups(ProductClass productTerm) {
    //  string sql = "SELECT * FROM PLMProductGroups INNER JOIN PLMProductGroupRules ";
    //  sql += "ON PLMProductGroups.ProductGroupId = PLMProductGroupRules.ProductGroupId ";
    //  sql += "WHERE PLMProductGroupRules.ProductTypeId = " + productTerm.Id.ToString() + " AND ";
    //  sql += "PLMProductGroupRules.ProductGroupRuleStatus <> 'X' AND PLMProductGroups.ProductGroupStatus <> 'X' ";
    //  sql += "ORDER BY PLMProductGroups.ProductGroupName";

    //  DataView view = DataReader.GetDataView(DataOperation.Parse(sql));

    //  return new FixedList<ProductGroup>((x) => ProductGroup.Parse(x), view);
    //}

    //static public FixedList<ProductGroup> GetProductGroupChilds(ProductGroup parentGroup) {
    //  DataView view = DataReader.GetDataView(DataOperation.Parse("qryPLMProductGroupChilds", 1, parentGroup.Id));

    //  return new FixedList<ProductGroup>((x) => ProductGroup.Parse(x), view);
    //}

    //static public FixedList<ProductGroupRule> GetProductGroupRules(ProductGroup productGroup) {
    //  DataView view = DataReader.GetDataView(DataOperation.Parse("qryPLMProductGroupRules", 1, productGroup.Id));

    //  FixedList<ProductGroupRule> list = new FixedList<ProductGroupRule>((x) => ProductGroupRule.Parse(x), view);

    //  list.Sort((x, y) => x.ProductTerm.Name.CompareTo(y.ProductTerm.Name));

    //  return list;
    //}

    #endregion Public methods

    #region Internal methods

    static internal int UpdateExpectedQuantitiesWithARPStock(WarehouseOrder order) {
      return DataWriter.Execute(DataOperation.Parse("doUpdateCountingWithARPStock", order.Id));
    }

    static internal int WriteWarehouseOrder(WarehouseOrder o) {
      DataOperation dataOperation = DataOperation.Parse("writeSNMWarehouseOrder", o.Id, o.ObjectTypeInfo.Id,
                                                        o.Operation.Id, o.Number, o.Concept, o.SupplyPoint.Id, o.StorageUnit.Id,
                                                        o.RequestedBy.Id, o.Responsible.Id, o.Supervisor.Id, o.OrderAuthorizationId,
                                                        o.OrderingTime, o.ClosingTime, o.Keywords, o.BaseSupplyOrder.Id, o.ParentWarehouseOrder.Id,
                                                        o.PostedBy.Id, o.PostingTime, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    static internal int WriteWarehouseOrderItem(WarehouseOrderItem o) {
      DataOperation dataOperation = DataOperation.Parse("writeSNMWarehouseOrderItem", o.Id, o.Order.Id,
                                                        o.OrderItemTypeId, o.Concept, -1, o.ApplicationItemId,
                                                        o.RequestedDate, o.PromisedDate, o.DeliveryTime, o.Product.Id,
                                                        o.Quantity, o.ExpectedQuantity, o.InputQuantity, o.OutputQuantity,
                                                        o.ErrorsCount, o.PresentationUnit.Id, o.ContentsQty, o.ContentsUnit.Id,
                                                        o.IdentificationTag, o.DutyEntryTag, o.ExpirationDate, o.RepositionValue,
                                                        o.BinCube, o.Responsible.Id, o.AuthorizationId, o.Keywords, o.ParentWarehouseOrderItem.Id,
                                                        o.SupplyOrderItem.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    #endregion Internal methods

  } // class WarehouseData

} // namespace Empiria.Trade.Data