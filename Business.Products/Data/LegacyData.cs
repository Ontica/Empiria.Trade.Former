/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products.Data                            Assembly : Empiria.Documents.dll             *
*  Type      : LegacyData                                       Pattern  : Data Services Static Class        *
*  Version   : 2.0                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides database read and write methods for product legacy data.                             *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Data.Convertion;

namespace Empiria.Products.Data {

  /// <summary>Provides database read and write methods for product legacy data.</summary>
  static internal class LegacyData {

    static private bool LegacyAppInstalled = ConfigurationData.GetBoolean("LegacyAppInstalled");

    #region Public methods

    static internal void InsertEquivalentProduct(Product baseProduct, Product equivalent) {
      if (!LegacyAppInstalled) {
        return;
      }

      DataConvertionEngine converter = DataConvertionEngine.GetInstance();
      converter.Initalize("Empiria", "Autopartes.MySQL");

      string sql1 = "INSERT INTO Equivalentes VALUES " +
                   "({0}, '{1}', {2}, '{3}', {4}, '', 1.00)";

      int nextID = converter.GetTargetIntegerValue("SELECT MAX(cveEquivalente) FROM Equivalentes");
      nextID++;
      sql1 = String.Format(sql1, nextID, baseProduct.PartNumber, baseProduct.Brand.LegacyId,
                          equivalent.PartNumber, equivalent.Brand.LegacyId);

      string sql2 = "UPDATE Articulos SET equivalente = 1 " +
                    "WHERE (cveArticulo = '{0}') AND (cveMarcaArticulo = {1})";

      sql2 = String.Format(sql2, equivalent.PartNumber, equivalent.Brand.LegacyId);

      converter.Execute(new string[] { sql1, sql2 });
    }

    internal static void RemoveEquivalentProduct(Product product, Product equivalentProduct) {
      if (!LegacyAppInstalled) {
        return;
      }

      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class LegacyData

} // namespace Empiria.Products.Data
