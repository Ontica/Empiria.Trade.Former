/* Empiria® Industries Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Industries Framework 2013               System   : Retail Industry Components        *
*  Namespace : Empiria.Industries.Retail.Data                   Assembly : Empiria.Industries.Retail.dll     *
*  Type      : StoreData                                        Pattern  : Data Services Static Class        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Provides data read methods for retail store data.                                             *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Industries.Retail.Data {

  /// <summary>Provides data read methods for retail store data.</summary>
  static public class StoreData {

    #region Fields

    static private DataTable productsTable = null;

    #endregion Fields

    #region Public methods

    static public DataView GetProductsDataView(string keywords) {
      productsTable = GetProductsDataTable();
      string[] expressions = keywords.Split(' ');
      string filter = String.Empty;
      for (int i = 0; i < expressions.Length; i++) {
        if (filter == String.Empty) {
          filter = "(ProductKeywords) LIKE '%" + expressions[i] + "%'";
        } else {
          filter += " AND (ProductKeywords) LIKE '%" + expressions[i] + "%'";
        }
      }
      string sort = "ProductName, PartNumber";
      return new DataView(productsTable, filter, sort, DataViewRowState.CurrentRows);
    }

    static public void Refresh() {
      productsTable = null;
    }

    #endregion Public methods

    #region Private methods

    static private DataTable GetProductsDataTable() {
      if (productsTable == null) {
        productsTable = DataReader.GetDataTable(DataOperation.Parse("SELECT * FROM vwPLMProducts"));
      }
      return productsTable;
    }

    #endregion Private methods

  } // class StoreData

} // namespace Empiria.Industries.Retail.Data