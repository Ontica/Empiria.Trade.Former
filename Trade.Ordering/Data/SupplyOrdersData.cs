/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Data                               Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyOrderData                                  Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Database read and write methods for Empiria Trade ordering services.                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      string sql = "SELECT * FROM tabSNMSupplierOrders(" + supplier.Id + ")" +
                    GeneralDataOperations.GetFilterSortSqlString(filter, sort);

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
    internal static FixedList<SupplyOrder> GetMyOrders(Organization org, Contact contact, string filter, string sort) {
      string sql = String.Format("SELECT * FROM tabSNMMyOrders({0},{1})", org.Id, contact.Id) +
                    GeneralDataOperations.GetFilterSortSqlString(filter, sort);
      Empiria.Messaging.Publisher.Publish(sql);
      var view = DataReader.GetDataView(DataOperation.Parse(sql));

      return new FixedList<SupplyOrder>((x) => SupplyOrder.Parse(x), view);
    }

    internal static SupplyOrderItemList GetSupplyOrderItems(SupplyOrder supplyOrder) {
      string sql = "SELECT * FROM SNMSupplyOrderItems WHERE SupplyOrderId = " + supplyOrder.Id;
      DataView view = DataReader.GetDataView(DataOperation.Parse(sql));

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
                                          o.DeliveryMode.Id, o.DeliveryTo.Id, o.DeliveryPoint.Id, o.DeliveryContact.Id, 
                                          o.DeliveryTime, o.DeliveryNotes, o.AuthorizationId, o.Currency.Id, o.OrderingTime, 
                                          o.ClosedBy.Id, o.ClosingTime, o.CanceledBy.Id, o.CancelationTime, o.Keywords, 
                                          o.Payment.Id, o.Bill.Id, o.ExternalOrderId, o.Parent.Id, 
                                          o.PostedBy.Id, o.PostingTime, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    static internal int WriteSupplyOrderItem(SupplyOrderItem o) {
      DataOperation dataOperation = DataOperation.Parse("writeSNMSupplyOrderItem", o.Id, o.Order.Id,
                                                        o.OrderItemTypeId, o.SupplyPoint.Id, o.Concept, o.ApplicationItemTypeId, 
                                                        o.ApplicationItemId, o.Commissioner.Id, o.RequestedDate, o.PromisedDate,
                                                        o.DeliveryTime, o.Product.Id, o.Quantity, o.PresentationUnit.Id, o.IdentificationTag,
                                                        o.DutyEntryTag, o.ExpirationDate, o.PriceRuleId, (char) o.PriceType, 
                                                        o.DiscountRuleId, (char) o.DiscountType, o.RepositionValue, o.ProductUnitPrice,
                                                        o.Order.Currency.Id, o.ProductSubTotalInBaseCurrency, o.ProductSubTotal,
                                                        o.ProductDiscount, o.ProductTaxes, o.ProductTotal, o.ShippingSubTotal,
                                                        o.ShippingDiscount, o.ShippingTaxes, o.ShippingTotal, o.PriceAuthorizationId,
                                                        o.Keywords, o.ParentItem.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    #endregion Internal methods


    static private Random random = new Random();

    static internal string GenerateOrderNumber() {
      string temp = String.Empty;
      int hashCode = 0;
      bool useLetters = false;
      for (int i = 0; i < 7; i++) {
        if (useLetters) {
          temp += GetRandomCharacter(random, temp);
          temp += GetRandomCharacter(random, temp);
        } else {
          temp += GetRandomDigit(random, temp);
          temp += GetRandomDigit(random, temp);
        }
        hashCode += ((Convert.ToInt32(temp[temp.Length - 2]) + Convert.ToInt32(temp[temp.Length - 1])) % ((int) Math.Pow(i + 1, 2)));
        useLetters = !useLetters;
      }
      string prefix = "TR";
      temp = prefix + temp.Substring(0, 4) + "-" + temp.Substring(4, 6) + "-" + temp.Substring(10, 4);

      hashCode = (hashCode * Convert.ToInt32(prefix[0])) % 49;
      hashCode = (hashCode * Convert.ToInt32(prefix[1])) % 53;

      temp += "ABCDEFGHJKMLNPQRSTUVWXYZ".Substring(hashCode % 24, 1);
      temp += "9A8B7C6D5E4F3G2H1JKR".Substring(hashCode % 20, 1);

      return temp;
    }

    static private char GetRandomCharacter(Random random, string current) {
      const string characters = "ABCDEFGHJKMLNPQRSTUVWXYZ";

      while (true) {
        char character = characters[random.Next(characters.Length)];
        if (!current.Contains(character.ToString())) {
          return character;
        }
      }
    }

    static private char GetRandomDigit(Random random, string current) {
      const string digits = "0123456789";

      while (true) {
        char digit = digits[random.Next(digits.Length)];
        if (!current.Contains(digit.ToString())) {
          return digit;
        }
      }
    }

    static private char GetRandomDigitOrCharacter(Random random, string current) {
      const string digitsAndCharacters = "A0B1C2D3E4F5G6H7J8K9MLNPQRSTUVWXYZ";

      while (true) {
        char character = digitsAndCharacters[random.Next(digitsAndCharacters.Length)];
        if (!current.Contains(character.ToString())) {
          return character;
        }
      }
    }

  } // class SupplyOrdersData

} // namespace Empiria.Trade.Data
